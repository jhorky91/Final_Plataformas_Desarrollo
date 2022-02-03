
namespace Final_Plataformas_De_Desarrollo.Models 
{ 
    public class CarroProducto
    {
        public int idCarroProducto { get; set; }
        public int idCarro { get; set; }
        public Carro carro { get; set; }
        public int idProducto { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }

        public CarroProducto() { }
        public CarroProducto(Carro carro, Producto producto, int cantidad) 
        {
            idCarro = carro.idCarro;
            this.carro = carro;
            idProducto = producto.idProducto;
            this.producto = producto;
            this.cantidad = cantidad;
        }
    }
}
