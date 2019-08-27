using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceReference;

namespace Ingenieria_de_Software.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteContext _context;

        public ClientesController(ClienteContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string searchString)
        {
            var clientes = from m in _context.Cliente select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.nombre.Contains(searchString));
            }

            return View(await clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,dni,nombre,domicilio")] Cliente cliente)
        {
            string codigoGrupo = "75263b76-71fc-4d0e-b5e6-7daa8e947319";
            ServicioPublicoCreditoClient cli = new ServicioPublicoCreditoClient();
            var response = cli.ObtenerEstadoClienteAsync(codigoGrupo, cliente.dni).Result;
            ViewData["creditos"]=response.CantidadCreditosActivos;
            ViewData["esValida"]=response.ConsultaValida;
            ViewData["tieneDeudas"]=response.TieneDeudas;
            ViewData["error"]=response.Error;
            if (ModelState.IsValid & response.Error == null)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,dni,nombre,domicilio")] Cliente cliente)
        {
            if (id != cliente.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.id == id);
        }

        public async Task<IActionResult> Credito(int? dni)
        {
            string codigoGrupo = "75263b76-71fc-4d0e-b5e6-7daa8e947319";
            ServicioPublicoCreditoClient cli = new ServicioPublicoCreditoClient();
            var response = cli.ObtenerEstadoClienteAsync(codigoGrupo, dni.GetValueOrDefault()).Result;
            ViewData["creditos"]=response.CantidadCreditosActivos;
            ViewData["esValida"]=response.ConsultaValida;
            ViewData["tieneDeudas"]=response.TieneDeudas;
            ViewData["error"]=response.Error;
            var clientes = from m in _context.Cliente select m;
            ViewData["dni"] = dni;
            if (!String.IsNullOrEmpty(dni.ToString()))
            {
                clientes = clientes.Where(s => s.dni.Equals(dni));
            }
            return View(await clientes.ToListAsync());

        }
    }
}
