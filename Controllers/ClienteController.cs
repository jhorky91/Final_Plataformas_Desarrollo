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
        //
        //                          <<------ VISTAS ------>>
        //
        //                                  INDEX / HOME
        //                                  CARRO
        //                                  MIS COMPRAS
        //                                  MIS DATOS
        //                                  LISTADO DE PRODUCTOS
        //                                  DETALLE DE PRODUCTO
        //
        // #######################################################################################

        // #######################################################################################
        //
        //                                   INDEX / HOME
        //
        // #######################################################################################
        public async Task<IActionResult> Index()
        {
            var productos = _context.productos
                                .Include(p => p.cat);

            ViewData["categorias"] = _context.categorias;
            return View(await productos.ToListAsync());
        }

        // #######################################################################################
        //
        //                                      CARRO
        //
        // #######################################################################################
        public async Task<IActionResult> Carro()
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                var carro = await _context.carros
                                    .Where(c => c.idUsuario == id_usr)
                                    .Include(c => c.carroProducto)
                                    .ThenInclude(cp => cp.producto)
                                    .FirstOrDefaultAsync();
                ViewData["Carro"] = carro.carroProducto;
                return View();
            }
            catch (Exception) 
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"]  = "Vuelva a iniciar sesion";
                return RedirectToAction("Login","Home");
            }
        }

        // #######################################################################################
        //
        //                            HISTORIAL DE COMPRAS / MIS COMPRAS
        //
        // #######################################################################################
        public async Task<IActionResult> MisCompras()
        {
            try 
            { 
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                var compras = await _context.compras
                                    .Where(u => u.idUsuario == id_usr)
                                    .Include(c => c.compraProducto)
                                    .ThenInclude(cp => cp.producto)
                                    .ThenInclude(p => p.cat)
                                    .ToListAsync();
                return View(compras);
            }
            catch (Exception) 
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"]  = "Vuelva a iniciar sesion";
                return RedirectToAction("Login","Home");
            }

        }

        // #######################################################################################
        //
        //                            MIS DATOS
        //
        // #######################################################################################
        public async Task<IActionResult> MisDatos()
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                var usuario = await _context.usuarios
                                        .Where(u => u.idUsuario == id_usr)
                                        .FirstOrDefaultAsync();

                ViewData["datosUsuario"] = usuario;
                return View();
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }

        }

        // #######################################################################################
        //
        //                                   LISTADO DE PRODUCTOS
        //
        // #######################################################################################
        public async Task<IActionResult> ListadoProductos(string query, int cat, string orderby, string az)
        {
            IEnumerable<Producto> productos = await _context.productos
                                                        .Include(p => p.cat)
                                                        .ToListAsync();
            if (query != ""&& query != null) 
            {
                if (productos.Where(p => p.nombre.ToUpper().Contains(query.ToUpper())) != null)
                {
                    productos = productos.Where(p => p.nombre.ToUpper().Contains(query.ToUpper()));
                }
                else 
                { 
                
                }
            }
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

        // #######################################################################################
        //
        //                                   DETALLE DEL PRODUCTO
        //
        // #######################################################################################
        public async Task<IActionResult> DetalleProducto(int id)
        {

            Producto producto = await _context.productos
                                        .Where(p => p.idProducto == id)
                                        .Include(p => p.cat)
                                        .FirstOrDefaultAsync();
            List<Producto> ProdRelacionado = await _context.productos
                                                    .Where(p => p.idCategoria == producto.idCategoria)
                                                    .ToListAsync();

            ViewData["prodRela"] = ProdRelacionado;
            ViewData["producto"] = producto;
            
            return View();
        }

        // #######################################################################################
        //
        //                             <--- METODOS DE CLIENTE --->
        //
        //                                  AGREGAR AL CARRO
        //                                  MODIFICAR CARRO
        //                                  QUITAR PRODUCTO DEL CARRO 
        //                                  VACIAR CARRO
        //                                  COMPRA
        //
        // #######################################################################################

        // #######################################################################################
        //
        //                                   AGREGAR AL CARRO
        //
        // #######################################################################################
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarro(FormulariosViewModel model)
        {
            try 
            { 
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

                Carro carro = await _context.carros
                                        .Where(c => c.idUsuario == id_usr)
                                        .Include(c => c.carroProducto)
                                        .ThenInclude(cp => cp.producto)
                                        .FirstOrDefaultAsync();

                Producto prod = await _context.productos
                                        .Where(p => p.idProducto == model.AgregarInput.ID)
                                        .FirstOrDefaultAsync();

                //SI EL PRODUCTO YA EXISTE EN EL CARRO, SE AGREGA SOLO LA CANTIDAD
                if (carro.carroProducto.Exists(cp => cp.producto == prod))
                {
                    foreach (CarroProducto carroProd in carro.carroProducto)
                    {
                        if (carroProd.idProducto == model.AgregarInput.ID)
                        {
                            carroProd.cantidad += model.AgregarInput.Cantidad;
                            break;
                        }
                    }
                }
                //SI EL PRODUCTO NO EXISTE EN EL CARRO, SE AGREGA
                else
                {
                    CarroProducto cp = new CarroProducto();
                    cp.producto = prod;
                    cp.cantidad = model.AgregarInput.Cantidad;

                    carro.carroProducto.Add(cp);
                    HttpContext.Session.SetString("CantProductos", (int.Parse(HttpContext.Session.GetString("CantProductos")) + 1).ToString());
                }

                //GUARDAMOS LOS CAMBIOS                               
                _context.carros.Update(carro);
                _context.SaveChanges();

                TempData["Mensaje"] = "Producto agregado exitosamente";
                return RedirectToAction("Carro");
                
            }
            catch (Exception) 
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"]  = "Vuelva a iniciar sesion";
                return RedirectToAction("Login","Home");
            }
        }
        
        // #######################################################################################
        //
        //                             MODIFICAR PRODUCTO DEL CARRO
        //
        // #######################################################################################
        [HttpPost]
        public async Task<IActionResult> ModificarCarro(FormulariosViewModel model)
        {
            try 
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;

                Carro carro = await _context.carros
                                        .Where(c => c.idUsuario == id_usr)
                                        .Include(c => c.carroProducto)
                                        .FirstOrDefaultAsync();

                if (model.ModificarInput.Cantidad == 0)
                {

                    List<CarroProducto> aux = new List<CarroProducto>();
                    foreach (CarroProducto cp in carro.carroProducto)
                    {
                        if (cp.idProducto != model.ModificarInput.ID)
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
                        if (cp.idProducto == model.ModificarInput.ID)
                        {
                            cp.cantidad = model.ModificarInput.Cantidad;
                            _context.carros.Update(carro);
                            await _context.SaveChangesAsync();

                            break;
                        }
                    }
                }
                TempData["Mensaje"] = "Producto modificado";
                return RedirectToAction("Carro");
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }            
        }

        //################################################################################# 
        //
        //                         QUITAR PRODUCTO DEL CARRO 
        //
        //#################################################################################

        public async Task<IActionResult> QuitarProductoDelCarro(int ID)
        {
            
            try 
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

                TempData["Mensaje"] = "Producto quitado del carro";
                return RedirectToAction("Carro");
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }


        }

        // #######################################################################################
        //
        //                                  VACIAR EL CARRO
        //
        // #######################################################################################

        public async Task<IActionResult> VaciarCarro()
        {
            try 
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                if (await _context.carros.Where(c => c.idUsuario == id_usr).FirstOrDefaultAsync() != null)
                {
                    Carro c = await _context.carros
                                        .Where(c => c.idUsuario == id_usr)
                                        .Include(c => c.carroProducto)
                                        .FirstOrDefaultAsync();

                    c.carroProducto = new List<CarroProducto>();
                    _context.carros.Update(c);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("CantProductos", 0.ToString());
                }

                TempData["Mensaje"] = "Carro vaciado";
                return RedirectToAction("Carro");

            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }
        }

        // #######################################################################################
        //
        //                                  COMPRA
        //
        // #######################################################################################

        public async Task<IActionResult> Comprar()
        {
            try 
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

                        TempData["Mensaje"] = "Error, no hay stock suficiente para el producto "
                                            + carProd.producto.nombre +
                                            ", modifique el carro en el producto antes de efectuar la compra.";
                        
                        return RedirectToAction("Carro");
                    }
                }

                Usuario user = await _context.usuarios
                                            .Where(u => u.idUsuario == id_usr).FirstOrDefaultAsync();

                Compra aux = new Compra(user, total, DateTime.Now);
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

                HttpContext.Session.SetString("CantProductos", 0.ToString());

                TempData["Mensaje"] = "Compra efectuada exitosamente";
                return RedirectToAction("Carro");
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }
        }

        // #######################################################################################
        //
        //                          <--- METODOS PARA MODIFICAR DATOS DE LA CUENTA --->
        //                                  
        //                                  MODIFICAR DATOS PERSONALES
        //                                  MODIFICAR CONTRASEÑA
        //
        // #######################################################################################

        // #######################################################################################
        //
        //                                  MODIFICAR DATOS PERSONALES
        //
        // #######################################################################################

        public async Task<IActionResult> ModificarDatos(FormulariosViewModel model)
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                var usuario = await _context.usuarios
                                        .Where(u => u.idUsuario == id_usr)
                                        .FirstOrDefaultAsync();
                if (usuario.nombre != model.ModificarDatosInput.Nombre)
                {
                    var sesion = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn"));
                    sesion.name = model.ModificarDatosInput.Nombre;
                    HttpContext.Session.SetString("SignIn", JsonConvert.SerializeObject(sesion));
                }

                usuario.nombre = model.ModificarDatosInput.Nombre;
                usuario.apellido = model.ModificarDatosInput.Apellido;
                usuario.mail = model.ModificarDatosInput.Email;
                usuario.cuit_cuil = model.ModificarDatosInput.CUIT_CUIL;

                _context.usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Datos modificada";
                return RedirectToAction("MisDatos");
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }
        }

        // #######################################################################################
        //
        //                                  MODIFICAR CONTRASEÑA
        //
        // #######################################################################################

        public async Task<IActionResult> ModificarPassword(FormulariosViewModel model)
        {
            try
            {
                int id_usr = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("SignIn")).id;
                var usuario = await _context.usuarios
                                        .Where(u => u.idUsuario == id_usr)
                                        .FirstOrDefaultAsync();

                if (usuario.password == model.ModificarPasswordInput.PasswordAnterior)
                {
                    if (model.ModificarPasswordInput.PasswordNueva == model.ModificarPasswordInput.PasswordNueva2)
                    {
                        usuario.password = model.ModificarPasswordInput.PasswordNueva;
                        _context.usuarios.Update(usuario);
                        await _context.SaveChangesAsync();

                        TempData["Mensaje"] = "Contraseña modificada";
                        return RedirectToAction("MisDatos");
                    }
                    else
                    {
                        TempData["Mensaje"] = "Error: Contraseñas nuevas no coinciden, deben ser iguales";
                        TempData["Modal"] = "Contraseña";
                        return RedirectToAction("MisDatos");
                    }
                }
                else
                {
                    TempData["Mensaje"] = "Error: Contraseña antigua equivocada ";
                    TempData["Modal"] = "Contraseña";
                    return RedirectToAction("MisDatos");
                }
            }
            catch (Exception)
            {
                TempData["TituloMensaje"] = "Sesion Caducada";
                TempData["Mensaje"] = "Vuelva a iniciar sesion";
                return RedirectToAction("Login", "Home");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
