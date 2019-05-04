using Cars.bg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsLibrary.Interfaces
{
    public interface IUnregistratedUser
    {
        void signUp(string username);

        IUser signIn();
    }
}
