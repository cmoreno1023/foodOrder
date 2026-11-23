using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foodOrder.Data;
using foodOrder.Models;

namespace foodOrder.Controllers
{
    public class DetallePedidosController : Controller
    {
        private readonly FoodOrderContext _context;

        public DetallePedidosController(FoodOrderContext context)
        {
            _context = context;
        }

        // Mostrar detalles de pedidos
        public async Task<IActionResult> Index()
        {
            var detalles = await _context.DetallePedido
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .ToListAsync();

            return View(detalles);
        }

        // Mostrar formulario para crear
        public IActionResult Create()
        {
            CargarListas();
            return View();
        }

        // Guardar detalle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetallePedido detalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CargarListas(detalle);
            return View(detalle);
        }

        // Mostrar formulario para editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var detalle = await _context.DetallePedido.FindAsync(id);

            if (detalle == null)
                return NotFound();

            CargarListas(detalle);
            return View(detalle);
        }

        // Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DetallePedido detalle)
        {
            if (id != detalle.IdDetalle)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(detalle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CargarListas(detalle);
            return View(detalle);
        }

        // Ver detalles
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var detalle = await _context.DetallePedido
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.IdDetalle == id);

            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        // Confirmar eliminación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var detalle = await _context.DetallePedido
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.IdDetalle == id);

            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        // Eliminar detalle
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalle = await _context.DetallePedido.FindAsync(id);

            if (detalle != null)
            {
                _context.DetallePedido.Remove(detalle);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Cargar listas de pedidos y productos
        private void CargarListas(DetallePedido? detalle = null)
        {
            ViewBag.IdPedido = new SelectList(
                _context.Pedidos,
                "IdPedido",
                "IdPedido",
                detalle?.IdPedido);

            ViewBag.IdProducto = new SelectList(
                _context.Productos,
                "IdProducto",
                "Nombre",
                detalle?.IdProducto);
        }
    }
}