using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    class Program
    {
        public static object MessageBox { get; private set; }

        static void Main(string[] args)
        {
            //Title
            Console.SetCursorPosition(30,0);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Welcome to Cars.bg");
            Console.ResetColor();

            User unregistratedUser = new UnregistratedUser();
            
            string input = null;
            while (input != "exit")
            {
                //Commands list
                Console.WriteLine();
                Console.WriteLine("Type any of the commands:");
                Console.WriteLine("-signUp");
                Console.WriteLine("-signIn");
                Console.WriteLine("-exit");

                input = Console.ReadLine();

                if (input == "signUp")
                {
                    Console.WriteLine("Enter username: ");
                    string username = Console.ReadLine();
                    unregistratedUser.signUp(username);
                }

                if(input == "signIn")
                {
                    User user = unregistratedUser.signIn();

                    string signInCommand = null;

                    while(signInCommand != "logout")
                    {
                        signInCommand = Console.ReadLine();

                        if (signInCommand == "writeMessage")
                        {
                            user.writeMessage();
                        }
                        if (signInCommand == "seeAllNewMessages")
                        {
                            user.seeAllNewMessages();
                        }
                        if(signInCommand == "deleteMyProfile")
                        {
                            user.deleteMyProfile();
                            break;
                        }
                        if(signInCommand == "addCar")
                        {
                            user.addCar();
                        }
                        if(signInCommand == "deleteCar")
                        {
                            user.deleteCar();
                        }
                        if(signInCommand == "showCar")
                        {
                            Console.WriteLine("Car's ID: ");
                            string carId = Console.ReadLine();
                            Car car = Commands.showCar(carId);
                            car.show();
                        }
                        if(signInCommand == "showUser")
                        {
                            Console.WriteLine("Enter username you want to see information about: ");
                            string username = Console.ReadLine();
                            User shownUser = Commands.showUser(username);
                            shownUser.show();
                        }
                        if(signInCommand == "deleteUser")
                        {
                            user.deleteUser();       
                        }
                        if(signInCommand == "searchCar")
                        {
                            user.searchCar();
                        }
                    }
                }
            }
        }
    }
}
