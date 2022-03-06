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
    public class CompraController : Controller
    {
        private readonly MyContext _context;

        public CompraController(MyContext context)
        {
            _context = context;
        }

        // GET: Compra
        public async Task<IActionResult> Index()
        {
            var myContext = _context.compras
                                .Include(c => c.compraProducto)
                                .ThenInclude(cp => cp.producto);
            return View(await myContext.ToListAsync());
        }

        
        // GET: Compra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["idUsuario"] = new SelectList(_context.usuarios, "idUsuario", "nombre", compra.idUsuario);
            return View(compra);
        }

        // POST: Compra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idCompra,idUsuario,total")] Compra compra)
        {
            if (id != compra.idCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    compra.fechaCreacion = DateTime.Now;
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.idCompra))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                TempData["Mensaje"] = "Compra editada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUsuario"] = new SelectList(_context.usuarios, "idUsuario", "nombre", compra.idUsuario);
            return View(compra);
        }
        public async Task<IActionResult> Eliminar(int id)
        {
            var compra = await _context.compras.FindAsync(id);
            _context.compras.Remove(compra);
            await _context.SaveChangesAsync();

            
            TempData["Mensaje"] = "Compra eliminada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.compras.Any(e => e.idCompra == id);
        }
    }
}
