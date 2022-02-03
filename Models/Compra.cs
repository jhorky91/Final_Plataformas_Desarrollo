using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class Compra
    {
        public int idCompra {get; set; }
        public Usuario usuario {get; set;}
        public int idUsuario  { get; set; }
        public List<CompraProducto> compraProducto { get; set; } = new List<CompraProducto>();
        public ICollection<Producto> productos { get; } = new List<Producto>();
        public double total { get; set; }

        public Compra() { }
        public Compra(Usuario usuario, double total) 
        {
            this.usuario = usuario;
            idUsuario = usuario.idUsuario;
            this.total = total;
        }


    }
}
