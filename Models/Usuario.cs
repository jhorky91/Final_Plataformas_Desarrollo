using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public int dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public int cuit_cuil { get; set; }
        public Carro miCarro { get; set; }
        

        public bool esAdmin { get;  set; }
        public bool esEmpresa { get; set; }
        public int intentos { get; set; }
        public bool bloqueado { get; set; }


        public ICollection<Compra> compras { get; set; }

        public Usuario() { }

        public Usuario(int dni, string nombre, string apellido, string mail, string password, int cuit_cuil)
        {
            //creo que hay que agregar en el constructor al carro
            this.dni        = dni;
            this.nombre     = nombre;
            this.apellido   = apellido;
            this.mail       = mail;
            this.password   = password;
            this.cuit_cuil  = cuit_cuil;
            
           
            
        }

    }
}
