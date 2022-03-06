using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final_Plataformas_De_Desarrollo.Data;
using Final_Plataformas_De_Desarrollo.Models;

namespace Final_Plataformas_De_Desarrollo.Controllers
{
    public class CarroController : Controller
    {
        private readonly MyContext _context;

        public CarroController(MyContext context)
        {
            _context = context;
        }

        // GET: Carro
        public async Task<IActionResult> Index()
        {
            var myContext = _context.carros
                                .Include(c => c.carroProducto)
                                .ThenInclude(cp => cp.producto);
            return View(await myContext.ToListAsync());
        }

    }
}
