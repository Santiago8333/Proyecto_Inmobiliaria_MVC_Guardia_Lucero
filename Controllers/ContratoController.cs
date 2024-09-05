using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;
    private RepositorioContrato repo;

 public ContratoController(ILogger<ContratoController> logger)
    {
        _logger = logger;
        repo = new RepositorioContrato();

    }

public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
{
    var contratosQueryable = repo.ObtenerTodos().AsQueryable();
    var paginacion = await Paginacion<Contrato>.CrearPaginacion(contratosQueryable, pageNumber, pageSize);

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

}