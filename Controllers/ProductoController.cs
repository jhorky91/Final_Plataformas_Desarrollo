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
    public class ProductoController : Controller
    {
        private readonly MyContext _context;

        public ProductoController(MyContext context)
        {
            _context = context;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var myContext = _context.productos.Include(p => p.cat);
            return View(await myContext.ToListAsync());
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            ViewData["idCategoria"] = new SelectList(_context.categorias, "idCategoria", "nombre");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idProducto,nombre,precio,cantidad,idCategoria,fechaCreacion")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Producto creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["idCategoria"] = new SelectList(_context.categorias, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["idCategoria"] = new SelectList(_context.categorias, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProducto,nombre,precio,cantidad,idCategoria,fechaCreacion")] Producto producto)
        {
            if (id != producto.idProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.idProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                TempData["Mensaje"] = "Producto modificado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["idCategoria"] = new SelectList(_context.categorias, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.productos.FindAsync(id);
            _context.productos.Remove(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Producto eliminado exitosamente";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.productos.Any(e => e.idProducto == id);
        }
    }
}
