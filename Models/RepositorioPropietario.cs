using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioPropietario
{
    string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";

    public List<Propietario> ObtenerTodos()
	{
		List<Propietario> propietarios = new List<Propietario>();
		using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
			var query = $@"SELECT {nameof(Propietario.Id_propietarios)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)} 
                      FROM propietarios";
			using(MySqlCommand command = new MySqlCommand(query, connection))
			{
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					propietarios.Add(new Propietario
					{
						Id_propietarios = reader.GetInt32(nameof(Propietario.Id_propietarios)),
                    Dni = reader.GetString(nameof(Propietario.Dni)),
                    Apellido = reader.GetString(nameof(Propietario.Apellido)),
                    Nombre = reader.GetString(nameof(Propietario.Nombre)),
                    Email = reader.GetString(nameof(Propietario.Email)),
                    Telefono = reader.GetInt32(nameof(Propietario.Telefono))
					});
				}
				connection.Close();
			}
			return propietarios;
		}
	}
}