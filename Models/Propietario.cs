namespace Proyecto_Inmobiliaria_MVC.Models;

public class Propietario
{
    public int Id_propietarios {get;set;}
    public string Dni {get;set;} = "";

    public string Apellido {get;set;} = "";

    public string Nombre {get;set;} = "";

    public string Email {get;set;} = "";

    public string Telefono {get;set;} = "";

    public bool Estado {get;set;} = false;

}