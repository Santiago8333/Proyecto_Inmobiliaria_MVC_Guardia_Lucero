using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Inmobiliaria_MVC.Controllers;
[Authorize]
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
    /*
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
    */
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, string estadoFiltro = "Todos",string EmailPropietario = "Todos")
{
    var inmueblesQueryable = repo.ObtenerTodos().AsQueryable();

    // Aplicar el filtro según el estado
    if (estadoFiltro == "Activos")
    {
        inmueblesQueryable = inmueblesQueryable.Where(i => i.Estado == true);
    }
    else if (estadoFiltro == "Suspendidos")
    {
        inmueblesQueryable = inmueblesQueryable.Where(i => i.Estado == false);
    }
    //aplicar filtro por email
    if (EmailPropietario != "Todos")
    {
        inmueblesQueryable = inmueblesQueryable.Where(i => i.EmailPropietario == EmailPropietario);
    }
    var paginacion = await Paginacion<Inmuebles>.CrearPaginacion(inmueblesQueryable, pageNumber, pageSize);
    var propietarios = repo2.ObtenerTodos();
    
    var viewModel = new InmueblesPropietariosViewModel
    {
        InmueblesPaginados = paginacion,
        Propietarios = propietarios,
        EstadoFiltro = estadoFiltro,
        EmailPropietarioFiltro = EmailPropietario
    };

    return View(viewModel);
}
[HttpPost]
public IActionResult Agregar(Inmuebles nuevoInmuebles)
{
if (ModelState.IsValid)
    {
        repo.AgregarInmuebles(nuevoInmuebles);
        TempData["Mensaje"] = "Inmueble agregado exitosamente.";
        return RedirectToAction("Index");
    }
    TempData["Mensaje"] = "Hubo un error al agregar el Inmueble.";
    return RedirectToAction("Index");
}
public IActionResult Suspender(int id)
{
    var Inmuebles = repo.ObtenerPorID(id);
    if (Inmuebles == null)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        repo.SuspenderInmuebles(id);
        TempData["Mensaje"] = "Inmueble Suspendido.";
        return RedirectToAction("Index");
}
public IActionResult Activar(int id)
{
    var Inmuebles = repo.ObtenerPorID2(id);
    if (Inmuebles == null)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        repo.ActivarInmuebles(id);
        TempData["Mensaje"] = "Inmueble Activado.";
        return RedirectToAction("Index");
}
public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Inmueble no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var inmuebles = repo.ObtenerPorID(id);
        if (inmuebles == null)
        {
            
            TempData["Mensaje"] = "Inmueble  no encontrado.";
            return RedirectToAction("Index");
        }
        var propietarios = repo2.ObtenerTodos();
        ViewBag.Propietarios = propietarios;
        return View(inmuebles);
    }

}
[HttpPost]
public IActionResult Actualizar(Inmuebles actualizarInmueble)
{
if (ModelState.IsValid)
    {
        //actualizar Monto en el contrato si tiene
        //repo.ActualizarContratoMonto(actualizarInmueble);
        //actualizar
        repo.ActualizarInmueble(actualizarInmueble);
        TempData["Mensaje"] = "Inmueble Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Inmueble.";
return RedirectToAction("Index");
}
public IActionResult Detalle(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Inmueble no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var Inmueble = repo.ObtenerPorID(id);
        if (Inmueble == null)
        {
            TempData["Mensaje"] = "Inmueble no encontrado.";
            return RedirectToAction("Index");
        }
        
        return View(Inmueble);
    }

}
}