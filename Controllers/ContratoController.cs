using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Contracts;

namespace Proyecto_Inmobiliaria_MVC.Controllers;
[Authorize]
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
/*
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
*/
//--
public IActionResult TerminarContrato(int id)
{
    DateTime fechaTerminacion = DateTime.Now;
    var contrato = repo.ObtenerPorID(id);
    if (contrato == null)
    {
        TempData["Mensaje"] = "Contrato no encontrado.";
        return RedirectToAction("Index");
    }
    if (contrato.Estado == false)
    {
        TempData["Mensaje"] = "No se puede Canselar un Contrato ya canselado.";
        return RedirectToAction("Index");
    }

    // Verificar si debe meses
    int mesesAdeudados = ObtenerMesesAdeudados(contrato);
    if (mesesAdeudados > 0)
    {
        TempData["Mensaje"] = $"El inquilino debe {mesesAdeudados} meses de alquiler.";
        return RedirectToAction("Detalles", new { id = contrato.Id_contrato });
    }

    // Calcular multa
    decimal multa = CalcularMulta(contrato, fechaTerminacion);
    var duracionTotal = (contrato.Fecha_hasta - contrato.Fecha_desde).TotalDays;
    var duracionCumplida = (fechaTerminacion - contrato.Fecha_desde).TotalDays;
    var razon = "";

     if (duracionCumplida < duracionTotal / 2)
    {
         razon = "Multa de 2 Meses,No se Cumplio menos de la mitad del tiempo de la duracion del contrato";
        
    }
    else
    {
         razon = "Multa de 2 Meses No se Cumplio la duracion del contrato";
       
    }
    // Registrar la terminación anticipada
    contrato.FechaTerminacionAnticipada = fechaTerminacion;
    repo.ActualizarContratoFechaTerminacion(contrato);

    // Registrar la multa como un pago
    RegistrarMulta(contrato.Id_contrato, multa,razon);
    TempData["Mensaje"] = $"Contrato terminado anticipadamente. Multa registrada: {multa:C}.";

    
    return RedirectToAction("Detalles", new { id = contrato.Id_contrato });
}
public int ObtenerMesesAdeudados(Contrato contrato)
{
    var fechaActual = DateTime.Now;
    int mesesAlquiler = ((fechaActual.Year - contrato.Fecha_desde.Year) * 12) + fechaActual.Month - contrato.Fecha_desde.Month;

    var pagosRealizados = repo.ObtenerPagoDelContrato(contrato.Id_contrato);
    int mesesPagados = pagosRealizados.Count;

    return mesesAlquiler - mesesPagados;
}
public decimal CalcularMulta(Contrato contrato, DateTime fechaTerminacion)
{
    // Calcula la duración original del contrato
    var duracionTotal = (contrato.Fecha_hasta - contrato.Fecha_desde).TotalDays;
    var duracionCumplida = (fechaTerminacion - contrato.Fecha_desde).TotalDays;
    var pagos = repo.ObtenerPagoDelContrato(contrato.Id_contrato);
    //suma de de los pagos
    var sumpagos = pagos.Where(p => p.Estado == true).Sum(p => p.Monto);
    // Verifica si se cumplió menos de la mitad del tiempo
    if (duracionCumplida < duracionTotal / 2)
    {
        // Multa de 2 meses extra
        //actualizar contrato
        contrato.Meses += 2; 
        contrato.Monto_total = contrato.Monto * contrato.Meses;
        contrato.Monto_Pagar = contrato.Monto_total - sumpagos;
        repo.ActualizarContratoMulta(contrato);
        return contrato.Monto * 2;
    }
    else
    {
        // Multa de 1 mes extra
        //actualizar contrato
        contrato.Meses += 1; 
        contrato.Monto_total = contrato.Monto * contrato.Meses;
        contrato.Monto_Pagar = contrato.Monto_total - sumpagos;
        repo.ActualizarContratoMulta(contrato);
        return contrato.Monto * 1;
    }
}
public void RegistrarMulta(int contratoId, decimal montoMulta, string razon)
{
    var multa = new Multa
    {
        Id_contrato = contratoId,
        Monto = montoMulta,
        RazonMulta = razon,
        Fecha = DateTime.Now,
        
    };
    repo.AgregarMulta(multa);
}
//--
[HttpPost]
public IActionResult Agregar(Contrato nuevoContrato)
{
        if (ModelState.IsValid)
    {
        //verificar si hay un contrato ya en ese rango de fecha
        var contrato = repo.ObtenerContratoPorFecha(nuevoContrato.Id_inmueble,nuevoContrato.Fecha_desde,nuevoContrato.Fecha_hasta);
        if(contrato == null){
            nuevoContrato.Monto_Pagar = nuevoContrato.Monto;
            nuevoContrato.FechaTerminacion = nuevoContrato.Fecha_hasta;
            repo.AgregarContrato(nuevoContrato);
        TempData["Mensaje"] = "Contrato agregado exitosamente.";
        return RedirectToAction("Index");
        }else{

            TempData["Mensaje"] = "Contrato ya exsiste un contrato en ese rango de fechas.";
        return RedirectToAction("Index");
        }
        
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
        //verificar si el contrato existe
        var contrato = repo.ObtenerPorID(actualizarContrato.Id_contrato);
        if(contrato == null){
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        var pagos = repo.ObtenerPagoDelContrato(actualizarContrato.Id_contrato);
        var pagosA = repo.ObtenerPagoActivosDelContrato(actualizarContrato.Id_contrato);
        // Verificar si alguno de los pagos tiene una fecha fuera del rango permitido
        DateTime fechaInicioContrato = actualizarContrato.Fecha_desde;
        DateTime fechaFinContrato = actualizarContrato.Fecha_hasta;

        foreach (var pago in pagosA)
        {
            if (pago.Estado == true && (pago.Fecha_pago < fechaInicioContrato || pago.Fecha_pago > fechaFinContrato))
            {
                TempData["Mensaje"] = "No se puede modificar el contrato. Existen pagos con fecha fuera del rango permitido.";
                return RedirectToAction("Index");
            }
        }

        var sumpagos = pagos.Where(p => p.Estado == true).Sum(p => p.Monto);
        // Verificar si el nuevo Monto_total es menor que el actual y también menor que la suma de los pagos realizados
        if (actualizarContrato.Monto_total < contrato.Monto_total)
        {
            if (actualizarContrato.Monto_total < sumpagos)
            {
                TempData["Mensaje"] = "No se puede modificar el contrato. El Monto_total ingresado es menor que los pagos ya realizados.";
                return RedirectToAction("Index");
            }
        }
        

        actualizarContrato.Monto_Pagar = actualizarContrato.Monto - sumpagos;
        repo.ActualizarContratoMontoPagar(actualizarContrato);
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
    //suma de de los pagos
    var sumpagos = pagos.Where(p => p.Estado == true).Sum(p => p.Monto);
    //caclular monto que falta pagar
    var MontoQueFaltaPagar =  contrato.Monto_total - sumpagos;
    // Si no hay pagos, permitimos que la vista se muestre vacía
    if (pagos == null || !pagos.Any())
    {
        ViewBag.Mensaje = "No hay pagos registrados para este contrato.";
        pagos = new List<Pago>();  // Inicializa una lista vacía de pagos
    }
    //obtener lista de multas
    var multas = repo.ObtenerMultasContrato(contrato.Id_contrato);

    // Realiza la paginación, aunque la lista esté vacía
    var pagosQueryable = pagos.AsQueryable();
    var paginacion = await Paginacion<Pago>.CrearPaginacion(pagosQueryable, pageNumber, pageSize); 
    DateTime fechaInicio = contrato.Fecha_desde; 
    ViewBag.sumpagos = sumpagos;
    ViewBag.Id_contrato = contrato.Id_contrato;
    ViewBag.TotalAPagar = contrato.Monto_total;
    ViewBag.MontoTotal = contrato.Monto;
    ViewBag.MontoQueFaltaPagar = MontoQueFaltaPagar;
    ViewBag.Meses = contrato.Meses;
    ViewBag.fechaInicio = fechaInicio;
    ViewBag.Multas = multas;
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
    // Verificación del rango de fechas
    if (nuevoPago.Fecha_pago < contrato.Fecha_desde || nuevoPago.Fecha_pago > contrato.Fecha_hasta)
    {
        TempData["Mensaje"] = "La fecha del pago está fuera del rango permitido por el contrato.";
        return RedirectToAction("Index");
    }
    // Actualizar el monto a pagar del contrato
    var nuevoMontoPagar = contrato.Monto_Pagar - nuevoPago.Monto;
    contrato.Monto_Pagar = nuevoMontoPagar;
    
    // Actualizar el contrato en la base de datos
    repo.ActualizarContratoMontoPagar(contrato);
    // Agregar el nuevo pago
    repo.AgregarPago(nuevoPago);
    //veririficar si se pago todo el contrato
    var pagosActivos = repo.ObtenerPagoActivosDelContrato(contrato.Id_contrato);
    // Total de meses del contrato
    var totalMeses = contrato.Meses;
    var mesesPagados = pagosActivos.Count();
    var mesesApagar = totalMeses - mesesPagados;
if (mesesApagar <= 0)
{
    // Todos los meses han sido pagados
    contrato.Contrato_Completado = true;
    repo.ActualizarContratoCompletado(contrato);
    TempData["Mensaje"] = "El contrato ya está completamente pagado.";
    return RedirectToAction("Index");
}
else
{
    // Mostrar la cantidad de meses restantes por pagar
    TempData["Mensaje"] = $"Faltan {mesesApagar} meses por pagar en el contrato.";
}
    TempData["Mensaje"] = "Pago agregado al Contrato exitosamente.";
    return RedirectToAction("Index");
}
public IActionResult EliminarPago(int id)
{
  var pago = repo.ObtenerPagoPorID(id);
        if (pago == null)
        {
            TempData["Mensaje"] = "Pago no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarPago(id);
        //obvtener contrato
        var contrato = repo.ObtenerPorID(pago.Id_contrato);
        if(contrato == null){
             TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        // Actualizar el monto a pagar del contrato
        var nuevoMontoPagar = contrato.Monto_Pagar + pago.Monto;
        contrato.Monto_Pagar = nuevoMontoPagar;
        
        // Actualizar el contrato en la base de datos
        repo.ActualizarContratoMontoPagar(contrato);
        //Actualizar Contrato_Completado
        //veririficar si se pago todo el contrato
        var pagosActivos = repo.ObtenerPagoActivosDelContrato(contrato.Id_contrato);
        // Total de meses del contrato
        var totalMeses = contrato.Meses;
        var mesesPagados = pagosActivos.Count();
        var mesesApagar = totalMeses - mesesPagados;
if (mesesApagar > 0)
{
    contrato.Contrato_Completado = false;
    repo.ActualizarContratoCompletado(contrato);
}
        TempData["Mensaje"] = "Pago eliminado.";
        return RedirectToAction("Index");
}
public IActionResult PagoEdicion(int id)
{
    if (id == 0)
    {
        TempData["Mensaje"] = "Pago no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
          var pago = repo.ObtenerPagoPorID(id);
        if (pago == null)
        {
            TempData["Mensaje"] = "Pago no encontrado.";
            return RedirectToAction("Index");
        }

        return View(pago);
    }
}
[HttpPost]
public IActionResult ActualizarPago(Pago actualizarPago)
{
if (ModelState.IsValid)
    {
        var pago = repo.ObtenerPagoPorID(actualizarPago.Id_pago);
        var pagos = repo.ObtenerPagoDelContrato(actualizarPago.Id_contrato);
        var contrato = repo.ObtenerPorID(actualizarPago.Id_contrato);
        var sumpagos = pagos.Sum(p => p.Monto);
        //verificar si el pago exsiste
        if (pago == null)
        {
            TempData["Mensaje"] = "Pago no encontrado.";
            return RedirectToAction("Index");
        }
        //verificar si el contrato exsiste 
        if (contrato == null)
        {
            TempData["Mensaje"] = "Contrato no encontrado.";
            return RedirectToAction("Index");
        }
        //veirificar pago
        // Verificar monto de pago (El monto que queda por pagar)
        var realmonto = contrato.Monto_Pagar;
        var realmonto2 = actualizarPago.Monto - pago.Monto;
        var realmonto3 = Math.Abs(realmonto2);
        if(realmonto2 < 0){
            realmonto = contrato.Monto;
        }
        if (realmonto3 > realmonto)
        {
          
            TempData["Mensaje"] = $"El monto ingresado ({actualizarPago.Monto}) excede lo que falta por pagar ({contrato.Monto_Pagar}).";
            return RedirectToAction("Index");
        }
        if (actualizarPago.Monto <= 0)
        {
            TempData["Mensaje"] = "El monto del pago debe ser mayor a cero.";
            return RedirectToAction("Index");
        }
        
        // Verificación del rango de fechas
        if (actualizarPago.Fecha_pago < contrato.Fecha_desde || actualizarPago.Fecha_pago > contrato.Fecha_hasta)
        {
            TempData["Mensaje"] = "La fecha del pago está fuera del rango permitido por el contrato.";
            return RedirectToAction("Index");
        }
       // Calcular la diferencia entre el monto actual y el monto a actualizar
        var diferenciaMonto = actualizarPago.Monto - pago.Monto;

        // Si la diferencia es positiva (el nuevo pago es mayor), restamos esa diferencia al monto total del contrato
        // Si la diferencia es negativa (el nuevo pago es menor), sumamos la diferencia al monto total del contrato
        contrato.Monto_Pagar -= diferenciaMonto;

        // Actualizar el contrato en la base de datos
        repo.ActualizarContratoMontoPagar(contrato);


        repo.ActualizarPago(actualizarPago);
        TempData["Mensaje"] = "Pago Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Pago.";
return RedirectToAction("Index");
}

}