using JAM7.Algorithms.Geometry;
using JAM7.Algorithms.Numerics;
using JAM7.Utilities;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace JAM7.DeepLearning
{
    public class Model_空间插值3 : Module<Tensor, Tensor>
    {
        readonly Module<Tensor, Tensor> layer1;
        readonly Module<Tensor, Tensor> layer2 = Linear(50, 100);
        readonly Module<Tensor, Tensor> layer3 = Linear(100, 200);
        readonly Module<Tensor, Tensor> layer4 = Linear(200, 100);
        readonly Module<Tensor, Tensor> layer5 = Linear(100, 1);

        public Model_空间插值3(string path, int N_input) : this(N_input)
        {
            this.load(path);
        }
        public Model_空间插值3(int N_input) : base(nameof(Model_空间插值3))
        {
            layer1 = Linear(N_input, 50);
            RegisterComponents();
        }

        public override Tensor forward(Tensor x)
        {
            x = layer1.forward(x);
            x = functional.relu(layer2.forward(x));
            x = functional.relu(layer3.forward(x));
            x = functional.relu(layer4.forward(x));
            x = layer5.forward(x).sigmoid();
            return x;
        }
    }
    internal class 空间插值3
    {
        public static void run()
        {
            var gs = GridStructure.create_win();//工区网格结构
            var (cd, _) = CData.read_from_gslibwin();//加载条件数据

            var (g_assigned, cd_assigned) = cd.assign_to_grid(gs);//将条件数据赋给网格结构上，可能有部分数据因为越界丢失

            DataMapper dm = new();//数据映射
            var (max, min, _, _) = cd_assigned.get_statistics("value");
            dm.Reset(max, min, 0, 1);

            var cd_assigned_归一化 = cd_assigned.deep_clone();//归一化cd
            foreach (var cdi in cd_assigned_归一化)
                cdi["value"] = (float)dm.MapAToB(cdi["value"].Value);

            g_assigned.add_gridProperty("interpolation");
            g_assigned.showGrid_win();

            List<float?> output = new();
            List<List<float?>> input = new();
            foreach (var cdi in cd_assigned_归一化)
            {
                var cd_order_distance = cd_assigned_归一化.order_by_distance(cdi.coord);
                List<float?> tmp = new();

                foreach (var item in cd_order_distance)
                {
                    if (item.distance == 0)
                    {
                        output.Add(item.cdi["value"]);
                    }
                    tmp.Add(item.cdi["value"]);
                    tmp.Add((float?)item.distance);
                    tmp.Add(cdi.coord.x);
                    tmp.Add(cdi.coord.y);
                }
                input.Add(tmp);
            }
            MyDataFrame mydf_input = MyDataFrame.create(input[0].Count);
            for (int i = 0; i < input.Count; i++)
            {
                var record = input[i].Select(a => (object)a.Value).ToArray();
                mydf_input.add_record(record);
            }
            MyDataFrame mydf_output = MyDataFrame.create(1);
            for (int i = 0; i < output.Count; i++)
            {
                var record = output[i];
                mydf_output.add_record(new object[] { record });
            }
            var output_array = mydf_output.convert_to_float_2dArray();
            var input_array = mydf_input.convert_to_float_2dArray();

            Model_空间插值3 model = null;
            Console.WriteLine("输入1:打开训练的模型 输入2:训练新模型");
            string s = Console.ReadLine();
            if (s == "2")
            {
                model = 多维数据的拟合(input_array, output_array);
                model.save("D:\\空间插值3_model.tsm");
            }
            if (s == "1")
            {
                OpenFileDialog ofd = new();
                ofd.ShowDialog();
                model = new Model_空间插值3(ofd.FileName, input_array.GetLength(1));
                model.to(CUDA);
            }

            model.eval();//计算
            for (int n = 1; n <= gs.N; n++)
            {
                input = new List<List<float?>>();
                Coord coord = gs.arrayIndex_to_coord(n);
                var cd_order_distance2 = cd_assigned_归一化.order_by_distance(coord);
                List<float?> tmp = new();

                foreach (var (distance, cdi) in cd_order_distance2)
                {
                    tmp.Add(cdi["value"]);
                    tmp.Add((float?)distance);
                    tmp.Add(coord.x);
                    tmp.Add(coord.y);
                }
                input.Add(tmp);
                mydf_input = MyDataFrame.create(input[0].Count);
                for (int i = 0; i < input.Count; i++)
                {
                    var record = input[i].Select(a => (object)a.Value).ToArray();
                    mydf_input.add_record(record);
                }
                input_array = mydf_input.convert_to_float_2dArray();
                Tensor? prediction = model.forward(tensor(input_array).to(CUDA));
                TorchSharp.Utils.TensorAccessor<float> ta = prediction.data<float>();
                float[] result = ta.ToArray();
                float value = (float)dm.MapBToA(result[0]);//反向映射
                g_assigned["interpolation"].set_value(n, value);
            }
            g_assigned.showGrid_win();
        }

        public static Model_空间插值3 多维数据的拟合(float[,] input, float[,] output)
        {
            Tensor t_input = tensor(input).to(CUDA);
            Tensor t_output = tensor(output).to(CUDA);
            var model = new Model_空间插值3(input.GetLength(1));
            model.train();//设置为训练模式
            model.to(CUDA);
            var optimizer = optim.SGD(model.parameters(), 0.0001);
            int N = 3_000_000;
            //N = 10_000;
            Tensor? prediction = null;
            for (int i = 0; i < N; i++)
            {
                prediction = model.forward(t_input);
                var loss = functional.mse_loss(prediction, t_output);
                if (i % 5000 == 0)
                    Console.WriteLine($"{loss.data<float>().First()} i={i.ToString("#,##0")}");

                optimizer.zero_grad();
                loss.backward();
                optimizer.step();
            }
            prediction?.print("g5", 100, "\n", null, TorchSharp.TensorStringStyle.Julia);
            return model;
        }
    }
}
