using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioPropietario 
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";


    public List<Propietario> ObtenerTodos()
	{
		List<Propietario> propietarios = new List<Propietario>();
		using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
			var query = $@"SELECT {nameof(Propietario.Id_propietarios)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)} 
                      FROM propietarios WHERE Estado = true";
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
                    Telefono = reader.GetString(nameof(Propietario.Telefono))
					});
				}
				connection.Close();
			}
			return propietarios;
		}
	}
public void AgregarPropietario(Propietario nuevoPropietario)
{
	
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"INSERT INTO propietarios ({nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Estado)})
                    VALUES (@Dni, @Apellido, @Nombre,@Email,@Telefono, @Estado)";
                       
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega los parámetros
			command.Parameters.AddWithValue("@Dni", nuevoPropietario.Dni);
            command.Parameters.AddWithValue("@Apellido", nuevoPropietario.Apellido);
            command.Parameters.AddWithValue("@Nombre", nuevoPropietario.Nombre);
            command.Parameters.AddWithValue("@Email", nuevoPropietario.Email);
			command.Parameters.AddWithValue("@Telefono", nuevoPropietario.Telefono);
            command.Parameters.AddWithValue("@Estado", true); 

            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void EliminarPropietario(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
		//var query = "DELETE FROM propietarios WHERE Id_propietarios = @Id";
        var query = $@"UPDATE propietarios 
                       SET {nameof(Propietario.Estado)} = @Estado
                       WHERE Id_propietarios = @Id";

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

public Propietario? ObtenerPorID(int id)
{
    Propietario? res = null;

    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_propietarios, Dni, Apellido, Nombre, Email, Telefono,Estado
                      FROM propietarios 
                      WHERE Id_propietarios = @Id AND Estado = true";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Propietario
                    {
                        Id_propietarios = reader.GetInt32(nameof(Propietario.Id_propietarios)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Propietario.Estado)))
                    };
                }
            }
        }
    }

    return res; // Retorna el propietario o null si no se encontró
}
public void ActualizarPropietario(Propietario actualizarPropietario)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE propietarios 
               SET 
                   {nameof(Propietario.Dni)} = @Dni, 
                   {nameof(Propietario.Apellido)} = @Apellido, 
                   {nameof(Propietario.Nombre)} = @Nombre, 
                   {nameof(Propietario.Email)} = @Email, 
                   {nameof(Propietario.Telefono)} = @Telefono
               WHERE Id_propietarios = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega los parámetros
			command.Parameters.AddWithValue("@Dni", actualizarPropietario.Dni);
            command.Parameters.AddWithValue("@Apellido", actualizarPropietario.Apellido);
            command.Parameters.AddWithValue("@Nombre", actualizarPropietario.Nombre);
            command.Parameters.AddWithValue("@Email", actualizarPropietario.Email);
			command.Parameters.AddWithValue("@Telefono", actualizarPropietario.Telefono);
            command.Parameters.AddWithValue("@Id", actualizarPropietario.Id_propietarios);

            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }

}
}