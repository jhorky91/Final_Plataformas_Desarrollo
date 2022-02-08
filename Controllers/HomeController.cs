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
            //ViewData["Login"] = "ERROR";
            HttpContext.Session.SetString("SighIn", JsonConvert.SerializeObject(new Account()));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            var usuario = await _context.usuarios.FirstOrDefaultAsync(u => u.dni == model.Input.DNI);
            
            if (usuario != null && usuario.password == model.Input.Password)
            {
                if (usuario.rol == 1)
                {
                    Account a = new Account();
                    a.name = usuario.nombre;
                    a.id = usuario.idUsuario;
                    a.rol = usuario.rol;
                    a.signIn = true;
                    HttpContext.Session.SetString("SighIn", JsonConvert.SerializeObject(a));
                    return RedirectToAction("Index", "Admin");
                }
                else 
                {
                    //TempData["Login"] = true;
                    //TempData.Keep("Login");
                    //HttpContext.Session.SetString("SighIn", JsonConvert.SerializeObject(usuario));
                    Account a = new Account();
                    a.name = usuario.nombre;
                    a.id = usuario.idUsuario;
                    a.rol = usuario.rol;
                    a.signIn = true;
                    HttpContext.Session.SetString("SighIn", JsonConvert.SerializeObject(a));
                    return RedirectToAction("Index","Cliente");
                }
            }
           
            return RedirectToAction("Login"); 
        }

        public async Task<IActionResult> Registrarse(RegisterViewModel model)
        {
            Usuario u = new Usuario();
            u.dni   =   model.Input.DNI;
            u.nombre  =  model.Input.Nombre;
            u.apellido = model.Input.Apellido;
            u.mail  = model.Input.Email;
            u.password = model.Input.Password;
            u.cuit_cuil = model.Input.CUIT_CUIL;
            u.rol = model.Input.Rol;
            




            return RedirectToAction("Login");
        }

        //CERRAR SESION
        public IActionResult CerrarSesion()
        {
            //TempData["Login"] = null;
            HttpContext.Session.SetString("SighIn", JsonConvert.SerializeObject(new Account()));
            return RedirectToAction("Login");
        }

        //CLIENTE
        public IActionResult Index()
        {
            //ViewData["Login"] = true;
            var productos = _context.productos;
            return View(productos);
        }

        //REGISTRO
        public IActionResult Register()
        {
            return View();
        }

       
        public IActionResult Privacy()
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