namespace Proyecto_Inmobiliaria_MVC.Models;
public class Multa{
public int Id_multa {get;set;}
public int Id_contrato {get;set;}
public decimal Monto {get;set;}
public string RazonMulta { get; set; } = "";
public DateTime Fecha {get;set;}
}