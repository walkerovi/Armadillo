using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armadillo.Data;
using Armadillo.Models;

namespace Armadillo.Controllers
{
    public class HojasController : Controller
    {
        private readonly ArmadilloContext _context;

        public HojasController(ArmadilloContext context)
        {
            _context = context;
        }

        // GET: Hojas
        public async Task<IActionResult> Index(int idPrograma)
        {
            var applicationDbContext = _context.Hoja.Include(h => h.Programa).Where(d=>d.IdPrograma==idPrograma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Hojas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hoja == null)
            {
                return NotFound();
            }

            var hoja = await _context.Hoja
                .Include(h => h.Programa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoja == null)
            {
                return NotFound();
            }

            return View(hoja);
        }

        // GET: Hojas/Create
        public IActionResult Create(int idPrograma)
        {
            Programa programa = _context.Programa.Single(d=>d.Id==idPrograma);
            ViewBag.Programa = programa;
            return View();
        }

        // POST: Hojas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPrograma,Nombre,Descripcion")] Hoja hoja)
        {
            _context.Add(hoja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { idPrograma = hoja.IdPrograma });
        }

        // GET: Hojas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hoja == null)
            {
                return NotFound();
            }

            var hoja = await _context.Hoja.FindAsync(id);
            if (hoja == null)
            {
                return NotFound();
            }
            ViewData["IdPrograma"] = new SelectList(_context.Programa, "Id", "Id", hoja.IdPrograma);
            return View(hoja);
        }

        // POST: Hojas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPrograma,Nombre,Descripcion")] Hoja hoja)
        {
            if (id != hoja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HojaExists(hoja.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPrograma"] = new SelectList(_context.Programa, "Id", "Id", hoja.IdPrograma);
            return View(hoja);
        }

        // GET: Hojas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hoja == null)
            {
                return NotFound();
            }

            var hoja = await _context.Hoja
                .Include(h => h.Programa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoja == null)
            {
                return NotFound();
            }

            return View(hoja);
        }

        // POST: Hojas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hoja == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hoja'  is null.");
            }
            var hoja = await _context.Hoja.FindAsync(id);
            if (hoja != null)
            {
                _context.Hoja.Remove(hoja);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HojaExists(int id)
        {
          return (_context.Hoja?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
