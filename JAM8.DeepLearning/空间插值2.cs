using JAM7.Algorithms.Geometry;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace JAM7.DeepLearning
{
    public class Model_空间插值2 : Module<Tensor, Tensor>
    {
        readonly Module<Tensor, Tensor> layer1 = Linear(2, 100);
        readonly Module<Tensor, Tensor> layer2 = Linear(100, 100);
        readonly Module<Tensor, Tensor> layer3 = Linear(100, 100);
        readonly Module<Tensor, Tensor> layer4 = Linear(100, 100);
        readonly Module<Tensor, Tensor> layer5 = Linear(100, 1);

        public Model_空间插值2(string path) : this()
        {
            this.load(path);
        }
        public Model_空间插值2() : base(nameof(NDimRegressionModel))
        {
            RegisterComponents();
        }

        public override Tensor forward(Tensor x)
        {
            x = layer1.forward(x);
            x = functional.relu(layer2.forward(x));
            x = functional.relu(layer3.forward(x));
            x = functional.relu(layer4.forward(x));
            x = layer5.forward(x);
            return x;
        }
    }
    class 空间插值2
    {
        public static void run()
        {
            var gs = GridStructure.create_win();
            var (cd, _) = CData.read_from_gslibwin();
            var (grid_assigned, cd_assigned) = cd.assign_to_grid(gs);
            var gp = grid_assigned[0];
            grid_assigned.showGrid_win();

            var df_cd = cd_assigned.convert_to_dataFrame();
            var input_cd = df_cd.get_series_subset(new List<int>() { 0, 1 }).convert_to_float_2dArray();
            var output_cd = df_cd.get_series_subset(new List<int>() { 2 }).convert_to_float_2dArray();
            var model = 空间插值2.多维数据的拟合(input_cd, output_cd);
            model.eval();
            for (int n = 1; n <= gs.N; n++)
            {
                if (gp.get_value(n) == null)
                {
                    var si = gs.get_spatialIndex(n);
                    float[,]? input = new float[1, 2];
                    input[0, 0] = si.ix;
                    input[0, 1] = si.iy;
                    Tensor? prediction = model.forward(tensor(input));
                    TorchSharp.Utils.TensorAccessor<float> ta = prediction.data<float>();
                    float[] result = ta.ToArray();
                    gp.set_value(n, result[0]);
                }
            }
        }

        public static Model_空间插值2 多维数据的拟合(float[,] input, float[,] output)
        {
            Tensor t_input = tensor(input);
            Tensor t_output = tensor(output);
            var model = new Model_空间插值2();
            model.train();//设置为训练模式
            var optimizer = optim.SGD(model.parameters(), 0.001);

            int N = 100000;
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
}
