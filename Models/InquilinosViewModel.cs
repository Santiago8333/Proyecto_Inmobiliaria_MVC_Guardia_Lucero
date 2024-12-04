using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;
public class InquilinosViewModel
{
    public Paginacion<Inquilinos>? InquilinosPaginados { get; set; }
    public IEnumerable<Inquilinos>? Inquilinos { get; set; }
    public string EstadoFiltro { get; set; } = "";
    public string EmailInquilinosFiltro { get; set; } = "";
}