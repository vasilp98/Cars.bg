using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class Administrator : User
    {
        public Administrator()
        {

        }

        public Administrator(string username, string password, string gender, string type) : base(username, password, gender, type)
        {
           
        }

        public Administrator(string username, string password, string messages,string gender, string type) : base(username, password, gender, type)
        {
            this.messages = messages;
        }

        public override User signIn()
        {
            throw new NotImplementedException();
        }

        public override void signUp(string username)
        {
            throw new NotImplementedException();
        }

        public override void show()
        {
            Console.WriteLine($"Username: {this.username}");
            Console.WriteLine($"Type: {this.type}");
        }

        public override void deleteUser()
        {
            Console.WriteLine("Enter the profile's username you want to delete: ");
            string deleteUsername = Console.ReadLine();

            Commands.deleteProfile(deleteUsername);
        }


    }
}
