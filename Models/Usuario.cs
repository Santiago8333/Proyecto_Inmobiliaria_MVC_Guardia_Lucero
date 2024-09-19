namespace Proyecto_Inmobiliaria_MVC.Models;
public enum enRoles
	{
		Administrador = 1,
		Empleado = 2,
	}
public class Usuario{
public int Id_usuario { get; set; }
public string Nombre { get; set; } = "";
public string Apellido { get; set; } = "";
public string Email { get; set; } = "";
public string Clave { get; set; } = "";
public string Avatar { get; set; } = "";
public int Rol { get; set; }
public string RolNombre { get; set; } = "";
public bool Estado {get;set;} = false;


}