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
    public class CamposController : Controller
    {
        private readonly ArmadilloContext _context;

        public CamposController(ArmadilloContext context)
        {
            _context = context;
        }

        // GET: Campos
        public async Task<IActionResult> Index(int idHoja)
        {
            var applicationDbContext = _context.Campo.Include(c => c.Hoja).Include(c => c.Tipo).Where(d=>d.IdHoja== idHoja);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Campos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campo == null)
            {
                return NotFound();
            }

            var campo = await _context.Campo
                .Include(c => c.Hoja)
                .Include(c => c.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campo == null)
            {
                return NotFound();
            }

            return View(campo);
        }

        // GET: Campos/Create
        public IActionResult Create()
        {
            ViewData["IdHoja"] = new SelectList(_context.Hoja, "Id", "Nombre");
            ViewData["IdTipo"] = new SelectList(_context.Tipo, "Id", "Nombre");
            return View();
        }

        // POST: Campos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Indice,IdTipo,Nombre,IdHoja,Calculo")] Campo campo)
        {
            _context.Add(campo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { idHoja = campo.IdHoja });
            
        }

        // GET: Campos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campo == null)
            {
                return NotFound();
            }

            var campo = await _context.Campo.FindAsync(id);
            if (campo == null)
            {
                return NotFound();
            }
            ViewData["IdHoja"] = new SelectList(_context.Hoja, "Id", "Id", campo.IdHoja);
            ViewData["IdTipo"] = new SelectList(_context.Tipo, "Id", "Id", campo.IdTipo);
            return View(campo);
        }

        // POST: Campos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Indice,IdTipo,Nombre,IdHoja,Calculo")] Campo campo)
        {
            if (id != campo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampoExists(campo.Id))
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
            ViewData["IdHoja"] = new SelectList(_context.Hoja, "Id", "Id", campo.IdHoja);
            ViewData["IdTipo"] = new SelectList(_context.Tipo, "Id", "Id", campo.IdTipo);
            return View(campo);
        }

        // GET: Campos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Campo == null)
            {
                return NotFound();
            }

            var campo = await _context.Campo
                .Include(c => c.Hoja)
                .Include(c => c.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campo == null)
            {
                return NotFound();
            }

            return View(campo);
        }

        // POST: Campos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Campo'  is null.");
            }
            var campo = await _context.Campo.FindAsync(id);
            if (campo != null)
            {
                _context.Campo.Remove(campo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampoExists(int id)
        {
          return (_context.Campo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
