using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class Account
    {
        public string name;
        public bool signIn;
        public int rol;

        public Account() 
        {
            this.signIn = false;
        }

    }
}
