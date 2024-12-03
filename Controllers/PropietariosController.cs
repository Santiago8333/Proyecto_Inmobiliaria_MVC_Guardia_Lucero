using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;


namespace Proyecto_Inmobiliaria_MVC.Controllers;
[Authorize]
public class PropietariosController : Controller
{

    
    private readonly ILogger<PropietariosController> _logger;
    private RepositorioPropietario repo;
    public PropietariosController(ILogger<PropietariosController> logger)
    {
        _logger = logger;
        repo = new RepositorioPropietario();
        
    }
    
/*
    public IActionResult Index()
    {
        var lista = repo.ObtenerTodos();
        ViewBag.TotalPropietarios = lista.Count();
        return View(lista);
    }
*/

public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5,string EmailPropietario = "Todos")
{
    var propietariosQueryable = repo.ObtenerTodos().AsQueryable();
    var listaPropietarios = propietariosQueryable;
    //aplicar filtro por email
    if (EmailPropietario != "Todos")
    {
        propietariosQueryable = propietariosQueryable.Where(i => i.Email == EmailPropietario);
        TempData["Mensaje"] = "Propietario Buscado: "+EmailPropietario;
    }


    var paginacion = await Paginacion<Propietario>.CrearPaginacion(propietariosQueryable, pageNumber, pageSize);

    var viewModel = new PropietariosViewModel
    {
        PropietariosPaginados = paginacion,
        Propietarios = listaPropietarios
    };

    return View(viewModel);
}
[HttpPost]
public IActionResult Agregar(Propietario nuevoPropietario)
{
    if (ModelState.IsValid)
    {
        var Propietarios = repo.ObtenerPorEmail(nuevoPropietario.Email);
    if (Propietarios != null)
        {
            TempData["Mensaje"] = "Este Propietario ya esta registrado.";
            return RedirectToAction("Index");
        }
        repo.AgregarPropietario(nuevoPropietario);
        TempData["Mensaje"] = "Propietario agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al agregar el Propietario.";
    var lista = repo.ObtenerTodos();
    return RedirectToAction("Index");
    //return View("Index", lista);
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
public IActionResult Detalle(int id)
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
public IActionResult Activar(int id)
{
    var Propietario = repo.ObtenerPorID2(id);
    if (Propietario == null)
        {
            TempData["Mensaje"] = "Propietario no encontrado.";
            return RedirectToAction("Index");
        }
        repo.ActivarPropietarios(id);
        TempData["Mensaje"] = "Propietario Activado.";
        return RedirectToAction("Index");
}
}
