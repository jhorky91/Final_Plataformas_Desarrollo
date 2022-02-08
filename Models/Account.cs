using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class Account
    {
        public string name;
        public int id;
        public bool signIn;
        public bool esAdmin;
        public bool esEmpresa;


        public Account() 
        {
            this.signIn = false;
        }

    }
}
