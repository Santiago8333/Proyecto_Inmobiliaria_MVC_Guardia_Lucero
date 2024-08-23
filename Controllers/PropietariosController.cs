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
        
        repo.AgregarPropietario(nuevoPropietario);

        
        return RedirectToAction("Index");
    }

    
    var lista = repo.ObtenerTodos();
    return View("Index", lista);
}

public IActionResult Eliminar(int id)
{
  var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            return RedirectToAction("Index");
        }
        repo.EliminarPropietario(id);
        return RedirectToAction("Index");
    
}

public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        return RedirectToAction("Index");
    }
    else
    {
        var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            return RedirectToAction("Index");
        }
        return View(propietario);
    }

}
[HttpPost]
public IActionResult Actualizar(Propietario actualizarPropietario)
{
if (ModelState.IsValid)
    {
        repo.ActualizarPropietario(actualizarPropietario);
        
        return RedirectToAction("Index");
    }

return RedirectToAction("Index");
}
}
