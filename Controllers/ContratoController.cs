using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    private RepositorioContrato repo;
    private RepositorioInquilinos repo2;
    private RepositorioInmuebles repo3;

 public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
        repo = new RepositorioContrato();
        repo2 = new RepositorioInquilinos();
        repo3 = new RepositorioInmuebles();
    }

public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
{
    var contratosQueryable = repo.ObtenerTodos().AsQueryable();
    var paginacion = await Paginacion<Contrato>.CrearPaginacion(contratosQueryable, pageNumber, pageSize);
    var inquilinos = repo2.ObtenerTodos();
    ViewBag.Inquilinos = inquilinos;
    var inmuebles = repo3.ObtenerTodos();
    ViewBag.Inmuebles = inmuebles;
    return View(paginacion);
}
public IActionResult Detalle(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var contrato = repo.ObtenerPorID(id);
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        
        return View(contrato);
    }

}
public IActionResult Eliminar(int id)
{
  var propietario = repo.ObtenerPorID(id);
        if (propietario == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarContrato(id);
        TempData["Mensaje"] = "Contrato eliminado.";
        return RedirectToAction("Index");
    
}
[HttpPost]
public IActionResult Agregar(Contrato nuevoContrato)
{
        if (ModelState.IsValid)
    {
        
        repo.AgregarContrato(nuevoContrato);
        TempData["Mensaje"] = "Contrato agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al agregar el Contrato.";
    return RedirectToAction("Index");
}
public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var contrato = repo.ObtenerPorID(id);
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
            var inquilinos = repo2.ObtenerTodos();
            ViewBag.Inquilinos = inquilinos;
            var inmuebles = repo3.ObtenerTodos();
            ViewBag.Inmuebles = inmuebles;
        return View(contrato);
    }

}
[HttpPost]
public IActionResult Actualizar(Contrato actualizarContrato)
{
if (ModelState.IsValid)
    {
        repo.ActualizarContrato(actualizarContrato);
        TempData["Mensaje"] = "Contrato Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Contrato.";
return RedirectToAction("Index");
}
}