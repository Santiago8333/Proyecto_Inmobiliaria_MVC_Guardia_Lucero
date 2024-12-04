using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;
public class PropietariosViewModel
{
    public Paginacion<Propietario>? PropietariosPaginados { get; set; }
    public IEnumerable<Propietario>? Propietarios { get; set; }
    public string EstadoFiltro { get; set; } = "";
    public string EmailPropietarioFiltro { get; set; } = "";

}
