using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioContrato{
    string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";

        public List<Contrato> ObtenerTodos()
	{
         List<Contrato> contratos = new List<Contrato>();
         using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
            var query = $@"SELECT
                            {nameof(Contrato.Id_contrato)},
                            {nameof(Contrato.Id_inmueble)},
                            {nameof(Contrato.Id_inquilino)},
                            {nameof(Contrato.Monto)},
                            {nameof(Contrato.Fecha_desde)},
                            {nameof(Contrato.Fecha_hasta)},
                            inq.Email AS Emailinquilino,
                            i.Uso AS Inmuebleuso
                        FROM
                            contrato c
                        JOIN inmuebles i ON i.Id_inmueble = c.Id_inmueble
                        JOIN inquilinos inq ON inq.Id_inquilinos = c.Id_inquilino
                        WHERE c.Estado = true";
            using(MySqlCommand command = new MySqlCommand(query, connection))
			{
                connection.Open();
				var reader = command.ExecuteReader();
                while (reader.Read())
				{
					contratos.Add(new Contrato
					{
						Id_contrato = reader.GetInt32(nameof(Contrato.Id_contrato)),
                        Id_inmueble= reader.GetInt32(nameof(Contrato.Id_inmueble)),
                        Id_inquilino= reader.GetInt32(nameof(Contrato.Id_inquilino)),
                        Monto= reader.GetInt32(nameof(Contrato.Monto)),
                        Fecha_desde = reader.GetDateTime(nameof(Contrato.Fecha_desde)), 
                        Fecha_hasta = reader.GetDateTime(nameof(Contrato.Fecha_hasta)),
                        Emailinquilino = reader.GetString("Emailinquilino"),
                        Inmuebleuso = reader.GetString("Inmuebleuso"),
					});

				}
                connection.Close();
            }
             return contratos;
        }
    }
}