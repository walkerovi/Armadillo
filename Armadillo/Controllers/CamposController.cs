﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armadillo.Data;
using Armadillo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            Hoja hoja = _context.Hoja.Include(d => d.Programa).Single(d => d.Id == idHoja);
            ViewBag.Hoja = hoja;
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
        public IActionResult Create(int idHoja)
        {
            Hoja hoja = _context
                .Hoja
                .Include(d=>d.Campos)
                .Include(c => c.Programa).AsNoTracking()
                .Single(t => t.Id == idHoja);
            ViewBag.Hoja = hoja;
            int UltimoIndice = 0;
            if (hoja != null)
            { 
                if(hoja.Campos.Count>0)
                    UltimoIndice= hoja.Campos.OrderBy(d => d.Indice).Last().Indice;
            }
            ViewBag.UltimoCampo = UltimoIndice+1;
            ViewData["IdTipo"] = new SelectList(_context.Tipo, "Id", "Nombre");
            var hojas = _context.Hoja.AsNoTracking().Where(d => d.IdPrograma == hoja.IdPrograma);/*solo debe ser por ahora del programa seleccionado*/
            ViewData["IdHojaForanea"] = new SelectList(hojas, "Id", "Nombre");/*Hoja hija*/

            /*servir lista de campos para elegir un campo foráneo*/
            if (hoja != null)
            {
                var campos = hoja.Campos;
                if (campos.Count > 0)
                {
                    if (campos.Any(d => d.IdTipo == 7))
                    {
                        var campo_foraneo = campos.Single(d => d.IdTipo == 7);/*7 es para detalle*/
                        if (campo_foraneo != null)
                        {
                            int idHojaForanea = Convert.ToInt32(campo_foraneo.Calculo);
                            var CamposForaneos = _context.Campo.AsNoTracking().Where(d => d.IdHoja == idHojaForanea && d.IdTipo != 6 && d.IdTipo != 7);

                            ViewData["IdCampoForaneo"] = new SelectList(CamposForaneos, "Id", "Nombre");
                        }
                    }
                }
            }

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

            try
            {
                _context.Update(campo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idHoja = campo.IdHoja });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampoExists(campo.Id))
                {
                    return NotFound("El registro no fue encontrado");
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Campos/Delete/5
        [HttpGet]
        public async Task<IActionResult> Borrar(int idCampo, int idHoja)
        {
            if (_context.Campo == null)
            {
                return Problem("no hay campos");
            }
            var campo = await _context.Campo.FindAsync(idCampo);
            if (campo != null)
            {
                _context.Campo.Remove(campo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idHoja = campo.IdHoja });
            }
            else
                return BadRequest("No se encontró el registro");
        }


        private bool CampoExists(int id)
        {
          return (_context.Campo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
