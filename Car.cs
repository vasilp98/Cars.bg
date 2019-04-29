using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class Car
    {
        public int carId { get; set; }

        public string brand { get; set; }

        public string model { get; set; }

        public string engine { get; set; }

        public int horsePowers { get; set; }

        public int year { get; set; }

        public string transmission { get; set; }

        public int views { get; set; }

        public Car()
        {

        }

        public Car(int carId, string brand, string model, string engine, int horsePower, int year, string transmission, int views = 0)
        {
            this.carId = carId;
            this.brand = brand;
            this.model = model;
            this.engine = engine;
            this.horsePowers = horsePower;
            this.year = year;
            this.transmission = transmission;
            this.views = views;
        }

        public void show()
        {
            Console.WriteLine($"Car's ID: {this.carId}");
            Console.WriteLine($"Brand: {this.brand}");
            Console.WriteLine($"Model: {this.model}");
            Console.WriteLine($"Engine: {this.engine}");
            Console.WriteLine($"Horse powers: {this.horsePowers}");
            Console.WriteLine($"Year: {this.year}");
            Console.WriteLine($"Transmission: {this.transmission}");
            Console.WriteLine($"Views: {this.views}");
        }
    }
}
