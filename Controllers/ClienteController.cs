﻿using Final_Plataformas_De_Desarrollo.Models;
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

        public async Task<IActionResult> Carro()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var carro =  await _context.carros.Where(c => c.idUsuario == id_usr).Include(c => c.carroProducto).FirstOrDefaultAsync();
            
            return View(carro.carroProducto);
        }

        public IActionResult AgregarProducto(int ID)
        {
            ViewData["Producto"] = ID;
            return View();
        }
        public async Task<IActionResult> MisCompras()
        {
            int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
            var compras = _context.compras.Where(u => u.idUsuario == id_usr).Include(c => c.compraProducto);
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

        public async Task<IActionResult> DetalleProducto(int id)
        {

            Producto producto = await _context.productos.Where(p => p.idProducto == id).Include(p => p.cat).FirstOrDefaultAsync();
            
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
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                Carro carro = await _context.carros.Where(c => c.idUsuario == id_usr).Include(c => c.carroProducto).FirstOrDefaultAsync();
                Producto prod = await _context.productos.Where(p => p.idProducto == model.Input.ID).FirstOrDefaultAsync();
                CarroProducto cp = new CarroProducto();
                cp.producto = prod;
                cp.cantidad = model.Input.Cantidad;

                carro.carroProducto.Add(cp);
                _context.carros.Update(carro);
                await _context.SaveChangesAsync();

                

                return RedirectToAction("Carro");
            }
            catch (Exception)
            {
                
            }
            
            
            
            return RedirectToAction("ListadoProductos");
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
