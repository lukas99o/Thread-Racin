using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    internal class Car
    {
        public int TopSpeed { get; set; }
        public double Position {  get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        public Car(int topSpeed, string brand, string model)
        {
            TopSpeed = topSpeed;
            Brand = brand;
            Model = model;
            Position = 0;
            int startSpeed = 0;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Brand: {Brand}\t Model: {Model}");
        }

        public void Race(Car car1, Car car2, Car car3)
        {
            double raceDistance = 10.0; // 10 km
            while (Position < raceDistance)
            {
                Console.WriteLine($"{Brand} {Model} is at {Position:F2} km");
            }

            Thread.Sleep((int)1000.0 / TopSpeed * 3600);
            Console.WriteLine("The race starts!");

            
            int carStartingPosition = 0;
            int carEndingPosition = 10;

            Position += 1;

            if (carStartingPosition == carEndingPosition)
            {
                Console.WriteLine($"Model: {Model}\t Brand: {Brand}\n Finished the race! ");
            }
            
        }
    }
}
