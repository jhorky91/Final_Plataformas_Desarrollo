
namespace Final_Plataformas_De_Desarrollo.Models
{
    public class CompraProducto
    {
        public int idCompraProducto { get; set; }
        public int idCompra { get; set; }
        public Compra compra { get; set; } 
        public int idProducto { get; set; }
        public Producto producto { get; set; } 
        public int cantidad { get; set; }

        public CompraProducto() { }
        public CompraProducto(Compra compra, Producto producto, int cantidad) 
        {
            this.compra = compra;
            idCompra = compra.idCompra;
            this.producto = producto;
            idProducto = producto.idProducto;
            this.cantidad = cantidad;
        }
    }
}
