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

        private bool _stopped = false;

        private static int _sharedVariable = 0; 

        private bool _timerIsThirty = false;

        private static void Race(Car car, CountdownEvent countdownEvent, List<string> completionOrder)
        {
            double raceDistance = 20.0; // 20km

            Thread carProblem = new Thread(() => TimerIsThirty(car));
            carProblem.Start();

            while (true)
            {
                car.Position += car.TopSpeed / 3600;

                if (car.Position >= raceDistance)
                {
                    _sharedVariable++;
                    Console.WriteLine($"{car.Name} has finished the race at {raceDistance}km and finished {_sharedVariable}.");
                    car._finished = true;
                    break;
                }

                if (car._timerIsThirty == true)
                {
                    CarProblems(car);
                }

                Thread.Sleep(1000);
            }
            
            lock (completionOrder)
            {
                completionOrder.Add(car.Name);
            }
            
            countdownEvent.Signal();
        }

        public static void StartRace(List<Car> cars)
        {
            Console.WriteLine("\nThese cars will race!");
            PrintInfo(cars);
            Console.WriteLine();
            Console.WriteLine("At any point input [STATUS] to see how the cars are doing!");
            Console.Write("Press any key to have the race start! ");
            Console.ReadLine();
            Console.WriteLine("The race begins!\n");

            CountdownEvent countdownEvent = new CountdownEvent(cars.Count);
            List<string> completionOrder = new List<string>();
            List<Thread> carThreads = new List<Thread>();

            foreach (var car in cars)
            {
                Thread carThread = new Thread(() => Race(car, countdownEvent, completionOrder));
                carThreads.Add(carThread);
                carThread.Start();
            }

            Thread raceStatus = new Thread(() => PrintRaceStatus(cars));
            raceStatus.Start();

            countdownEvent.Wait();

            raceStatus.Join();

            Console.WriteLine("\nRace Finished!\n");

            int i = 1;
            foreach (var car in completionOrder)
            {
                
                Console.WriteLine($"{car} Finished {i}.");
                i++;
            }

            Console.WriteLine();
            Console.Write("Press any key to exit the simulation... ");
            Console.ReadLine();
        }

        private static void PrintRaceStatus(List<Car> cars)
        {
            foreach (var car1 in cars)
            {
                while (car1._finished == false)
                {
                    if (Console.KeyAvailable)
                    {
                        string userInput = Console.ReadLine();
                        Console.WriteLine();

                        foreach (var car2 in cars)
                        {
                            if (userInput.ToLower() == "status")
                            {
                                if (car2._finished == false && car2._stopped == false)
                                {
                                    Console.WriteLine($"{car2.Name} is at {car2.Position:F2} km with a speed of {car2.TopSpeed}km/h");
                                }
                                else
                                {
                                    Console.WriteLine($"{car2.Name} is at {car2.Position:F2} km");
                                }
                            }
                        }

                        Console.WriteLine();
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private static void TimerIsThirty(Car car)
        {
            while (car._finished == false)
            {
                if (car._timerIsThirty == false)
                {
                    Thread.Sleep(30000);
                    car._timerIsThirty = true;
                }
            }
        } 

        private static void CarProblems(Car car)
        {
            bool noFuel = RandomNumberGenerator(1, 50);
            bool flatTire = RandomNumberGenerator(2, 50);
            bool birdInSight = RandomNumberGenerator(5, 50);
            bool engineError = RandomNumberGenerator(10, 50);
            
            if (noFuel)
            {
                Console.WriteLine($"{car.Name} is stopping to refuel!");
                car._stopped = true;
                Thread.Sleep(30000);
                Console.WriteLine($"{car.Name} Starts the race again!");
            }
            else if (flatTire)
            {
                Console.WriteLine($"{car.Name} Is stopping to change tire!");
                car._stopped = true;
                Thread.Sleep(20000);
                Console.WriteLine($"{car.Name} Starts the race again!");
            }
            else if (birdInSight)
            {
                Console.WriteLine($"{car.Name} Is stopping to clean the window!");
                car._stopped = true;
                Thread.Sleep(10000);
                Console.WriteLine($"{car.Name} Starts the race again!");

            }
            else if (engineError)
            {
                Console.WriteLine($"{car.Name} Has engine failure and loses 1km/h!");
                car.TopSpeed -= 1;
            }

            car._timerIsThirty = false;
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
