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
    public class ProgramasController : Controller
    {
        private readonly ArmadilloContext _context;

        public ProgramasController(ArmadilloContext context)
        {
            _context = context;
        }

        // GET: Programas
        public async Task<IActionResult> Index()
        {
              return _context.Programa != null ? 
                          View(await _context.Programa.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Programa'  is null.");
        }

        // GET: Programas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Programa == null)
            {
                return NotFound();
            }

            var programa = await _context.Programa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programa == null)
            {
                return NotFound();
            }

            return View(programa);
        }

        // GET: Programas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] Programa programa)
        {
            _context.Add(programa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Programas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Programa == null)
            {
                return NotFound();
            }

            var programa = await _context.Programa.FindAsync(id);
            if (programa == null)
            {
                return NotFound();
            }
            return View(programa);
        }

        // POST: Programas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Programa programa)
        {
            if (id != programa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramaExists(programa.Id))
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
            return View(programa);
        }

        // GET: Programas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Programa == null)
            {
                return NotFound();
            }

            var programa = await _context.Programa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programa == null)
            {
                return NotFound();
            }

            return View(programa);
        }

        // POST: Programas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Programa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Programa'  is null.");
            }
            var programa = await _context.Programa.FindAsync(id);
            if (programa != null)
            {
                _context.Programa.Remove(programa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramaExists(int id)
        {
          return (_context.Programa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
