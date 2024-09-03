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
            var query = $@"SELECT {nameof(Inmuebles.Id_inmueble)},{nameof(Inmuebles.Uso)},{nameof(Inmuebles.Tipo)},{nameof(Inmuebles.Ambiente)},{nameof(Inmuebles.Precio)},p.Nombre AS NombrePropietario
                      FROM inmuebles i
                      JOIN 
                        propietarios p ON p.Id_propietarios = i.Id_propietario
                      WHERE i.Estado = true";
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
                        Precio = reader.GetInt32(nameof(Inmuebles.Precio)),
                        NombrePropietario = reader.GetString("NombrePropietario")
					});
				}
                connection.Close();
            }
            return inmuebles;
        }
    }
public void AgregarInmuebles(Inmuebles nuevoInmuebles)
{
	
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"INSERT INTO inmuebles ({nameof(Inmuebles.Id_propietario)}, {nameof(Inmuebles.Uso)}, {nameof(Inmuebles.Tipo)}, {nameof(Inmuebles.Ambiente)}, {nameof(Inmuebles.Precio)}, {nameof(Inmuebles.Direccion)}, {nameof(Inmuebles.Cordenada)}, {nameof(Inmuebles.Estado)})
                    VALUES (@Id_propietario, @Uso, @Tipo,@Ambiente,@Precio, @Direccion,@Cordenada,@Estado)";
                       
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega los parámetros
			command.Parameters.AddWithValue("@Id_propietario", nuevoInmuebles.Id_propietario);
            command.Parameters.AddWithValue("@Uso", nuevoInmuebles.Uso);
            command.Parameters.AddWithValue("@Tipo", nuevoInmuebles.Tipo);
            command.Parameters.AddWithValue("@Ambiente", nuevoInmuebles.Ambiente);
            command.Parameters.AddWithValue("@Precio", nuevoInmuebles.Precio);
            command.Parameters.AddWithValue("@Direccion", nuevoInmuebles.Direccion);
            command.Parameters.AddWithValue("@Cordenada", nuevoInmuebles.Cordenada);
            command.Parameters.AddWithValue("@Estado", true);
            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void EliminarInmuebles(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
		
        var query = $@"UPDATE inmuebles 
                       SET {nameof(Inmuebles.Estado)} = @Estado
                       WHERE Id_inmueble = @Id";

 		using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Estado", false); 
			command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
	}
}
public Inmuebles? ObtenerPorID(int id)
{
    Inmuebles? res = null;

    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"SELECT
                        i.{nameof(Inmuebles.Id_inmueble)},
                        i.{nameof(Inmuebles.Id_propietario)},
                        i.{nameof(Inmuebles.Uso)},
                        i.{nameof(Inmuebles.Tipo)},
                        i.{nameof(Inmuebles.Ambiente)},
                        i.{nameof(Inmuebles.Precio)},
                        i.{nameof(Inmuebles.Direccion)},
                        i.{nameof(Inmuebles.Cordenada)},
                        i.{nameof(Inmuebles.Estado)},
                        p.Nombre AS NombrePropietario
                    FROM
                        inmuebles i
                    JOIN 
                        propietarios p ON p.Id_propietarios = i.Id_propietario
                    WHERE i.Id_inmueble = @Id AND i.Estado = true";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Inmuebles
                    {
                        Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                        Id_propietario = reader.GetInt32(nameof(Inmuebles.Id_propietario)),
                        Uso = reader.GetString(nameof(Inmuebles.Uso)),
                        Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                        Ambiente = reader.GetString(nameof(Inmuebles.Ambiente)),
                        Precio = reader.GetInt32(nameof(Inmuebles.Precio)),
                        Direccion = reader.GetString(nameof(Inmuebles.Direccion)),
                        Cordenada = reader.GetString(nameof(Inmuebles.Cordenada)),
                        Estado = reader.GetBoolean(nameof(Inmuebles.Estado)),
                        NombrePropietario = reader.GetString(nameof(Inmuebles.NombrePropietario))
                    };
                }
            }
        }
    }

    return res; // Retorna el propietario o null si no se encontró
}
public void ActualizarInmueble(Inmuebles actualizarInmueble)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
{
    var query = $@"UPDATE
                    inmuebles
                SET 
                    i.{nameof(Inmuebles.Id_propietario)} = @Id_propietario,
                    i.{nameof(Inmuebles.Uso)}= @Uso,
                    i.{nameof(Inmuebles.Tipo)}= @Tipo,
                    i.{nameof(Inmuebles.Ambiente)}= @Ambiente,
                    i.{nameof(Inmuebles.Precio)}= @Precio,
                    i.{nameof(Inmuebles.Direccion)}= @Direccion,
                    i.{nameof(Inmuebles.Cordenada)}= @Cordenada
                WHERE Id_inmueble = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
    {

        command.Parameters.AddWithValue("@Id_propietario", actualizarInmueble.Id_propietario);
        command.Parameters.AddWithValue("@Uso", actualizarInmueble.Uso);
        command.Parameters.AddWithValue("@Tipo", actualizarInmueble.Tipo);
        command.Parameters.AddWithValue("@Ambiente", actualizarInmueble.Ambiente);
        command.Parameters.AddWithValue("@Precio", actualizarInmueble.Precio);
        command.Parameters.AddWithValue("@Direccion", actualizarInmueble.Direccion);
        command.Parameters.AddWithValue("@Cordenada", actualizarInmueble.Cordenada);

        connection.Open();
        command.ExecuteNonQuery(); 
        connection.Close();
    }
}

}
}