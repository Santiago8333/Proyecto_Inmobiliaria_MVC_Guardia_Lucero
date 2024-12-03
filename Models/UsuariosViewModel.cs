using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;
public class UsuariosViewModel
{
    public Paginacion<Usuario>? UsuariosPaginados { get; set; }
    public IEnumerable<Usuario>? Usuarios { get; set; }

}
