using Final_Plataformas_De_Desarrollo.Models;
using Final_Plataformas_De_Desarrollo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace Final_Plataformas_De_Desarrollo.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly MyContext _context;

        public ClienteController(ILogger<ClienteController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        // #######################################################################################
        //                                  INDEX
        //                                  CARRO
        //                                  AGREGAR PRODUCTO
        //                                  MODIFICAR PRODUCTO
        //                                  MIS COMPRAS
        //                                  LISTADO DE PRODUCTOS
        //                                  DETALLE DE PRODUCTO
        // #######################################################################################

        public async Task<IActionResult> Index()
        {
            var productos = _context.productos
                                .Include(p => p.cat);

            ViewData["categorias"] = _context.categorias;
            return View(await productos.ToListAsync());
        }

        public async Task<IActionResult> Carro()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var carro = await _context.carros
                                .Where(c => c.idUsuario == id_usr)
                                .Include(c => c.carroProducto)
                                .ThenInclude(cp => cp.producto)
                                .FirstOrDefaultAsync();

            return View(carro.carroProducto);
        }

        //METODO FORMULARIO AGREGAR AL CARRO
        public async Task<IActionResult> AgregarProducto(int ID)
        {
            var productos = await _context.productos
                                        .Include(p => p.cat)
                                        .Where(p => p.idProducto == ID)
                                        .FirstOrDefaultAsync();

            ViewData["Producto"] = productos;
            return View();
        }
        public async Task<IActionResult> ModificarProducto(int ID)
        {
            var productos = await _context.productos
                                        .Include(p => p.cat)
                                        .Where(p => p.idProducto == ID)
                                        .FirstOrDefaultAsync();

            ViewData["Producto"] = productos;
            return View();
        }

        public async Task<IActionResult> MisCompras()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var compras = _context.compras
                                .Where(u => u.idUsuario == id_usr)
                                .Include(c => c.compraProducto);

            return View(await compras.ToListAsync());
        }

        public async Task<IActionResult> ListadoProductos(int cat, string orderby, string az)
        {
            IEnumerable<Producto> productos = await _context.productos
                                                        .Include(p => p.cat)
                                                        .ToListAsync();

            if (cat != 0)
            {
                productos = productos.Where(p => p.idCategoria == cat);
            }
            if (orderby != "")
            {
                if (orderby == "precio")
                {
                    if (az != "")
                    {
                        if (az == "asc")
                        {
                            productos = productos.OrderBy(p => p.precio);
                        }
                        else
                        {
                            productos = productos.OrderByDescending(p => p.precio);
                        }
                    }
                }
                else
                {
                    if (az != "")
                    {
                        if (az == "asc")
                        {
                            productos = productos.OrderBy(p => p.nombre);
                        }
                        else
                        {
                            productos = productos.OrderByDescending(p => p.nombre);
                        }
                    }
                }
            }

            ViewData["categorias"] = _context.categorias;
            return View(productos);
        }

        public async Task<IActionResult> DetalleProducto(int id)
        {

            Producto producto = await _context.productos
                                        .Where(p => p.idProducto == id)
                                        .Include(p => p.cat)
                                        .FirstOrDefaultAsync();

            return View(producto);
        }

        // #######################################################################################
        //                                  AGREGAR AL CARRO
        //                                  MODIFICAR CARRO
        //                                  QUITAR PRODUCTO DEL CARRO ------------------------------>  REVISAR
        //                                  VACIAR CARRO
        // #######################################################################################

        // #######################################################################################
        //
        //                                   AGREGAR AL CARRO
        //
        // #######################################################################################
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarro(AgregarAlCarroViewModel model)
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

            Carro carro = await _context.carros
                                    .Where(c => c.idUsuario == id_usr)
                                    .Include(c => c.carroProducto)
                                    .ThenInclude(cp => cp.producto)
                                    .FirstOrDefaultAsync();

            Producto prod = await _context.productos
                                    .Where(p => p.idProducto == model.Input.ID)
                                    .FirstOrDefaultAsync();

            //SI EL PRODUCTO YA EXISTE EN EL CARRO, SE AGREGA SOLO LA CANTIDAD
            if (carro.carroProducto.Exists(cp => cp.producto == prod))
            {
                foreach (CarroProducto carroProd in carro.carroProducto)
                {
                    if (carroProd.idProducto == model.Input.ID)
                    {
                        carroProd.cantidad += model.Input.Cantidad;
                        break;
                    }
                }
            }
            //SI EL PRODUCTO NO EXISTE EN EL CARRO, SE AGREGA
            else
            {
                CarroProducto cp = new CarroProducto();
                cp.producto = prod;
                cp.cantidad = model.Input.Cantidad;

                carro.carroProducto.Add(cp);
                HttpContext.Session.SetString("CantProductos", (int.Parse(HttpContext.Session.GetString("CantProductos")) + 1).ToString());
            }

            //GUARDAMOS LOS CAMBIOS                               
            _context.carros.Update(carro);
            _context.SaveChanges();

            return RedirectToAction("Carro");

        }

        // #######################################################################################
        //
        //                             MODIFICAR PRODUCTO DEL CARRO
        //
        // #######################################################################################

        public async Task<IActionResult> ModificarCarro(ModificarCarroViewModel model)
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

            Carro carro = await _context.carros
                                    .Where(c => c.idUsuario == id_usr)
                                    .Include(c => c.carroProducto)
                                    .FirstOrDefaultAsync();
            if (model.Input.Cantidad == 0)
            {

                List<CarroProducto> aux = new List<CarroProducto>();
                foreach (CarroProducto cp in carro.carroProducto)
                {
                    if (cp.idProducto != model.Input.ID)
                    {
                        aux.Add(cp);
                    }
                }

                carro.carroProducto = aux;
                _context.carros.Update(carro);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("CantProductos", carro.carroProducto.Count().ToString());
            }
            else
            {
                foreach (CarroProducto cp in carro.carroProducto)
                {
                    if (cp.idProducto == model.Input.ID)
                    {
                        cp.cantidad = model.Input.Cantidad;
                        _context.carros.Update(carro);
                        await _context.SaveChangesAsync();

                        break;
                    }
                }
            }

            return RedirectToAction("Carro");
        }

        //################################################################################# ---> REVISAR
        //
        //                         QUITAR PRODUCTO DEL CARRO 
        //
        //#################################################################################

        public async Task<IActionResult> QuitarProductoDelCarro(int ID)
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

            Carro carro = await _context.carros
                                    .Where(c => c.idUsuario == id_usr)
                                    .Include(c => c.carroProducto)
                                    .FirstOrDefaultAsync();

            List<CarroProducto> aux = new List<CarroProducto>();
            foreach (CarroProducto cp in carro.carroProducto)
            {
                if (cp.idProducto != ID)
                {
                    aux.Add(cp);
                }
            }

            carro.carroProducto = aux;
            _context.carros.Update(carro);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("CantProductos", carro.carroProducto.Count().ToString());
            return RedirectToAction("Carro");
        }

        // #######################################################################################
        //
        //                                  VACIAR EL CARRO
        //
        // #######################################################################################

        public async Task<IActionResult> VaciarCarro()
        {

            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            if (await _context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefaultAsync() != null)
            {
                Carro c = await _context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefaultAsync();

                c.carroProducto = new List<CarroProducto>();
                _context.carros.Update(c);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Carro");
        }

        // #######################################################################################
        //
        //                                  COMPRA
        //
        // #######################################################################################

        public async Task<IActionResult> Comprar()
        {

            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            double total = 0;

            Carro c = await _context.carros
                                .Where(c => c.idUsuario == id_usr)
                                .Include(c => c.carroProducto)
                                .ThenInclude(cp => cp.producto)
                                .FirstOrDefaultAsync();

            foreach (CarroProducto carProd in c.carroProducto)
            {
                if (carProd.producto.cantidad >= carProd.cantidad)
                {
                    total += (carProd.cantidad * carProd.producto.precio);
                }
                else
                {
                    //##############################################################################-------------------------> REVISAR
                    ViewData["ERROR"] = "Error, no hay stock suficiente para el producto "
                                        + carProd.producto.nombre +
                                        ", modifique el carro en el producto antes de efectuar la compra ";

                    return RedirectToAction("Carro");
                }
            }

            Usuario user = await _context.usuarios
                                        .Where(u => u.idUsuario == id_usr).FirstOrDefaultAsync();

            Compra aux = new Compra(user, total);
            _context.compras.Add(aux);
            await _context.SaveChangesAsync();

            foreach (CarroProducto cp in c.carroProducto)
            {
                int cant = cp.cantidad;
                CompraProducto comp = new CompraProducto(aux, cp.producto, cant);
                aux.compraProducto.Add(comp);
            }
            c.carroProducto = new List<CarroProducto>();
            _context.carros.Update(c);
            _context.compras.Update(aux);
            await _context.SaveChangesAsync();

            //##############################################################################-------------------------> REVISAR
            ViewData["Compra"] = "Compra efectuada exitosamente.";
            return RedirectToAction("Carro");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
