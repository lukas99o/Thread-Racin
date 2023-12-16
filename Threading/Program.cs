using System.Diagnostics;
using System.Text;

namespace Threading
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<Car> raceCars = new List<Car>()
            {
               new Car() { TopSpeed = 150, Name = "Volkswagen Golf" },
               new Car() { TopSpeed = 300, Name = "Tesla Model 3" },
               new Car() { TopSpeed = 500, Name = "Tesla Roadster 2" }
            };

            Car.StartRace(raceCars);
        }
    }
}