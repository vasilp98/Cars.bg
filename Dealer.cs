using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.bg
{
    public class Dealer : User
    {

        public Dealer()
        {

        }

        public Dealer(string username, string cars, string type) : base(username, cars, type)
        {

        }

        public Dealer(string username, string password, string gender, string type) : base(username, password, gender, type)
        {
            
        }

        public Dealer(string username, string password, string cars, string messages, string gender, string type = "dealer") : base(username, password, cars, messages, gender, type)
        {
            
        }

        public override void signUp(string username)
        {

        }

        public override User signIn()
        {
            throw new NotImplementedException();
        }
    }
}
