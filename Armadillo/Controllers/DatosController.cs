using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armadillo.Data;
using Armadillo.Models;
using Armadillo.Models.ModelViews;
using Newtonsoft.Json;

namespace Armadillo.Controllers
{
    public class DatosController : Controller
    {
        private readonly ArmadilloContext _context;

        public DatosController(ArmadilloContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> LlenarHoja(int idHoja)
        {
            List<Campo> CamposHoja =await _context.Campo.Where(d=>d.IdHoja==idHoja).ToListAsync();
            return View(CamposHoja);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarValoresHoja()
        {
            var item =Request.Form.Keys.AsEnumerable().FirstOrDefault();
            List<CampoJson> datos = JsonConvert.DeserializeObject<List<CampoJson>>(item);

            /*entender la última fila que exista*/
            var idCampoDefault = datos.FirstOrDefault().idCampo;
            int noFila = 0;
            bool tieneFila = _context.Campo.Include(d=>d.Datos).Single(d => d.Id == idCampoDefault).Datos.Any();
            if (tieneFila)
                noFila = _context.Campo.Include(d => d.Datos).Single(d => d.Id == idCampoDefault).Datos.OrderBy(d => d.NoFila).Last().NoFila + 1;
            else
                noFila = 1;

            foreach (CampoJson campo in datos)
            {
                Dato dato = new Dato();
                var campocompleto = await _context.Campo.SingleAsync(d => d.Id == campo.idCampo);
                dato.IdCampo =campo.idCampo/*hay que buscarlo*/;
                dato.Indice =campocompleto.Indice/*del mismo campo se trae*/;
                dato.NoFila = noFila;
                dato.Valor = campo.valor;
                _context.Add(dato);
            }
            await _context.SaveChangesAsync();
            return Ok("Se han guardado los datos");
        }

        [HttpGet]
        public async Task<IActionResult> MostrarDatos(int idHoja)
        {
            var hoja =await _context.Hoja.SingleAsync(d=>d.Id==idHoja);
            List<string> cabeceras = new List<string>();
            List<Campo> campos =await _context
                .Campo
                .Where(d=>d.IdHoja==idHoja)
                .OrderBy(d=>d.Indice)
                .ToListAsync();
            foreach (var item in campos)
                cabeceras.Add(item.Nombre);
            List<Dato> datos =await _context.Dato.Include(d=>d.Campo).Where(d => d.Campo.IdHoja == idHoja).OrderBy(d=>d.Indice).ToListAsync();
            Contenido contenido = new Contenido();
            contenido.Campos = cabeceras;
            contenido.Datos = datos;
            contenido.Cantidadfila = datos.OrderBy(d => d.NoFila).Last().NoFila;
            return View(contenido);
        }





        /*Generado por el framework*/


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
