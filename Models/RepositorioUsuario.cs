using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioUsuario
{
    private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";

    public List<Usuario> ObtenerTodos()
	{
		List<Usuario> usuarios = new List<Usuario>();
        using(MySqlConnection connection = new MySqlConnection(ConectionString))
		{
            var query = $@"SELECT {nameof(Usuario.Id_usuario)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, {nameof(Usuario.RolNombre)}, {nameof(Usuario.Estado)}
                      FROM usuario";

        using(MySqlCommand command = new MySqlCommand(query, connection))
			{
                connection.Open();
				var reader = command.ExecuteReader();
                while (reader.Read())
				{
					usuarios.Add(new Usuario
					{
					Id_usuario = reader.GetInt32(nameof(Usuario.Id_usuario)),
                    Nombre = reader.GetString(nameof(Usuario.Nombre)),
                    Apellido = reader.GetString(nameof(Usuario.Apellido)),
                    Email = reader.GetString(nameof(Usuario.Email)),
                    RolNombre = reader.GetString(nameof(Usuario.RolNombre)),
                    Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Usuario.Estado)))
					});
				}
				connection.Close();
            }
            return usuarios;
        }
    }
public async Task<Usuario?> ObtenerPorEmailAsync(string email)
{
    Usuario? res = null;

    using (var connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_usuario, Nombre, Apellido, Email, RolNombre, Clave
                      FROM usuario
                      WHERE Email = @Email AND Estado = true";

        using (var command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro Email
            command.Parameters.AddWithValue("@Email", email);

            await connection.OpenAsync(); // Abre la conexión de manera asincrónica

            using (var reader = await command.ExecuteReaderAsync()) // Ejecuta el lector asincrónicamente
            {
                if (await reader.ReadAsync()) // Lee de forma asincrónica
                {
                    res = new Usuario
                    {
                        Id_usuario = reader.GetInt32(reader.GetOrdinal("Id_usuario")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        RolNombre = reader.GetString(reader.GetOrdinal("RolNombre")),
                        Clave = reader.GetString(reader.GetOrdinal("Clave")) 
                    };
                }
            }
        }
    }

    return res; // Retorna el usuario o null si no se encontró
}

}