using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public int cantidad { get; set; }
        public Categoria cat { get; set; }
        public int idCategoria { get; set; }
        public ICollection<Carro> carros { get; } = new List<Carro>();
        public List<CarroProducto> carroProducto { get; set; }
        public ICollection<Compra> compras { get; } = new List<Compra>();
        public List<CompraProducto> compraProducto { get; set; }

        public Producto() { }
        public Producto(string nombre, double precio, int cantidad, Categoria cat)
        {
            this.nombre         = nombre;
            this.precio         = precio;
            this.cantidad       = cantidad;
            this.cat            = cat;
            this.idCategoria    = cat.idCategoria;
            this.carroProducto  = new List<CarroProducto>();
            this.compraProducto = new List<CompraProducto>();
        }

        public override string ToString()
        {
            return idProducto + "-" + nombre + "-" + precio + "-" + cantidad + "-" + cat;
        }

    }
}


