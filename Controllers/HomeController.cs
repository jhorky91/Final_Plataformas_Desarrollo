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
        public async Task<IActionResult> IniciarSesion(FormulariosViewModel model)
        {
            var usuario = await _context.usuarios
                                    .FirstOrDefaultAsync(u => u.dni == model.LoginInput.DNI);

            if (usuario != null)
            {
                if (usuario.password == model.LoginInput.Password)
                {
                    //ADMIN
                    if (usuario.esAdmin)
                    {
                        Account a = new Account();
                        a.name = usuario.nombre;
                        a.id = usuario.idUsuario;
                        a.esAdmin = usuario.esAdmin;
                        a.signIn = true;

                        usuario.intentos = 0;
                        _context.usuarios.Update(usuario);
                        await _context.SaveChangesAsync();

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

                        usuario.intentos = 0;
                        _context.usuarios.Update(usuario);
                        await _context.SaveChangesAsync();

                        HttpContext.Session.SetString("CantProductos", carro.carroProducto.Count().ToString());
                        HttpContext.Session.SetString("SignIn", JsonConvert.SerializeObject(a));
                        TempData["Mensaje"] = "Bienvenido "+usuario.nombre+".";
                        return RedirectToAction("Index", "Cliente");
                    }
                }
                else
                {

                    usuario.intentos += 1;
                    if (usuario.intentos < 3)
                    {
                        TempData["Mensaje"] = "Error: ingresaste una contraseña incorrecta.";
                    }
                    else
                    {
                        usuario.intentos = 0;
                        usuario.bloqueado = true;
                        TempData["Mensaje"] = "Error: usuario bloqueado por exceso de intentos de inicio, contactece con un administrador para ser desbloqueado";
                    }
                    TempData["TituloMensaje"] = "Contraseña incorrecta";

                    _context.usuarios.Update(usuario);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login");
                }
            }
            else 
            {
                TempData["TituloMensaje"] = "Usuario incorrecto";
                TempData["Mensaje"] = "Vuelva a intentar iniciar sesion";
                return RedirectToAction("Login");
            }
            
        }

        public async Task<IActionResult> Registrarse(FormulariosViewModel model)
        {
            Usuario u = new Usuario();
            u.dni = model.RegisterInput.DNI;
            u.nombre = model.RegisterInput.Nombre;
            u.apellido = model.RegisterInput.Apellido;
            u.mail = model.RegisterInput.Email;
            u.password = model.RegisterInput.Password;
            u.cuit_cuil = model.RegisterInput.CUIT_CUIL;

            u.esAdmin = false;
            u.esEmpresa = model.RegisterInput.esEmpresa;
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
