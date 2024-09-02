using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioInmuebles{
    string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";

    public List<Inmuebles> ObtenerTodos()
	{
        List<Inmuebles> inmuebles = new List<Inmuebles>();
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
            var query = $@"SELECT {nameof(Inmuebles.Id_inmueble)},{nameof(Inmuebles.Uso)},{nameof(Inmuebles.Tipo)},{nameof(Inmuebles.Ambiente)},{nameof(Inmuebles.Precio)}
                      FROM inmuebles WHERE Estado = true";
        using(MySqlCommand command = new MySqlCommand(query, connection))
			{
                connection.Open();
				var reader = command.ExecuteReader();
                while (reader.Read())
				{
					inmuebles.Add(new Inmuebles
					{
						Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                        Uso = reader.GetString(nameof(Inmuebles.Uso)),
                        Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                        Ambiente = reader.GetString(nameof(Inmuebles.Ambiente)),
                        Precio = reader.GetInt32(nameof(Inmuebles.Precio))
					});
				}
                connection.Close();
            }
            return inmuebles;
        }
    }
}