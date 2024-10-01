namespace Proyecto_Inmobiliaria_MVC.Models;

public class Pago{
public int Id_pago {get;set;}
public int Id_contrato {get;set;}
public string Detalle {get;set;} = "";
public DateTime Fecha_pago {get;set;}
public decimal Monto {get;set;}
public decimal MontoTotalApagar {get;set;}
public bool Estado {get;set;} = false;
    
}