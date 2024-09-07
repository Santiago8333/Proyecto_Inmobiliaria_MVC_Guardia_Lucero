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
  var contrato = repo.ObtenerPorID(id);
        if (contrato == null)
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
public async Task<IActionResult> Pago(int id, int pageNumber = 1, int pageSize = 5)
{
    // Verifica si el contrato es válido o no
    if(id == 0)
    {
        TempData["Mensaje"] = "Contrato no válido.";
        return RedirectToAction("Index");
    }
    //Verificar si el contrato existe
      var contrato = repo.ObtenerPorID(id);
    if (contrato == null)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }
    var pagos = repo.ObtenerPagoDelContrato(id);

    // Si no hay pagos, permitimos que la vista se muestre vacía
    if (pagos == null || !pagos.Any())
    {
        ViewBag.Mensaje = "No hay pagos registrados para este contrato.";
        pagos = new List<Pago>();  // Inicializa una lista vacía de pagos
    }
     // Calcular el total
     var primerPago = pagos.FirstOrDefault();
    decimal? monto = primerPago?.MontoTotalApagar;
    int? Id_contrato = primerPago?.Id_contrato;
    // Realiza la paginación, aunque la lista esté vacía
    var pagosQueryable = pagos.AsQueryable();
    var paginacion = await Paginacion<Pago>.CrearPaginacion(pagosQueryable, pageNumber, pageSize);
    ViewBag.totalApagarMonto = monto; 
    ViewBag.Id_contrato = Id_contrato;
    return View(paginacion);
}
[HttpPost]
public IActionResult AgregarPago(Pago nuevoPago)
{
    if (!ModelState.IsValid)
    {
        TempData["Mensaje"] = "Hubo un error al agregar el Pago al Contrato.";
        return RedirectToAction("Index");
    }

    // Obtener el contrato correspondiente
    var contrato = repo.ObtenerPorID(nuevoPago.Id_contrato);
    
    if (contrato == null)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }
    //veirificar pago
    if (nuevoPago.Monto > contrato.Monto_Pagar || nuevoPago.Monto <= 0)
    {
        TempData["Mensaje"] = "Monto ingersado del pago Incorrecto.";
        return RedirectToAction("Index");
    }
    // Actualizar el monto a pagar del contrato
    var nuevoMontoPagar = contrato.Monto_Pagar - nuevoPago.Monto;
    contrato.Monto_Pagar = nuevoMontoPagar;
    
    // Actualizar el contrato en la base de datos
    repo.ActualizarContratoMontoPagar(contrato);

    // Agregar el nuevo pago
    repo.AgregarPago(nuevoPago);

    TempData["Mensaje"] = "Pago agregado al Contrato exitosamente.";
    return RedirectToAction("Index");
}
}