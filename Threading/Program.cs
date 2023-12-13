using System.Diagnostics;
using System.Text;

namespace Threading
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            List<Car> raceCars = new List<Car>()
            {
               new Car() { TopSpeed = 500, Brand = "Volkswagen", Model = "Golf", Position = 0 },
               new Car() { TopSpeed = 750, Brand = "Tesla", Model = "Model 3", Position = 0 },
               new Car() { TopSpeed = 1000, Brand = "Tesla", Model = "Roadster 2", Position = 0 }
            };

            Car.StartRace(raceCars);

        }

        
    }
}