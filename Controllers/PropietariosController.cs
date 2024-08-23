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
        TempData["Mensaje"] = "Propietario agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al agregar el Propietario.";
    var lista = repo.ObtenerTodos();
    return View("Index", lista);
}

public IActionResult Eliminar(int id)
{
  var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            TempData["Mensaje"] = "Propietario no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarPropietario(id);
        TempData["Mensaje"] = "Propietario eliminado.";
        return RedirectToAction("Index");
    
}

public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Propietario no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            TempData["Mensaje"] = "Propietario no encontrado.";
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
        TempData["Mensaje"] = "Propietario Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Propietario.";
return RedirectToAction("Index");
}
}
