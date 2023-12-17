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
               new Car() { TopSpeed = 300, Name = "Volkswagen Golf" },
               new Car() { TopSpeed = 400, Name = "Tesla Model S" },
               new Car() { TopSpeed = 500, Name = "Tesla Roadster 2" }
            };

            Car.StartRace(raceCars);
        }
    }
}