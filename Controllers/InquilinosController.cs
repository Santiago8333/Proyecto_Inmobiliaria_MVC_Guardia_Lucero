using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Inmobiliaria_MVC.Controllers;
[Authorize]
public class InquilinosController : Controller
{
    private readonly ILogger<InquilinosController> _logger;
    private RepositorioInquilinos repo;
    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
        repo = new RepositorioInquilinos();
    }
    /*
public IActionResult Index()
{
    var lista = repo.ObtenerTodos();
        return View(lista);
}
*/
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5,string EmailInquilinos = "Todos")
{
    var inquilinosQueryable = repo.ObtenerTodos().AsQueryable();
    var listaInquilinos = inquilinosQueryable;
    //aplicar filtro por email
    if (EmailInquilinos != "Todos")
    {
        inquilinosQueryable = inquilinosQueryable.Where(i => i.Email == EmailInquilinos);
        TempData["Mensaje"] = "Inquilino Buscado: "+EmailInquilinos;
    }

    var paginacion = await Paginacion<Inquilinos>.CrearPaginacion(inquilinosQueryable, pageNumber, pageSize);

    var viewModel = new InquilinosViewModel
    {
        InquilinosPaginados = paginacion,
        Inquilinos = listaInquilinos
    };

    return View(viewModel);
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
    //return View("Index", lista);
    return RedirectToAction("Index");

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
public IActionResult Detalle(int id)
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