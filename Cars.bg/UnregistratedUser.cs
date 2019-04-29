using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class UnregistratedUser : User
    {
        public UnregistratedUser()
        {

        }

        public override void signUp(string username)
        {
            Commands.signUp(username);
        }

        public override User signIn()
        {
            return Commands.signIn();
        }

        public override void addCar()
        {
            throw new NotImplementedException();
        }

        public override void deleteCar()
        {
            throw new NotImplementedException();
        }

        public override void deleteMyProfile()
        {
            throw new NotImplementedException();
        }

        public override void seeAllNewMessages()
        {
            throw new NotImplementedException();
        }

        public override void show()
        {
            throw new NotImplementedException();
        }

        public override void writeMessage()
        {
            throw new NotImplementedException();
        }

        public override void searchCar()
        {
            
        }
    }
}
