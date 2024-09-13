using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;


namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class UsuarioController : Controller
{
private readonly ILogger<UsuarioController> _logger;
private RepositorioUsuario repo;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        repo = new RepositorioUsuario();
   
    }
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
{
    var UsuarioQueryable = repo.ObtenerTodos().AsQueryable();
    var paginacion = await Paginacion<Usuario>.CrearPaginacion(UsuarioQueryable, pageNumber, pageSize);

    return View(paginacion);
}
/*
public IActionResult Index()
{
return View();
}
*/
public IActionResult Login()
{
return View();
}
/*
public IActionResult Register()
{
return View();
}
*/
}