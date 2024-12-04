using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioInquilinos
{
    string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";
    public List<Inquilinos> ObtenerTodos()
	{
		List<Inquilinos> inquilinos = new List<Inquilinos>();
		using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
			var query = $@"SELECT {nameof(Inquilinos.Id_inquilinos)}, {nameof(Inquilinos.Dni)}, {nameof(Inquilinos.Apellido)}, {nameof(Inquilinos.Nombre)}, {nameof(Inquilinos.Email)}, {nameof(Inquilinos.Telefono)},{nameof(Propietario.Estado)}
                      FROM inquilinos WHERE 1";
			using(MySqlCommand command = new MySqlCommand(query, connection))
			{
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					inquilinos.Add(new Inquilinos
					{
					Id_inquilinos = reader.GetInt32(nameof(Inquilinos.Id_inquilinos)),
                    Dni = reader.GetString(nameof(Inquilinos.Dni)),
                    Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                    Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                    Email = reader.GetString(nameof(Inquilinos.Email)),
                    Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                    Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilinos.Estado)))
					});
				}
				connection.Close();
			}
			return inquilinos;
		}
	}
public void AgregarPropietario(Inquilinos nuevoInquilino)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
    var query = $@"INSERT INTO inquilinos ({nameof(Inquilinos.Dni)}, {nameof(Inquilinos.Apellido)}, {nameof(Inquilinos.Nombre)}, {nameof(Inquilinos.Email)}, {nameof(Inquilinos.Telefono)},{nameof(Inquilinos.Estado)})
                 VALUES (@Dni, @Apellido, @Nombre,@Email,@Telefono, @Estado)";

        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega los parámetros
			command.Parameters.AddWithValue("@Dni", nuevoInquilino.Dni);
            command.Parameters.AddWithValue("@Apellido", nuevoInquilino.Apellido);
            command.Parameters.AddWithValue("@Nombre", nuevoInquilino.Nombre);
            command.Parameters.AddWithValue("@Email", nuevoInquilino.Email);
            command.Parameters.AddWithValue("@Telefono", nuevoInquilino.Telefono);
            command.Parameters.AddWithValue("@Estado", true); 
            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void EliminarInquilino(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE inquilinos 
                       SET {nameof(Inquilinos.Estado)} = @Estado
                       WHERE Id_inquilinos = @Id";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@estado", false);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
public Inquilinos? ObtenerPorID(int id)
{
    Inquilinos? res = null;
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_inquilinos, Dni, Apellido, Nombre, Email, Telefono , Estado
                      FROM inquilinos 
                      WHERE Id_inquilinos = @Id AND Estado = true";
    using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Inquilinos
                    {
                        Id_inquilinos = reader.GetInt32(nameof(Inquilinos.Id_inquilinos)),
                        Dni = reader.GetString(nameof(Inquilinos.Dni)),
                        Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                        Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                        Email = reader.GetString(nameof(Inquilinos.Email)),
                        Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilinos.Estado)))
                    };
                }
            }
        }
    }

return res; // Retorna el propietario o null si no se encontró
}
public void ActualizarInquilinos(Inquilinos actualizarInquilinos)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE inquilinos
               SET 
                   {nameof(Propietario.Dni)} = @Dni, 
                   {nameof(Propietario.Apellido)} = @Apellido, 
                   {nameof(Propietario.Nombre)} = @Nombre, 
                   {nameof(Propietario.Email)} = @Email, 
                   {nameof(Propietario.Telefono)} = @Telefono
               WHERE Id_inquilinos = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega los parámetros
			command.Parameters.AddWithValue("@Dni", actualizarInquilinos.Dni);
            command.Parameters.AddWithValue("@Apellido", actualizarInquilinos.Apellido);
            command.Parameters.AddWithValue("@Nombre", actualizarInquilinos.Nombre);
            command.Parameters.AddWithValue("@Email", actualizarInquilinos.Email);
			command.Parameters.AddWithValue("@Telefono", actualizarInquilinos.Telefono);
            command.Parameters.AddWithValue("@Id", actualizarInquilinos.Id_inquilinos);

            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }

}
public Inquilinos? ObtenerPorEmail(string Email)
{
    Inquilinos? res = null;
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_inquilinos, Dni, Apellido, Nombre, Email, Telefono , Estado
                      FROM inquilinos 
                      WHERE Email = @Email";
    using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Email", Email);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Inquilinos
                    {
                        Id_inquilinos = reader.GetInt32(nameof(Inquilinos.Id_inquilinos)),
                        Dni = reader.GetString(nameof(Inquilinos.Dni)),
                        Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                        Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                        Email = reader.GetString(nameof(Inquilinos.Email)),
                        Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilinos.Estado)))
                    };
                }
            }
        }
    }

return res; // Retorna el propietario o null si no se encontró
}
public void ActivarInquilino(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE inquilinos 
                       SET {nameof(Inquilinos.Estado)} = @Estado
                       WHERE Id_inquilinos = @Id";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@estado", true);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
public Inquilinos? ObtenerPorID2(int id)
{
    Inquilinos? res = null;
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_inquilinos, Dni, Apellido, Nombre, Email, Telefono , Estado
                      FROM inquilinos 
                      WHERE Id_inquilinos = @Id AND Estado = false";
    using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Inquilinos
                    {
                        Id_inquilinos = reader.GetInt32(nameof(Inquilinos.Id_inquilinos)),
                        Dni = reader.GetString(nameof(Inquilinos.Dni)),
                        Apellido = reader.GetString(nameof(Inquilinos.Apellido)),
                        Nombre = reader.GetString(nameof(Inquilinos.Nombre)),
                        Email = reader.GetString(nameof(Inquilinos.Email)),
                        Telefono = reader.GetString(nameof(Inquilinos.Telefono)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilinos.Estado)))
                    };
                }
            }
        }
    }

return res; // Retorna el propietario o null si no se encontró
}
}