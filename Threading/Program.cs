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
               new Car() { TopSpeed = 150, Brand = "Volkswagen", Model = "Golf", Position = 0 },
               new Car() { TopSpeed = 300, Brand = "Tesla", Model = "Model 3", Position = 0 },
               new Car() { TopSpeed = 500, Brand = "Tesla", Model = "Roadster 2", Position = 0 }
            };

            Thread runTheRace = new Thread(new ThreadStart(() => Car.StartRace(raceCars)));
            runTheRace.Start();

            Thread raceStatus = new Thread(() =>
            {
                while (runTheRace.IsAlive)
                {
                    if (Console.KeyAvailable)
                    {
                        string userInput = Console.ReadLine();
                        if (userInput.ToLower() == "status")
                        {
                            Car.PrintRaceStatus(raceCars);
                        }
                    }

                    Thread.Sleep(1000);
                }
            });
            raceStatus.Start();
        }
    }
}