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
        
        public async Task<IActionResult> Index()
        {
            //ViewData["Login"] = true;
            var productos = _context.productos.Include(p => p.cat);
            ViewData["categorias"] = _context.categorias;
            return View(await productos.ToListAsync());
        }

        public IActionResult Carro()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var carro =  _context.usuarios.Where(u => u.idUsuario == id_usr).FirstOrDefault().miCarro;
            return View(carro);
        }
        public async Task<IActionResult> MisCompras()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var compras = _context.compras.Where(u => u.idUsuario == id_usr);
            return View(await compras.ToListAsync());
        }

        //public async Task<IActionResult> ListadoProductos()
        //{
        //    var productos = _context.productos.Include(p => p.cat);
        //    ViewData["categorias"] = _context.categorias;
        //    return View(await productos.ToListAsync());
        //}


        public async Task<IActionResult> ListadoProductos(int cat, string orderby, string az)
        {
            IEnumerable<Producto> productos = await _context.productos.Include(p => p.cat).ToListAsync();

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

        public IActionResult DetalleProducto(int id)
        {
            var producto = _context.productos.Where(p => p.idProducto == id).FirstOrDefault();
            return View(producto);
        }

        // #######################################################################################
        //                                  AGREGAR AL CARRO
        //                                  MODIFICAR CARRO
        //                                  VACIAR CARRO
        // #######################################################################################

        public bool AgregarAlCarro(int ID, int cantidad)
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                Carro c = _context.usuarios.Where(u => u.idUsuario == id_usr).FirstOrDefault().miCarro;
                c.productos.Add(_context.productos.Where(p => p.idProducto == ID).FirstOrDefault());
                _context.carros.Update(c);
                _context.SaveChanges();

                foreach (CarroProducto cp in c.carroProducto)
                {
                    if (cp.idProducto == ID)
                    {
                        cp.cantidad = cantidad;
                        _context.carros.Update(c);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }

        public bool ModificarCarro(int ID, int cantidad)
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                if (_context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefault() != null)
                {
                    Carro c = _context.usuarios.Where(u => u.idUsuario == id_usr).FirstOrDefault().miCarro;
                    Producto p = _context.productos.Where(p => p.idProducto == ID).FirstOrDefault();

                    if (cantidad == 0)
                    {
                        
                        c.productos.Remove(p);
                        _context.carros.Update(c);
                        _context.SaveChanges();
                    }
                    else
                    {
                        int Cant = c.carroProducto.Where(p => p.idProducto == ID).FirstOrDefault().cantidad;
                        c.carroProducto.Where(p => p.idProducto == ID).FirstOrDefault().cantidad = cantidad;
                        _context.carros.Update(c);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool VaciarCarro()
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                if (_context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefault() != null)
                {
                    Carro c = _context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefault();

                    c.carroProducto = new List<CarroProducto>();
                    _context.carros.Update(c);
                    _context.SaveChanges();
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
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
