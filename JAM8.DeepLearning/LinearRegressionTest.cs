using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace JAM7.DeepLearning
{
    public class LinearRegressionTest
    {
        [STAThread]
        public static void LinearRegression()
        {
            var x = tensor(new float[] { 1, 2, 3, 4, 5, 6, 7 }).reshape(7, 1);
            x.print();
            var y = tensor(new float[] { 10, 20, 30, 40, 50, 60, 70 }).reshape(7, 1);
            y.print();
            var model = new LinearRegressionModel();
            var optimizer = optim.Adam(model.parameters(), 0.05);

            for (int i = 0; i < 2000; i++)
            {
                var eval = model.forward(x);
                var loss_value = functional.mse_loss(eval, y);
                Console.WriteLine();
                eval.reshape(1, 7).print();
                y.reshape(1, 7).print();
                loss_value.print();
                optimizer.zero_grad();
                loss_value.backward();
                optimizer.step();
            }
        }

        public class LinearRegressionModel : Module<Tensor, Tensor>
        {
            public LinearRegressionModel() : base(nameof(LinearRegressionModel))
            {
                RegisterComponents();
            }

            public override Tensor forward(Tensor input)
            {
                return lin1.forward(input);
            }

            private Module<Tensor, Tensor> lin1 = Linear(1, 1);
        }
    }
}
