using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class Dealer : IUser
    {

        public Dealer()
        {

        }

        public string cars { get; set; }
        public string gender { get; set; }
        public string messages { get; set; }
        public string password { get; set; }
        public string type { get; set; }
        public string username { get; set; }

        public virtual void writeMessage()
        {
            Commands.writeMessage(this.username);
        }

        public virtual void seeAllNewMessages()
        {
            Commands.seeAllNewMessages(this.username);
        }

        public virtual void deleteMyProfile()
        {
            Commands.deleteProfile(this.username);
        }

        public virtual void addCar()
        {
            Commands.addCar(this.username);

            this.cars = Commands.getData("Users", "username", "cars", this.username);
        }

        public virtual void deleteCar()
        {
            Console.WriteLine("Enter car's ID you want to delete: ");
            int carId = Int32.Parse(Console.ReadLine());

            string[] Ids = this.cars.Split(',');

            bool correctCarId = false;

            for (int i = 1; i < Ids.GetLength(0); i++)
            {
                if (Int32.Parse(Ids[i]) == carId)
                    correctCarId = true;
            }

            if (correctCarId == true)
            {
                Commands.deleteCar(carId, this.username);
            }
            else
            {
                Console.WriteLine("This car does not belong to you!");
            }
        }

        public virtual void show()
        {
            Console.WriteLine($"Username: {this.username}");
            Console.WriteLine($"Cars: {this.cars}");
            Console.WriteLine($"type: {this.type}");
        }

        public virtual void searchCar()
        {
            Console.WriteLine("Brand: ");
            string brand = Console.ReadLine();
            Console.WriteLine("Model: ");
            string model = Console.ReadLine();

            var details = Commands.getDetails("Cars", "*", brand, model);

            foreach (var detail in details)
            {
                detail.show();
                Console.WriteLine();
            }
        }
    }
}
