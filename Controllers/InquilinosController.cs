using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class InquilinosController : Controller
{
    private readonly ILogger<InquilinosController> _logger;
    private RepositorioInquilinos repo;
    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
        repo = new RepositorioInquilinos();
    }
    public IActionResult Index()
    {
        var lista = repo.ObtenerTodos();
        return View(lista);
    }
[HttpPost]
public IActionResult Agregar(Inquilinos nuevoInquilino)
{
if (ModelState.IsValid)
    {
         repo.AgregarPropietario(nuevoInquilino);
         TempData["Mensaje"] = "Inquilino agregado exitosamente.";
         return RedirectToAction("Index");
    }
    var lista = repo.ObtenerTodos();
    TempData["Mensaje"] = "Hubo un error al agregar el Inquilino.";
    return View("Index", lista);

}

public IActionResult Eliminar(int id)
{
    var inquilinos = repo.ObtenerPorID(id);
        if (inquilinos == null)
        {
             TempData["Mensaje"] = "Inquilino no encontrado.";
            return RedirectToAction("Index");
        }
    repo.EliminarInquilino(id);
    TempData["Mensaje"] = "Inquilino Eliminado exitosamente.";
return RedirectToAction("Index");
}
public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Inquilino no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var inquilinos = repo.ObtenerPorID(id);
        if (inquilinos == null)
        {
            TempData["Mensaje"] = "Inquilino no encontrado.";
            return RedirectToAction("Index");
        }
        return View(inquilinos);
    }

}
[HttpPost]
public IActionResult Actualizar(Inquilinos actualizarInquilinos)
{
if (ModelState.IsValid)
    {
        repo.ActualizarInquilinos(actualizarInquilinos);
        TempData["Mensaje"] = "Inquilino actualizado exitosamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "error al actualizar Inquilino.";
return RedirectToAction("Index");
}
}