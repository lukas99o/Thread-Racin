using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class Car
    {
        public int TopSpeed { get; set; }
        public double Position { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        private static object lockObject = new object();

        private bool Finished = false;

        private double Points = 0;

        public static void PrintInfo(List<Car> cars)
        {
            foreach (Car car in cars)
            {
                Console.WriteLine($"Brand: {car.Brand}\t Model: {car.Model}");
            }
        }    

        public void Race(List<Car> cars, CountdownEvent countdownEvent)
        {
            double raceDistance = 10.0; // 10km
            object lockObject = new object();
            double points = 100;

            while (true)
            {
                bool allCarsFinished = true;

                Thread raceStatus = new Thread(() =>
                {
                    while (allCarsFinished)
                    {
                        if (Console.KeyAvailable)
                        {

                        }
                    }
                }

                foreach (var car in cars)
                {
                    lock (lockObject)
                    {
                        if (car.Finished)
                        {
                            continue;
                        }
                    }

                    car.Position += (car.TopSpeed / 3600.0);

                    if (car.Position >= raceDistance && !car.Finished)
                    {
                        if (!countdownEvent.IsSet)
                        {
                                if (!car.Finished)
                                {
                                    Console.WriteLine($"{car.Brand} {car.Model} finished at {car.Position:F2} km");
                                    car.Finished = true;
                                    countdownEvent.Signal();

                                    car.Points += points;
                                    points -= 10;
                                }
                        }
                    }

                    if (!car.Finished)
                    {
                        allCarsFinished = false;
                    }
                }

                if (allCarsFinished)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        public static void StartRace(List<Car> cars)
        {
            Console.WriteLine("These cars will race!");
            PrintInfo(cars);
            Console.WriteLine();
            Console.Write("Press any key to have the race start! ");
            Console.ReadKey();
            Console.WriteLine("\n");

            CountdownEvent countdownEvent = new CountdownEvent(cars.Count);

            List<Thread> carThreads = cars.Select(car => new Thread(() => car.Race(cars, countdownEvent))).ToList();
            carThreads.ForEach(carThread => carThread.Start());

            countdownEvent.Wait();

            Console.WriteLine("Race Finished!\n");
            List<Car> winners = cars.OrderByDescending(car => car.Points).ToList();
            for (int i = 0; i < winners.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {winners[i].Brand} {winners[i].Model}");
                Console.WriteLine($"Points {i + 1} place: {winners[i].Points}\n");
            }
        }

        public static void PrintRaceStatus(List<Car> cars)
        {

            Console.WriteLine("Race Status:");

            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Brand} {car.Model} is at {car.Position:F2} km with a speed of {car.TopSpeed} km/h");
            }

            Console.WriteLine();
        }
    }     
}
