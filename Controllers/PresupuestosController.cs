using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_dan1dlm.Models;
using tl2_tp8_2025_dan1dlm.Interfaces;
using tl2_tp8_2025_dan1dlm.ViewModels;
using SistemaVentas.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace tl2_tp8_2025_dan1dlm.Controllers;


public class PresupuestosController : Controller
{
    private readonly IPresupuestosRepository _presupuestosRepository;
    private readonly IProductosRepository _productosRepository;
    private readonly IAuthenticationService _authServices;

    public PresupuestosController(IPresupuestosRepository presupuestosRepository, IProductosRepository productosRepository, IAuthenticationService authServices)
    {
        _presupuestosRepository = presupuestosRepository;
        _productosRepository = productosRepository;
        _authServices = authServices;
    }

    public IActionResult Index()
    {
        // Comprobación de si está logueado
        if (!_authServices.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        // Verifica Nivel de acceso que necesite validar
        if (_authServices.HasAccessLevel("Administrador") || _authServices.HasAccessLevel("Cliente"))
        {
            //si es es valido entra sino vuelve a login
            var presupuestos = _presupuestosRepository.GetPresupuestos();
            return View(presupuestos);
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }

    }

    public IActionResult Create()
    {
        // Comprobación de si está logueado
        if (!_authServices.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        // Verifica Nivel de acceso
        if (!_authServices.HasAccessLevel("Administrador"))
        {
            return RedirectToAction(nameof(AccesoDenegado));
        }
        // Se retorna un VM vacío para el formulario
        return View(new PresupuestosViewModel());
    }

    public IActionResult Create(PresupuestosViewModel presupuestoVm)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuestoVm);
        }

        var presupuesto = new Presupuestos(presupuestoVm);

        _presupuestosRepository.Create(presupuesto);

        return RedirectToAction(nameof(Index));
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }

    public IActionResult Edit(int id)
    {
        if (!_authServices.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        if (!_authServices.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));

        var modelo = _presupuestosRepository.GetPresupuesto(id);
        if (modelo == null)
            return NotFound();

        // Convertimos el modelo al ViewModel
        var vm = new PresupuestosViewModel(modelo);

        return View(vm);
    }

    public IActionResult Edit(int id, PresupuestosViewModel presupuestoVm)
    {
        if (!_authServices.IsAuthenticated())
            return RedirectToAction("Index", "Login");

        if (!_authServices.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));

        if (!ModelState.IsValid)
        {
            return View(presupuestoVm);
        }

        var modeloExistente = _presupuestosRepository.GetPresupuesto(id);
        if (modeloExistente == null)
            return NotFound();

        // Actualizamos SOLO los campos del encabezado
        modeloExistente.NombreDestinatario = presupuestoVm.NombreDestinatario;
        modeloExistente.FechaCreacion = presupuestoVm.FechaCreacion;

        _presupuestosRepository.Update(id, modeloExistente);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetPresupuesto(int idPresupuesto)
    {
        if (!_authServices.IsAuthenticated())
            return RedirectToAction("Index", "Login");
        if (!_authServices.HasAccessLevel("Administrador"))
            return RedirectToAction(nameof(AccesoDenegado));

        var resultados = _presupuestosRepository.GetPresupuesto(idPresupuesto);
        return View("Index", resultados);
    }

    public IActionResult AgregarProducto(int idPresupuesto)
    {
        var productos = _productosRepository.GetProductos(); // Obtiene todos los productos
        var model = new AgregarProductoViewModel
        {
            IdPresupuesto = idPresupuesto,
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var producto = _productosRepository.GetProducto(model.IdProducto);
        // Lógica para agregar el producto al presupuesto
        _presupuestosRepository.AddProduct(model.IdPresupuesto, producto, model.Cantidad);

        return RedirectToAction("Details", new { id = model.IdPresupuesto });
    }
}
