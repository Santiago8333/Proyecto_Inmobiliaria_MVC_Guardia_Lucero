using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class InmueblesController : Controller
{
      private readonly ILogger<InmueblesController> _logger;
      private RepositorioInmuebles repo;
      private RepositorioPropietario repo2;
 public InmueblesController(ILogger<InmueblesController> logger)
    {
        _logger = logger;
        repo = new RepositorioInmuebles();
        repo2 = new RepositorioPropietario();
    }
 public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var inmueblesQueryable = repo.ObtenerTodos().AsQueryable();
        var paginacion = await Paginacion<Inmuebles>.CrearPaginacion(inmueblesQueryable, pageNumber, pageSize);
        var propietarios = repo2.ObtenerTodos();

        var viewModel = new InmueblesPropietariosViewModel
        {
            InmueblesPaginados = paginacion,
            Propietarios = propietarios
        };

        return View(viewModel);
    }
}