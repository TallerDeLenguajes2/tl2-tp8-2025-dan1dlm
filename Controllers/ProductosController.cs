using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_dan1dlm.Models;

namespace tl2_tp8_2025_dan1dlm.Controllers;

public class ProductosController : Controller
{
    private ProductosRepository productosRepository;
    

    private readonly ILogger<ProductosController> _logger;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        productosRepository = new ProductosRepository();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]

    
    //INICIAN LOS METOODOS
    public IActionResult Index()
    {
        List<Productos> productos = productosRepository.GetProductos();
         
        return View(productos);
    }


    

    
}
