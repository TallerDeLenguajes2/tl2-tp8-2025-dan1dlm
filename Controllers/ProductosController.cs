using SistemaVentas.Web.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_dan1dlm.Models;


namespace tl2_tp8_2025_dan1dlm.Controllers;

public class ProductosController : Controller
{
    private readonly ProductosRepository _repo;

    private readonly ILogger<ProductosController> _logger;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        _repo = new ProductosRepository();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Index()
    {
        List<Productos> productos = _repo.GetProductos();

        return View(productos);
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


     //INICIAN LOS METODOS

    //POST: Productos/Create
    [HttpPost]
    public IActionResult Create(ProductosViewModel productoVm)
    {
        //Chequeo de Seguridad del servidor
        if(!ModelState.IsValid)
        {
            //Devuelvo el viewModel con los datos y errores de la Vista
            return View(productoVm);
        }

        var producto = new Productos(productoVm);

        //Llamada al repositorio:
        _repo.Create(producto);

        return RedirectToAction(nameof(Index));
    }


    //POST: Productos/Edit
    [HttpPost]
    public IActionResult Edit(int id, ProductosViewModel productoVm)
    {
        if(id != productoVm.idProducto) return NotFound();

        if (!ModelState.IsValid)
        {
            return View(productoVm);
        }

        var productoEditar = new Productos
        {
            IdProducto = id,
            Descripcion = productoVm.Descripcion,
            Precio = productoVm.Precio
        };

        _repo.Update(productoEditar);

        return RedirectToAction(nameof(Index));
    }


}
