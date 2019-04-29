using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public abstract class User
    {
        public string username { get; }

        public string password { get; }

        public string cars { get; set; }

        public string messages { get; set; }

        public string gender { get; }

        public string type { get; }

        public User()
        {

        }

        public User(string username, string cars, string type)
        {
            this.username = username;
            this.cars = cars;
            this.type = type;
        }

        public User(string username, string password, string gender, string type) 
        {
            this.username = username;
            this.password = password;
            this.gender = gender;
            this.type = type;
        }

        public User(string username, string password, string cars, string messages, string gender, string type)
        {
            this.username = username;
            this.password = password;
            this.cars = cars;
            this.messages = messages;
            this.gender = gender;
            this.type = type;
        }

        public abstract void signUp(string username);

        public abstract User signIn();

        public virtual void deleteUser()
        {
            Console.WriteLine("You do not have permission for that!");
        }

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
                Commands.deleteCar(carId);
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

            foreach(var detail in details)
            {
                detail.show();
                Console.WriteLine();

            }
        }
    }
}
