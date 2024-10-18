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
      private RepositorioContrato repo3;
 public InmueblesController(ILogger<InmueblesController> logger)
    {
        _logger = logger;
        repo = new RepositorioInmuebles();
        repo2 = new RepositorioPropietario();
        repo3 = new RepositorioContrato();
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
public async Task<IActionResult> Index(DateTime? FechaInicio, DateTime? FechaFin,int pageNumber = 1, int pageSize = 5, string estadoFiltro = "Todos",string EmailPropietario = "Todos")
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
// Filtrar inmuebles que estén disponibles en el rango de fechas si ambas fechas son válidas
    if (FechaInicio.HasValue && FechaFin.HasValue)
    {
        // Obtener todos los contratos que se solapen con las fechas seleccionadas
        var contratosOcupados = repo3.ObtenerTodos().Where(c =>
            c.Fecha_desde <= FechaFin && c.Fecha_hasta >= FechaInicio
        ).Select(c => c.Id_inmueble).ToList();
        /*
    foreach (var idInmueble in contratosOcupados)
        {
            Console.WriteLine("Inmueble ocupado ID: " + idInmueble);
        }
        */
        //Console.WriteLine($"Contratos ocupados encontrados: {contratosOcupados.Count}");
        // Filtrar los inmuebles que no estén ocupados en ese rango de fechas
        //Console.WriteLine($"Inmuebles antes del filtrado: {inmueblesQueryable.Count()}");
        inmueblesQueryable = inmueblesQueryable.Where(i => !contratosOcupados.Contains(i.Id_inmueble));
        //Console.WriteLine($"Inmuebles disponibles después del filtrado: {inmueblesQueryable.Count()}");
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