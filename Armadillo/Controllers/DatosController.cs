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
    public class DatosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DatosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> LlenarHoja(int idHoja)
        {
            List<Campo> CamposHoja =await _context.Campo.Where(d=>d.IdHoja==idHoja).ToListAsync();
            return View(CamposHoja);
        }



        // GET: Datos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dato.Include(d => d.Campo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Datos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dato == null)
            {
                return NotFound();
            }

            var dato = await _context.Dato
                .Include(d => d.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dato == null)
            {
                return NotFound();
            }

            return View(dato);
        }

        // GET: Datos/Create
        public IActionResult Create()
        {
            ViewData["IdCampo"] = new SelectList(_context.Campo, "Id", "Id");
            return View();
        }

        // POST: Datos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Indice,IdCampo,Valor")] Dato dato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampo"] = new SelectList(_context.Campo, "Id", "Id", dato.IdCampo);
            return View(dato);
        }

        // GET: Datos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dato == null)
            {
                return NotFound();
            }

            var dato = await _context.Dato.FindAsync(id);
            if (dato == null)
            {
                return NotFound();
            }
            ViewData["IdCampo"] = new SelectList(_context.Campo, "Id", "Id", dato.IdCampo);
            return View(dato);
        }

        // POST: Datos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Indice,IdCampo,Valor")] Dato dato)
        {
            if (id != dato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatoExists(dato.Id))
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
            ViewData["IdCampo"] = new SelectList(_context.Campo, "Id", "Id", dato.IdCampo);
            return View(dato);
        }

        // GET: Datos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dato == null)
            {
                return NotFound();
            }

            var dato = await _context.Dato
                .Include(d => d.Campo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dato == null)
            {
                return NotFound();
            }

            return View(dato);
        }

        // POST: Datos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dato == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Dato'  is null.");
            }
            var dato = await _context.Dato.FindAsync(id);
            if (dato != null)
            {
                _context.Dato.Remove(dato);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatoExists(int id)
        {
          return (_context.Dato?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
