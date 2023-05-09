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
using System.Linq.Expressions;

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
        public async Task<IActionResult> LlenarHoja(int idHoja, int idHojaForanea = 0, int noFilaForanea = 0)
        {
            ViewBag.idHoja = idHoja;
            ViewBag.idHojaForanea = idHojaForanea;
            ViewBag.noFilaForanea = noFilaForanea;

            List<Campo> CamposHoja =await _context.Campo.Where(d=>d.IdHoja==idHoja).ToListAsync();
            Hoja hoja = _context.Hoja.Include(d => d.Programa).Single(d=>d.Id==idHoja);
            List<Hoja> hojas = _context.Hoja.Where(d => d.IdPrograma == hoja.IdPrograma).AsNoTracking().ToList();/*solo las hojas del mismo programa*/

            ViewBag.Hojas = new SelectList(hojas, "Id", "Nombre");
            ViewBag.Hoja= hoja;
            return View(CamposHoja);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarValoresHoja()
        {
            var item =Request.Form.Keys.AsEnumerable().FirstOrDefault();
            List<CampoJson> datos = JsonConvert.DeserializeObject<List<CampoJson>>(item);

            /*entender la última fila que exista*/
            var idCampoDefault = datos.FirstOrDefault().idCampo;
            Campo campoDefault = _context.Campo.Include(d => d.Datos).Single(d => d.Id == idCampoDefault);
            int noFila = 0;
            bool tieneFila = campoDefault.Datos.Any();
            if (tieneFila)
                noFila = campoDefault.Datos.OrderBy(d => d.NoFila).Last().NoFila + 1;
            else
                noFila = 1;

            foreach (CampoJson campo in datos)
            {
                Dato dato = new Dato();
                var campocompleto = await _context.Campo.SingleAsync(d => d.Id == campo.idCampo);
                dato.IdCampo =campo.idCampo/*hay que buscarlo*/;
                dato.Indice =campocompleto.Indice/*del mismo campo se trae*/;
                dato.NoFila = noFila;

                if (campocompleto.IdTipo==8) {/*es un Campo Foráneo*/
                    var campoConocer = _context.Campo.AsNoTracking().Single(d=>d.Id==campo.idCampo);
                    var hojaMadre = _context.Hoja.Include(c => c.Campos).Single(d=>d.Id==campoConocer.IdHoja);
                    var campoClave = hojaMadre.Campos.Single(d=>d.IdTipo==7);
                    var resolucion = datos.Single(d=>d.idCampo==campoClave.Id);
                    /*traer los valores*/
                    var tupla = resolucion.valor.Split(",");
                    int idHojaForanea = Convert.ToInt16(tupla[0]);
                    int noFilaForanea = Convert.ToInt16(tupla[1]);
                    int idcampoForaneo = Convert.ToInt16(campoConocer.Calculo);
                    var DatoForaneo = _context
                        .Dato
                        .AsNoTracking()
                        .Include(d=>d.Campo)
                        .Single(d=>d.NoFila==noFilaForanea && d.Campo.IdHoja==idHojaForanea && d.IdCampo== idcampoForaneo);
                    dato.Valor = DatoForaneo.Valor;
                }else
                    dato.Valor = campo.valor;
                _context.Add(dato);
            }
            await _context.SaveChangesAsync();
            return Ok("Es correcto");
        }

        [HttpGet]
        public async Task<IActionResult> MostrarDatos(int idHoja,int idHojaForanea = 0, int noFilaForanea=0)
        {
            ViewBag.idHoja = idHoja;
            ViewBag.idHojaForanea = idHojaForanea;
            ViewBag.noFilaForanea = noFilaForanea;
            ViewBag.IdPrograma = _context.Hoja.AsNoTracking().Single(d => d.Id == idHoja).IdPrograma;
            if (idHojaForanea > 0)
            {
                Contenido contenidoForaneo = new Contenido();
                contenidoForaneo = await ObtenerContenido(idHoja, idHojaForanea, noFilaForanea);
                ViewBag.ContenidoForanea = contenidoForaneo;
                Contenido contenido = await ObtenerContenido(idHoja, idHojaForanea, noFilaForanea,true);
                return View(contenido);
            }
            else
                return View(await ObtenerContenido(idHoja));
        }

        /*Select mostrar datos desde el hijo*/
        [HttpGet]
        public async Task<IActionResult> MostrarDatosSelect(int idHoja,string busqueda)
        {
            var contenido=await ObtenerContenido(idHoja, 0, 0);
            List<Dato> obtenerFilas = contenido.Datos.Where(d => d.Valor.Contains(busqueda)).ToList();
            List<Dato> datosFiltrados = new List<Dato>();
            foreach (var item in obtenerFilas)
            {
                List<Dato> dato = contenido.Datos.Where(d=>d.NoFila==item.NoFila).ToList();
                datosFiltrados.AddRange(dato);
            }
            contenido.Datos = datosFiltrados;
            return PartialView(contenido);
        }


        private async Task<Contenido> ObtenerContenido(
            int idHoja, 
            int idHojaForanea=0,
            int noFilaForanea = 0,
            bool EsDetalle=false)
        {
            var hoja = await _context.Hoja.Include(d => d.Programa).SingleAsync(d => d.Id == idHoja);
            List<string> cabeceras = new List<string>();
            List<Campo> campos = new List<Campo>();
            List<Dato> datos = new List<Dato>();
            if (idHojaForanea > 0 && !EsDetalle)
            {
                campos = await _context
                .Campo
                .Where(d => d.IdHoja == idHojaForanea)
                .OrderBy(d => d.Indice)
                .ToListAsync();
                foreach (var item in campos)
                    cabeceras.Add(item.Nombre);

                datos = await _context
                .Dato
                .Include(d => d.Campo)
                .Where(d => d.Campo.IdHoja == idHojaForanea && d.NoFila == noFilaForanea)
                .OrderBy(d => d.Indice)
                .ToListAsync();
            }
            else
            {
                campos = await _context
                .Campo
                .Where(d => d.IdHoja == idHoja)
                .OrderBy(d => d.Indice)
                .ToListAsync();
                foreach (var item in campos)
                    cabeceras.Add(item.Nombre);
                if (idHojaForanea > 0 && EsDetalle)
                {
                    string dupla = string.Format("{0},{1}", idHojaForanea, noFilaForanea);
                    var lasFilas = _context
                    .Dato
                    .AsNoTracking()
                    .Include(d => d.Campo)
                    .Where(d => d.Campo.IdHoja == idHoja && d.Valor == dupla)
                    .OrderBy(d => d.Indice)
                    .Select(d=>d.NoFila)
                    .ToList();

                    datos = new List<Dato>();/*reset el objeto*/

                    foreach (var item in lasFilas)
                    {
                        var datoFila = _context
                            .Dato
                            .AsNoTracking()
                            .Include(d => d.Campo)
                            .Where(d => d.Campo.IdHoja == idHoja && d.NoFila == item).ToList();
                        if(datoFila != null)
                            datos.AddRange(datoFila);
                    }
                    datos = datos.OrderBy(d=>d.Indice).ToList();
                }
                else
                    datos = await _context
                    .Dato
                    .Include(d => d.Campo)
                    .Where(d => d.Campo.IdHoja == idHoja)
                    .OrderBy(d => d.Indice)
                    .ToListAsync();
            }
            
            Contenido contenido = new Contenido();
            contenido.Campos = cabeceras;
            contenido.Datos = EjecutarFormula(datos);
            contenido.NombreHoja = hoja.Nombre;
            contenido.idHoja = idHoja;
            contenido.NombrePrograma = hoja.Programa.Nombre;
            contenido.Cantidadfila = datos.Count > 0 ? datos.OrderBy(d => d.NoFila).Last().NoFila : 0;

            return contenido;
        }


        private List<Dato> LlenarCamposForaneos(List<Dato> datos)
        {
            foreach (var item in datos)
            {
                if (item.Campo.IdTipo == 8)/*para campos foráneos*/
                {
                    var fila = item.NoFila;
                    var idCampoForaneo=Convert.ToInt32(item.Campo.Calculo);
                    var datoForaneo = datos.Single(d=>d.NoFila==fila && d.Campo.IdTipo==7);/*el que contiene la tupla*/
                    var tupla = datoForaneo.Valor.Split(',');
                    var idHojaForanea = Convert.ToInt32(tupla[0]);
                    var valorForaneo = _context
                        .Dato.Include(d => d.Campo)
                        .Single(d => d.Campo.IdHoja == idHojaForanea && d.NoFila==fila && d.IdCampo== idCampoForaneo);
                    item.Valor = valorForaneo.Valor;
                }
            }
            return datos;
        }


        /*Implementar para ingresar fórmulas*/
        private List<Dato> EjecutarFormula(List<Dato> datos)
        {
            foreach (var dato in datos)
            {   /*datos del campo*/
                if (dato.Campo.IdTipo == 5)/*si es cálculo se va a trabajar*/
                {
                    /*ahora sustituir los datos*/
                    var todosloscamposutilizar = datos
                        .Where(c => c.NoFila == dato.NoFila)
                        .OrderBy(d=>d.Indice)
                        .ToList();/*sub lista, de la misma fila*/
                    string expression = dato.Campo.Calculo;
                    foreach (var item in todosloscamposutilizar)
                        expression = expression.Replace(item.Campo.Nombre, item.Valor);
                    System.Data.DataTable table = new System.Data.DataTable();
                    object result = table.Compute(expression, string.Empty);
                    var respuesta = Convert.ToDouble(result);
                    dato.Valor = respuesta.ToString();
                }
            }
            return datos;
        }

        [HttpGet]
        public IActionResult EditarForm(int idHoja, int noFila)
        {
            List<Dato> datos = _context
                .Dato
                .AsNoTracking()
                .Include(d=>d.Campo)
                .Where(d=>d.Campo.IdHoja==idHoja && d.NoFila==noFila).ToList();

            return View(datos);
        }

        [HttpPost]
        public async Task<IActionResult> Editar()
        {
            var item = Request.Form.Keys.AsEnumerable().FirstOrDefault();
            List<DatoJson> datos = JsonConvert.DeserializeObject<List<DatoJson>>(item);
            foreach (DatoJson datojson in datos)
            {
                Dato dato = await _context.Dato.SingleAsync(d=>d.Id== datojson.idDato);
                dato.Valor = datojson.valor;
                _context.Update(dato);
            }
            await _context.SaveChangesAsync();
            return Ok("Se Ha Actualizado");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int idHoja, int noFilaForanea, int idHojaForanea = 0)
        {
            List<Dato> datos = _context
                .Dato
                .Include(d => d.Campo)
                .Where(d => d.Campo.IdHoja == idHoja && d.NoFila == noFilaForanea).ToList();
            if (datos.Count > 0)
            {
                foreach (var item in datos)
                    _context.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MostrarDatos), new { idHoja = idHoja, idHojaForanea= idHojaForanea, noFilaForanea= noFilaForanea });
            }
            else 
                return BadRequest("No se encotró el registro");
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
