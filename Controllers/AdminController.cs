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

namespace Final_Plataformas_De_Desarrollo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly MyContext _context;

        public AdminController(ILogger<AdminController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        // #######################################################################################
        //                                  INDEX
        //                                  PRODUCTO
        //                                  CATEGORIA
        //                                  USUARIO
        //                                  CARRO
        //                                  COMPRA
        // #######################################################################################

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Producto()
        {
            return RedirectToAction("Index", "Producto");
        }
        public IActionResult Categoria()
        {
            return RedirectToAction("Index", "Categoria");
        }
        public IActionResult Usuario()
        {
            return RedirectToAction("Index", "Usuario");
        }
        public IActionResult Carro()
        {
            return RedirectToAction("Index", "Carro");
        }
        public IActionResult Compra()
        {
            return RedirectToAction("Index", "Compra");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
