namespace Proyecto_Inmobiliaria_MVC.Models;

public class Contrato{
     public int Id_contrato {get;set;}
     public int Id_inquilino {get;set;}
     public int Id_inmueble {get;set;}
     public string Emailinquilino {get;set;} = "";
     public string EmailPropietario {get;set;} = "";
     public string Inmuebletipo {get;set;} = "";
     public string Inmuebledireccion {get;set;} = "";
     public decimal Monto {get;set;}
     public decimal Monto_total {get;set;}
     public DateTime Fecha_desde {get;set;}
     public DateTime Fecha_hasta {get;set;}
     public DateTime FechaTerminacion {get;set;}
     public DateTime? FechaTerminacionAnticipada { get; set; }
     public decimal Monto_Pagar {get;set;}
     public int Meses {get;set;}
     public bool Estado {get;set;} = false;
}