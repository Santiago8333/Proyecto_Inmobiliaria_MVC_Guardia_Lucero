using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class PropietariosController : Controller
{
    private readonly ILogger<PropietariosController> _logger;
    private RepositorioPropietario repo;
    public PropietariosController(ILogger<PropietariosController> logger)
    {
        _logger = logger;
        repo = new RepositorioPropietario();
    }

    public IActionResult Index()
    {
        var lista = repo.ObtenerTodos();
        ViewBag.TotalPropietarios = lista.Count();
        return View(lista);
    }

[HttpPost]
public IActionResult Agregar(Propietario nuevoPropietario)
{
    if (ModelState.IsValid)
    {
        // Aquí agregas el nuevo propietario a la base de datos
        repo.AgregarPropietario(nuevoPropietario);

        // Rediriges de nuevo al Index después de agregar
        return RedirectToAction("Index");
    }

    // Si hay un error, recargas la lista de propietarios y vuelves a la vista Index
    var lista = repo.ObtenerTodos();
    return View("Index", lista);
}

public IActionResult Eliminar(int id)
{
    try
    {
        repo.EliminarPropietario(id);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        
        return View("Error", new { message = ex.Message });
    }
}
/*
public IActionResult Edicion(int id)
{
if(id == 0)
    return View();
else
    
}
*/
}
