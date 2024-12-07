using System.Diagnostics;
using TorchSharp;
using TorchSharp.Examples;
using TorchSharp.Utils;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using transforms = TorchSharp.torchvision.transforms;

namespace JAM7.DeepLearning
{
    internal class MNIST多分类
    {
        private static int _epochs = 4;
        private static int _trainBatchSize = 64;
        private static int _testBatchSize = 128;

        private readonly static int _logInterval = 100;

        public static void Run(int epochs, int timeout, string logdir, string dataset)
        {
            _epochs = epochs;

            if (string.IsNullOrEmpty(dataset))
            {
                dataset = "mnist";
            }

            var device = cuda.is_available() ? CUDA : CPU;

            Console.WriteLine();
            Console.WriteLine($"\tRunning MNIST with {dataset} on {device.type.ToString()} for {epochs} epochs, terminating after {TimeSpan.FromSeconds(timeout)}.");
            Console.WriteLine();

            var datasetPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "..", "Downloads", dataset);

            random.manual_seed(1);

            var cwd = Environment.CurrentDirectory;

            var writer = String.IsNullOrEmpty(logdir) ? null : torch.utils.tensorboard.SummaryWriter(logdir, createRunName: true);

            var sourceDir = datasetPath;
            var targetDir = Path.Combine(datasetPath, "test_data");

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                Decompress.DecompressGZipFile(Path.Combine(sourceDir, "train-images-idx3-ubyte.gz"), targetDir);
                Decompress.DecompressGZipFile(Path.Combine(sourceDir, "train-labels-idx1-ubyte.gz"), targetDir);
                Decompress.DecompressGZipFile(Path.Combine(sourceDir, "t10k-images-idx3-ubyte.gz"), targetDir);
                Decompress.DecompressGZipFile(Path.Combine(sourceDir, "t10k-labels-idx1-ubyte.gz"), targetDir);
            }

            if (device.type == DeviceType.CUDA)
            {
                _trainBatchSize *= 4;
                _testBatchSize *= 4;
            }

            Console.WriteLine($"\tCreating the model...");

            var model = new Model("model", device);

            var normImage = transforms.Normalize(new double[] { 0.1307 }, new double[] { 0.3081 }, device: (Device)device);

            Console.WriteLine($"\tPreparing training and test data...");
            Console.WriteLine();

            using (MNISTReader train = new MNISTReader(targetDir, "train", _trainBatchSize, device: device, shuffle: true, transform: normImage),
                                test = new MNISTReader(targetDir, "t10k", _testBatchSize, device: device, transform: normImage))
            {

                TrainingLoop(dataset, timeout, writer, device, model, train, test);
            }
        }

        internal static void TrainingLoop(string dataset, int timeout, TorchSharp.Modules.SummaryWriter writer, Device device, Module<Tensor, Tensor> model, MNISTReader train, MNISTReader test)
        {
            var optimizer = optim.Adam(model.parameters());

            var scheduler = optim.lr_scheduler.StepLR(optimizer, 1, 0.7);

            Stopwatch totalTime = new Stopwatch();
            totalTime.Start();

            for (var epoch = 1; epoch <= _epochs; epoch++)
            {

                Train(model, optimizer, NLLLoss(reduction: Reduction.Mean), device, train, epoch, train.BatchSize, train.Size);
                Test(model, NLLLoss(reduction: nn.Reduction.Sum), writer, device, test, epoch, test.Size);

                Console.WriteLine($"End-of-epoch memory use: {GC.GetTotalMemory(false)}");

                if (totalTime.Elapsed.TotalSeconds > timeout) break;
            }

            totalTime.Stop();
            Console.WriteLine($"Elapsed time: {totalTime.Elapsed.TotalSeconds:F1} s.");

            Console.WriteLine("Saving model to '{0}'", dataset + ".model.bin");
            model.save(dataset + ".model.bin");
        }

        private static void Train(
            Module<Tensor, Tensor> model,
            optim.Optimizer optimizer,
            Loss<Tensor, Tensor, Tensor> loss,
            Device device,
            IEnumerable<(Tensor, Tensor)> dataLoader,
            int epoch,
            long batchSize,
            int size)
        {
            model.train();

            int batchId = 1;

            Console.WriteLine($"Epoch: {epoch}...");

            foreach (var (data, target) in dataLoader)
            {
                using (var d = torch.NewDisposeScope())
                {
                    optimizer.zero_grad();

                    var prediction = model.forward(data);
                    var output = loss.forward(prediction, target);

                    output.backward();

                    optimizer.step();

                    if (batchId % _logInterval == 0)
                    {
                        Console.WriteLine($"\rTrain: epoch {epoch} [{batchId * batchSize} / {size}] Loss: {output.ToSingle():F4}");
                    }

                    batchId++;
                }

            }
        }

        private static void Test(
            Module<Tensor, Tensor> model,
            Loss<Tensor, Tensor, Tensor> loss,
            TorchSharp.Modules.SummaryWriter writer,
            Device device,
            IEnumerable<(Tensor, Tensor)> dataLoader,
            int epoch,
            int size)
        {
            model.eval();

            double testLoss = 0;
            int correct = 0;

            foreach (var (data, target) in dataLoader)
            {
                using (var d = torch.NewDisposeScope())
                {
                    var prediction = model.forward(data);
                    var output = loss.forward(prediction, target);
                    testLoss += output.ToSingle();

                    correct += prediction.argmax(1).eq(target).sum().ToInt32();
                }
            }

            Console.WriteLine($"Size: {size}, Total: {size}");

            Console.WriteLine($"\rTest set: Average loss {(testLoss / size):F4} | Accuracy {((double)correct / size):P2}");

            if (writer != null)
            {
                writer.add_scalar("MNIST/loss", (float)(testLoss / size), epoch);
                writer.add_scalar("MNIST/accuracy", (float)correct / size, epoch);
            }
        }
    }

    public class Model : Module<Tensor, Tensor>
    {
        private Module<Tensor, Tensor> conv1 = Conv2d(1, 32, 3);
        private Module<Tensor, Tensor> conv2 = Conv2d(32, 64, 3);
        private Module<Tensor, Tensor> fc1 = Linear(9216, 128);
        private Module<Tensor, Tensor> fc2 = Linear(128, 10);

        // These don't have any parameters, so the only reason to instantiate
        // them is performance, since they will be used over and over.
        private Module<Tensor, Tensor> pool1 = MaxPool2d(kernelSize: new long[] { 2, 2 });

        private Module<Tensor, Tensor> relu1 = ReLU();
        private Module<Tensor, Tensor> relu2 = ReLU();
        private Module<Tensor, Tensor> relu3 = ReLU();

        private Module<Tensor, Tensor> dropout1 = Dropout(0.25);
        private Module<Tensor, Tensor> dropout2 = Dropout(0.5);

        private Module<Tensor, Tensor> flatten = Flatten();
        private Module<Tensor, Tensor> logsm = LogSoftmax(1);

        public Model(string name, torch.Device device = null) : base(name)
        {
            RegisterComponents();

            if (device != null && device.type == DeviceType.CUDA)
                this.to(device);
        }

        public override Tensor forward(Tensor input)
        {
            var l11 = conv1.forward(input);
            var l12 = relu1.forward(l11);

            var l21 = conv2.forward(l12);
            var l22 = relu2.forward(l21);
            var l23 = pool1.forward(l22);
            var l24 = dropout1.forward(l23);

            var x = flatten.forward(l24);

            var l31 = fc1.forward(x);
            var l32 = relu3.forward(l31);
            var l33 = dropout2.forward(l32);

            var l41 = fc2.forward(l33);

            return logsm.forward(l41);
        }
    }
}
