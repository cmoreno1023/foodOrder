using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foodOrder.Data;
using foodOrder.Models;

namespace foodOrder.Controllers
{
    public class PedidosController : Controller
    {
        private readonly FoodOrderContext _context;

        public PedidosController(FoodOrderContext context)
        {
            _context = context;
        }

        // Mostrar pedidos
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .ToListAsync();

            return View(pedidos);
        }

        // Mostrar formulario para crear
        public IActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(
                _context.Clientes, "IdCliente", "Nombre");

            return View();
        }

        // Guardar pedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdCliente = new SelectList(
                _context.Clientes, "IdCliente", "Nombre",
                pedido.IdCliente);

            return View(pedido);
        }

        // Mostrar formulario para editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            ViewBag.IdCliente = new SelectList(
                _context.Clientes, "IdCliente", "Nombre",
                pedido.IdCliente);

            return View(pedido);
        }

        // Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (id != pedido.IdPedido)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdCliente = new SelectList(
                _context.Clientes, "IdCliente", "Nombre",
                pedido.IdCliente);

            return View(pedido);
        }

        // Ver detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        // Confirmar eliminación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        // Eliminar pedido
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}