using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace JAM8.Tests
{
    public class Test_TorchSharp
    {

        public static void Test_TorchSharp1()
        {
            var lin1 = Linear(1000, 100);
            var lin2 = Linear(100, 10);
            var seq = Sequential(("lin1", lin1), ("relu1", ReLU()), ("drop1", Dropout(0.1)), ("lin2", lin2));

            using var x = torch.randn(64, 1000);
            using var y = torch.randn(64, 10);

            var optimizer = torch.optim.Adam(seq.parameters());

            for (int i = 0; i < 10; i++)
            {
                using var eval = seq.forward(x);
                using var output = functional.mse_loss(eval, y, Reduction.Sum);

                optimizer.zero_grad();

                output.backward();

                optimizer.step();
            }
        }

        public static void Test_TorchSharp2_LinearRegression()
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
                Console.WriteLine(model.parameters().ToList().Count);
                model.parameters().ToList()[0].print();
                model.parameters().ToList()[1].print();
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

            private readonly Module<Tensor, Tensor> lin1 = Linear(1, 1);
        }
    }
}
