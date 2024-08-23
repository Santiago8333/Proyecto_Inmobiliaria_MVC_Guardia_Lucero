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
         return RedirectToAction("Index");
    }
    var lista = repo.ObtenerTodos();
    return View("Index", lista);

}

public IActionResult Eliminar(int id)
{
    var inquilinos = repo.ObtenerPorID(id);
        if (inquilinos == null)
        {
            return RedirectToAction("Index");
        }
    repo.EliminarInquilino(id);
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
        var inquilinos = repo.ObtenerPorID(id);
        if (inquilinos == null)
        {
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
        
        return RedirectToAction("Index");
    }

return RedirectToAction("Index");
}
}