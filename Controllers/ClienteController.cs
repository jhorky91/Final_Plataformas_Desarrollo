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
            var carro =  await _context.carros
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
            return View( productos);
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
        //                                  VACIAR CARRO
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
            }
            
            //GUARDAMOS LOS CAMBIOS                               
            _context.carros.Update(carro);
            _context.SaveChanges();

            return RedirectToAction("Carro");

        }

        public async Task<IActionResult> ModificarCarro(int ID, int cantidad)
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

            Carro carro = await _context.carros
                                    .Where(c => c.idUsuario == id_usr)
                                    .Include(c => c.carroProducto)
                                    .ThenInclude(cp => cp.producto)
                                    .FirstOrDefaultAsync();

            Producto prod = await _context.productos
                                    .Where(p => p.idProducto == ID)
                                    .Include(p => p.carroProducto)
                                    .FirstOrDefaultAsync();

            foreach (CarroProducto cp in carro.carroProducto)
            {
                if (cp.idProducto == ID)
                {
                    if (cantidad == 0)
                    {
                        carro.carroProducto.Remove(cp);
                        _context.carros.Update(carro);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        cp.cantidad = cantidad;
                        _context.carros.Update(carro);
                        await _context.SaveChangesAsync();
                    }
                    break;
                }
            }

            return RedirectToAction("Carro");
        }
        public IActionResult VaciarCarro()
        {
            
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            if (_context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefault() != null)
            {
                Carro c = _context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefault();

                c.carroProducto = new List<CarroProducto>();
                _context.carros.Update(c);
                _context.SaveChanges();
            }
            

            return RedirectToAction("Carro");
        }

        // #######################################################################################
        //                                  COMPRA
        // #######################################################################################

        public bool Comprar()
        {                       
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                double total = 0;

                Carro c = _context.usuarios.Where(U => U.idUsuario == id_usr).FirstOrDefault().miCarro;
                foreach (CarroProducto carProd in c.carroProducto)
                {
                    if (carProd.producto.cantidad >= carProd.cantidad)
                    {
                        total += (carProd.cantidad * carProd.producto.precio);
                    }
                    else 
                    {
                        return false;
                    }
                }

                Usuario user = _context.usuarios.Where(U => U.idUsuario == id_usr).FirstOrDefault();
                Compra aux = new Compra(user, total);
                _context.compras.Add(aux);
                _context.SaveChanges();

                foreach (Producto prod in c.productos)
                {
                    int cant = c.carroProducto.Where(CP => CP.idProducto == prod.idProducto).FirstOrDefault().cantidad;
                    CompraProducto cp = new CompraProducto(aux, prod, cant);
                    aux.compraProducto.Add(cp);
                }
                c.carroProducto = new List<CarroProducto>();
                _context.carros.Update(c);
                _context.compras.Update(aux);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
