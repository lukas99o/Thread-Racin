using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Threading
{
    internal class Car
    {
        public double TopSpeed { get; set; }
        public string Name { get; set; }
        public double Position { get; set; }

        private bool _finished = false;

        private static System.Timers.Timer _timer = new System.Timers.Timer(30000);

        private static bool _timerIsThirty = false;

        private static void Race(Car car, CountdownEvent countdownEvent, List<string> completionOrder)
        {
            double raceDistance = 20.0; // 20km
            Console.WriteLine("im runnin");
            while (true)
            {
                Console.WriteLine("im runnin");
                car.Position += car.TopSpeed / 3600;
                Console.WriteLine(car.Position);

                if (car.Position >= raceDistance)
                {
                    Console.WriteLine("im runnin");
                    Console.WriteLine($"{car.Name} has finished the race at {raceDistance}km");
                    break;
                }

                Thread.Sleep(1000);
            }
            
            lock (completionOrder)
            {
                completionOrder.Add(car.Name);
            }
            
            countdownEvent.Signal();
            car._finished = true;
        }

        public static void StartRace(List<Car> cars)
        {
            Console.WriteLine("These cars will race!");
            PrintInfo(cars);
            Console.WriteLine();
            Console.WriteLine("At any point input [STATUS] to see how the cars are doing!");
            Console.Write("Press any key to have the race start! ");
            Console.ReadLine();
            Console.WriteLine("\n");

            CountdownEvent countdownEvent = new CountdownEvent(cars.Count);
            List<string> completionOrder = new List<string>();
            List<Thread> carThreads = new List<Thread>();
            List<Thread> raceStatuses = new List<Thread>();

            foreach (var car in cars)
            {
                Thread carThread = new Thread(() => Race(car, countdownEvent, completionOrder));
                carThreads.Add(carThread);
                carThread.Start();
            }

            foreach (var car in cars)
            {
                Thread raceStatus = new Thread(() => PrintRaceStatus(car));
                raceStatuses.Add(raceStatus);
                raceStatus.Start();
            }

            countdownEvent.Wait();

            Console.WriteLine("Race Finished!\n");
            foreach (var car in completionOrder)
            {
                int i = 1;
                Console.WriteLine($"{car} Finished {i}.");
                i++;
            }
        }

        private static void PrintRaceStatus(Car car)
        {
            while (car._finished)
            {
                if (Console.KeyAvailable)
                {
                    string userInput = Console.ReadLine();
                    if (userInput.ToLower() == "status")
                    {
                        Console.WriteLine($"{car.Name} is at {car.Position:F2} km with a speed of {car.TopSpeed}km/h\n");

                        Thread.Sleep(1000);
                    }
                }
            } 
        }

        private void CarProblems()
        {
            bool noFuel = RandomNumberGenerator(1, 2);
            bool flatTire = RandomNumberGenerator(2, 50);
            bool birdInSight = RandomNumberGenerator(5, 50);
            bool engineError = RandomNumberGenerator(10, 50);
               
            if (noFuel)
            {
                Console.WriteLine($"{Name} is stopping to refuel!");
                Thread.Sleep(30000);
                Console.WriteLine($"{Name} is stopping to refuel!");
            }
            else if (flatTire)
            {
                Console.WriteLine($"{Name} is stopping to refuel!");
                Thread.Sleep(20000);
                Console.WriteLine($"{Name} is stopping to refuel!");
            }
            else if (birdInSight)
            {
                Console.WriteLine($"{Name} is stopping to refuel!");
                Thread.Sleep(10000);
                Console.WriteLine($"{Name} is stopping to refuel!");
                
            }
            else if (engineError)
            {
                Console.WriteLine($"{Name} is stopping to refuel!");
                TopSpeed -= 1;
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
                _timerIsThirty = true;

                _timer.Stop();
                _timer.Start();
        }

        private static bool RandomNumberGenerator(int numerator, int denominator)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, denominator + 1);

            return randomNumber == numerator;
        }

        public static void PrintInfo(List<Car> cars)
        {
            foreach (Car car in cars)
            {
                Console.WriteLine(car.Name);
            }
        }
    }     
}
