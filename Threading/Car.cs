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
        public int TopSpeed { get; set; }
        public double Position { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        private bool Finished = false;

        private double Points = 0;

        private static System.Timers.Timer timer = new System.Timers.Timer(30000);

        private static bool timerIsThirty = false;

        public static void PrintInfo(List<Car> cars)
        {
            foreach (Car car in cars)
            {
                Console.WriteLine($"Brand: {car.Brand}\t Model: {car.Model}");
            }
        }    

        public void Race(List<Car> cars, CountdownEvent countdownEvent)
        {
            double raceDistance = 20.0; // 20km
            timer.Start();
            timer.Elapsed += OnTimerElapsed;

            while (true)
            {
                bool allCarsFinished = true;
                
                foreach (var car in cars)
                {
                    lock (car)
                    {
                        if (car.Finished)
                        {
                            continue;
                        }

                        car.Position += (car.TopSpeed / 3600.0);

                        if (timerIsThirty)
                        {
                            CarProblems(cars);
                            timerIsThirty = false;
                        }

                        if (car.Position >= raceDistance && !car.Finished)
                        {
                            if (!countdownEvent.IsSet)
                            {
                                if (!car.Finished)
                                {
                                    Console.WriteLine($"{car.Brand} {car.Model} finished at {car.Position:F2} km");
                                    car.Finished = true;
                                    countdownEvent.Signal();

                                    car.Points += CalculatePoints(cars.Count - countdownEvent.CurrentCount);
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
                        return;
                    }
                    Thread.Sleep(1000);
                }   
            }
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

        private int CalculatePoints(int position)
        {
            int points = 100 - (position - 1) * 10;
            return points > 0 ? points : 0;
        }

        private static void CarProblems(List<Car> cars)
        {
            foreach (var car in cars)
            {
                bool noFuel = RandomNumberGenerator(1, 2);
                bool flatTire = RandomNumberGenerator(2, 50);
                bool birdInSight = RandomNumberGenerator(5, 50);
                bool engineError = RandomNumberGenerator(10, 50);
               
                if (noFuel)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} is stopping to refuel!");
                    Thread.Sleep(30000);
                    break;
                }

                if (flatTire)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} is stopping to change tires!");
                    Thread.Sleep(20000);
                    Console.WriteLine($"{car.Brand} {car.Model} starts the race again!");
                    break;
                }

                if (birdInSight)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} is stopping to clean the window!");
                    Thread.Sleep(10000);
                    Console.WriteLine($"{car.Brand} {car.Model} starts the race again!");
                    break;
                }

                if (engineError)
                {
                    Console.WriteLine($"{car.Brand} {car.Model} has engine failure loses 1km/h");
                    car.TopSpeed -= 1;
                    break;
                }
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
                timerIsThirty = true;

                timer.Stop();
                timer.Start();
        }

        private static bool RandomNumberGenerator(int numerator, int denominator)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, denominator + 1);

            return randomNumber == numerator;
        }
    }     
}
