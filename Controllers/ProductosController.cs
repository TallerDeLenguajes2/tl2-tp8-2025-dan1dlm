using SistemaVentas.Web.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_dan1dlm.Models;
using tl2_tp8_2025_dan1dlm.Interfaces;


namespace tl2_tp8_2025_dan1dlm.Controllers;

public class ProductosController : Controller
{
    private readonly IProductosRepository _productosRepository;

    private readonly IAuthenticationService _authServices;

    public ProductosController(IProductosRepository productosRepository, IAuthenticationService authServices)
    {
        _productosRepository = productosRepository;
        _authServices = authServices;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Index()
    {

        // Aplicamos el chequeo de seguridad
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        List<Productos> productos = _productosRepository.GetProductos();
        return View(productos);
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authServices.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authServices.HasAccessLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }

    public IActionResult AccesoDenegado()
    {
        // El usuario está logueado, pero no tiene el rol suficiente.
        return View();
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
        if (!ModelState.IsValid)
        {
            //Devuelvo el viewModel con los datos y errores de la Vista
            return View(productoVm);
        }

        var producto = new Productos(productoVm);

        //Llamada al repositorio:
        _productosRepository.Create(producto);

        return RedirectToAction(nameof(Index));
    }


    //POST: Productos/Edit
    [HttpPost]
    public IActionResult Edit(int id, ProductosViewModel productoVm)
    {
        if (id != productoVm.idProducto) return NotFound();

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

        _productosRepository.Update(productoEditar);

        return RedirectToAction(nameof(Index));
    }


}
