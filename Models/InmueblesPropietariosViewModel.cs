using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;
public class InmueblesPropietariosViewModel
{
    public Paginacion<Inmuebles>? InmueblesPaginados { get; set; }
    public IEnumerable<Propietario>? Propietarios { get; set; }
    public string EstadoFiltro { get; set; } = "";
}
