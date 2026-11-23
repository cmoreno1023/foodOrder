using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using foodOrder.Models;
using Microsoft.EntityFrameworkCore;
using foodOrder.Data;

namespace foodOrder.Controllers;

public class HomeController : Controller
{
    private readonly FoodOrderContext _context;

public HomeController(FoodOrderContext context)
{
    _context = context;
}
   public async Task<IActionResult> Index()
{
    ViewBag.TotalProductos = await _context.Productos.CountAsync();
    ViewBag.TotalClientes = await _context.Clientes.CountAsync();
    ViewBag.TotalPedidos = await _context.Pedidos.CountAsync();
    ViewBag.TotalCategorias = await _context.Categorias.CountAsync();

    return View();
}

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ProbarConexion()
{
    try
    {
        var categorias = _context.Categorias.ToList();

        return Content("Conexión exitosa con FoodOrderDB. Categorías encontradas: " + categorias.Count);
    }
    catch (Exception ex)
    {
        return Content("Error al conectar con la base de datos: " + ex.Message);
    }
}   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
