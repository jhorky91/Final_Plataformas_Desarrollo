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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Final_Plataformas_De_Desarrollo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Login()
        {
            //if (ViewBag.error != null) 
            //{
            //    ViewBag.msg = ViewBag.error;
            //}
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            var usuario = await _context.usuarios
                                    .FirstOrDefaultAsync(u => u.dni == model.Input.DNI);


            if (usuario != null && usuario.password == model.Input.Password)
            {
                //ADMIN
                if (usuario.esAdmin)
                {
                    Account a = new Account();
                    a.name = usuario.nombre;
                    a.id = usuario.idUsuario;
                    a.esAdmin = usuario.esAdmin;
                    a.signIn = true;
                    HttpContext.Session.SetString("SignIn", JsonConvert.SerializeObject(a));

                    return RedirectToAction("Index", "Admin");
                }
                //CLIENTE
                else
                {
                    Account a = new Account();
                    a.name = usuario.nombre;
                    a.id = usuario.idUsuario;
                    a.esAdmin = usuario.esAdmin;
                    a.signIn = true;

                    var carro = _context.carros
                                        .Where(c => c.idUsuario == usuario.idUsuario)
                                        .Include(c => c.carroProducto)
                                        .FirstOrDefault();

                    HttpContext.Session.SetString("CantProductos", carro.carroProducto.Count().ToString());

                    HttpContext.Session.SetString("SignIn", JsonConvert.SerializeObject(a));
                    return RedirectToAction("Index", "Cliente");
                }
            }
            else
            {
                //
                //usuario.intentos += 1;
                //if (usuario.intentos < 3)
                //{
                //    ViewData["errorInicio"] = "Error: ingresaste un dni o contraseña incorrecta(" + usuario.intentos + ")";
                //}
                //else 
                //{
                //    usuario.intentos = 0;
                //    usuario.bloqueado = true;
                //    ViewData["errorInicio"] = "Error: usuario bloqueado por exceso de intentos de inicio, contactece con un administrador para ser desbloqueado";
                //}
            }

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Registrarse(RegisterViewModel model)
        {
            Usuario u = new Usuario();
            u.dni = model.Input.DNI;
            u.nombre = model.Input.Nombre;
            u.apellido = model.Input.Apellido;
            u.mail = model.Input.Email;
            u.password = model.Input.Password;
            u.cuit_cuil = model.Input.CUIT_CUIL;

            u.esAdmin = false;
            u.esEmpresa = model.Input.esEmpresa;
            u.intentos = 0;
            u.bloqueado = false;

            _context.usuarios.Add(u);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        //CERRAR SESION
        public IActionResult CerrarSesion()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //REGISTRO
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
