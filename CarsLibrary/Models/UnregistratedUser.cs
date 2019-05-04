using CarsLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class UnregistratedUser : IUnregistratedUser
    {
        public UnregistratedUser()
        {

        }

        public void signUp(string username)
        {
            Commands.signUp(username);
        }

        public IUser signIn()
        {
            return Commands.signIn();
        }
    }
}
