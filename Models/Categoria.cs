using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Plataformas_De_Desarrollo.Models

{
    public class Categoria
    {
        public int idCategoria { get; set; }
        public string nombre { get; set; }

        public ICollection<Producto> productos { get; set; }
        
        public Categoria() { }

        public Categoria(string Nombre)
        {
            this.nombre = Nombre;
        }
        
        public override string ToString()
        {
            return idCategoria + "-" + nombre;
        }
    }
}
