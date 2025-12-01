using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_dan1dlm.Models;
using tl2_tp8_2025_dan1dlm.Interfaces;
using tl2_tp8_2025_dan1dlm.ViewModels;
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




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AccesoDenegado()
    {
        return View();
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
}
