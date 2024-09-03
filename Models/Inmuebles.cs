namespace Proyecto_Inmobiliaria_MVC.Models;

public class Inmuebles{
     public int Id_inmueble {get;set;}
     public int Id_propietario {get;set;}
     public string NombrePropietario { get; set; } = "";
     public string Uso {get;set;} = "";
     public string Tipo {get;set;} = "";
     public string Ambiente {get;set;} = "";
     public decimal Precio {get;set;}
     public string Direccion {get;set;} = "";
     public string Cordenada {get;set;} = "";
     public bool Estado {get;set;} = false;

}