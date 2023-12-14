using System.Diagnostics;
using System.Text;

namespace Threading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Car> raceCars = new List<Car>()
            {
               new Car() { TopSpeed = 500, Brand = "Volkswagen", Model = "Golf", Position = 0 },
               new Car() { TopSpeed = 750, Brand = "Tesla", Model = "Model 3", Position = 0 },
               new Car() { TopSpeed = 1000, Brand = "Tesla", Model = "Roadster 2", Position = 0 }
            };

            Thread thread1 = new Thread(new ThreadStart(() => Car.StartRace(raceCars)));
            thread1.Start();

            Thread thread2 = new Thread(() =>
            {
                while 
            }


        }


    }
}