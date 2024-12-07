using System.Diagnostics;
using JAM7.Utilities;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.TensorExtensionMethods;

namespace JAM7.DeepLearning
{
    public class NDimRegressionModel : Module<Tensor, Tensor>
    {
        readonly Module<Tensor, Tensor> layer1 = Linear(40, 25);
        readonly Module<Tensor, Tensor> layer2 = Linear(25, 20);
        readonly Module<Tensor, Tensor> layer4 = Linear(20, 3);

        public NDimRegressionModel(string path) : this()
        {
            this.load(path);
        }
        public NDimRegressionModel() : base(nameof(NDimRegressionModel))
        {
            RegisterComponents();
        }

        public override Tensor forward(Tensor x)
        {
            x = layer1.forward(x);
            x = functional.relu(layer2.forward(x));
            x = layer4.forward(x);
            return x;
        }
    }
    public class NDimRegression
    {
        public static void 多维数据的拟合训练并保存()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "csv文件|*.csv"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            var (_, csv) = CSVHelper.csv_to_array(ofd.FileName);
            var input = ArrayHelper.Get2dArray_Cols<string>(csv, Utilities.MyGenerator.range(0, 40, 1).ToArray());
            var output = ArrayHelper.Get2dArray_Cols(csv, new int[3]
            { 40, 41, 42 });

            float[,] input_dType = ArrayHelper.convert_to_float(input);
            Tensor t_input = tensor(input_dType);
            t_input = t_input.to(CUDA);
            //t_input.print();

            float[,] output_dType = ArrayHelper.convert_to_float(output);
            Tensor t_output = tensor(output_dType);
            t_output = t_output.to(CUDA);
            //t_output.print();

            var model = new NDimRegressionModel();
            model.to(CUDA);
            model.train();//设置为训练模式

            var optimizer = optim.SGD(model.parameters(), 0.00001);

            int N = 50000;
            Tensor? prediction = null;
            for (int i = 0; i < N; i++)
            {
                //Console.WriteLine("*********************************************");
                prediction = model.forward(t_input);
                //prediction.print();
                var loss = functional.mse_loss(prediction, t_output);
                Console.WriteLine(loss.data<float>().First());

                optimizer.zero_grad();
                loss.backward();
                optimizer.step();
            }

            prediction?.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
            SaveFileDialog sfd = new()
            {
                Filter = "torchsharp模型|*.tsm"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            model.save(sfd.FileName);
        }

        public static void 导入多维数据拟合模型并测试()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "torchsharp模型|*.tsm"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            var model = new NDimRegressionModel(ofd.FileName);
            //model.to(CUDA);
            model.eval();

            var (_, csv) = CSVHelper.csv_to_array(FileDialogHelper.OpenCSV());
            var input = ArrayHelper.Get2dArray_Cols<string>(csv, Utilities.MyGenerator.range(0, 40, 1).ToArray());
            float[,] input_dType = ArrayHelper.convert_to_float(input);
            Tensor t_input = tensor(input_dType);
            //t_input = t_input.to(CUDA);
            t_input.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
            Stopwatch sw = new();
            sw.Start();
            var prediction = model.forward(t_input);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            prediction.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
        }

        public static NDimRegressionModel 多维数据的拟合(float[,] input, float[,] output)
        {
            Tensor t_input = tensor(input);
            Tensor t_output = tensor(output);
            var model = new NDimRegressionModel();
            model.train();//设置为训练模式
            var optimizer = optim.SGD(model.parameters(), 0.0001);

            int N = 500000;
            Tensor? prediction = null;
            for (int i = 0; i < N; i++)
            {
                //Console.WriteLine("*********************************************");
                prediction = model.forward(t_input);
                //prediction.print();
                var loss = functional.mse_loss(prediction, t_output);
                Console.WriteLine(loss.data<float>().First());

                optimizer.zero_grad();
                loss.backward();
                optimizer.step();
            }
            prediction?.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
            return model;
        }
    }

    public class NDdim_regression
    {
        public static void train_and_predict()
        {
            MyDataFrame df_train = MyDataFrame.read_from_excel();
            float[,] input = df_train.get_series_subset(new List<int>() { 0, 1, 2, 3, 4, 5, 6 })
                .convert_to_float_2dArray();
            float[,] output = df_train.get_series_subset(new List<int>() { 7 })
                .convert_to_float_2dArray();
            Tensor t_input = tensor(input);
            t_input = t_input.to(CUDA);
            t_input.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
            Tensor t_output = tensor(output);
            t_output = t_output.to(CUDA);
            t_output.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);

            //var modules = new List<(string, Module<Tensor, Tensor>)>
            //{
            //    ("lin1", Linear(7, 20)),
            //    ("relu1", ReLU()),
            //    ("lin2", Linear(20, 20)),
            //    ("relu1", ReLU()),
            //    ("lin2", Linear(20, 1)),
            //};

            //var model = Sequential(modules);
            //model.to(CUDA);
            //model.train();//设置为训练模式
            //var optimizer = optim.SGD(model.parameters(), 0.01);
            //int N = 20000;
            //Tensor? prediction = null;
            //for (int i = 0; i < N; i++)
            //{
            //    //Console.WriteLine("*********************************************");
            //    prediction = model.forward(t_input);
            //    var loss = functional.mse_loss(prediction, t_output);
            //    Console.WriteLine(loss.data<float>().First());
            //    optimizer.zero_grad();//梯度清零
            //    loss.backward();//计算梯度
            //    optimizer.step();//更新参数
            //}

            ////test
            //model.eval();
            //var result = model.forward(t_input);
            //result.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);


            var modules = new List<(string, Module<Tensor, Tensor>)>
            {
                ("lin1", Linear(7, 20)),
                ("relu1", ReLU(true)),
                ("lin2", Linear(20, 1)),
            };
            var net = Sequential(modules);

            net.to(CUDA);
            net.train();//设置为训练模式
            var optimizer = optim.SGD(net.parameters(), 0.01);
            int N = 20000;
            var func_loss = CrossEntropyLoss();
            Tensor? prediction = null;
            for (int i = 0; i < N; i++)
            {
                //Console.WriteLine("*********************************************");
                prediction = net.forward(t_input);
                //t_input.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
                //t_output.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
                //prediction.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);

                var loss = func_loss.forward(prediction, t_output);
                Console.WriteLine(loss.data<float>().First());
                optimizer.zero_grad();//梯度清零
                loss.backward();//计算梯度
                optimizer.step();//更新参数
            }

            //test
            net.eval();
            var result = net.forward(t_input);
            result.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
        }
    }

}
