using System.Text;

namespace Threading
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;



            await Task.Delay(3000);
            tokenSource.Cancel();
            string result = await task1;
            await Console.Out.WriteLineAsync(result);



            Car car1 = new Car(100, "Volkswagen 1976", "Golf");
            Car car2 = new Car(250, "Tesla", "Model 3");
            Car car3 = new Car(420, "Tesla", "Roadster 2");

            Console.WriteLine("These cars will race eachother!");
            car1.PrintInfo();
            car2.PrintInfo();
            car3.PrintInfo();

            Car.Race(car1, car2, car3);
        }
    }
}