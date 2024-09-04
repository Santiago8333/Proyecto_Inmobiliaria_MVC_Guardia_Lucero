namespace Proyecto_Inmobiliaria_MVC.Models;

public class Contrato{
     public int Id_contrato {get;set;}
     public int Id_inquilino {get;set;}
     public int Id_inmueble {get;set;}
     public string Emailinquilino {get;set;} = "";
     public string Inmuebleuso {get;set;} = "";
     public decimal Monto {get;set;}
     public DateTime Fecha_desde {get;set;}
     public DateTime Fecha_hasta {get;set;}
     public bool Estado {get;set;} = false;
}