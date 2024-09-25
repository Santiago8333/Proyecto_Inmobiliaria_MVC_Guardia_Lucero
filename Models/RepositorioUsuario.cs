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
public void AgregarUsuario(Usuario nuevoUsuario)
{
     using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"INSERT INTO usuario ({nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)}, {nameof(Usuario.RolNombre)},{nameof(Usuario.Estado)})
                    VALUES (@Nombre, @Apellido, @Email,@Clave,@Rol, @RolNombre, @Estado)";
         using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            
            command.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", nuevoUsuario.Apellido);
            command.Parameters.AddWithValue("@Email", nuevoUsuario.Email);
            command.Parameters.AddWithValue("@Clave", nuevoUsuario.Clave);
            command.Parameters.AddWithValue("@Rol", nuevoUsuario.Rol);
            command.Parameters.AddWithValue("@RolNombre", nuevoUsuario.RolNombre);
            command.Parameters.AddWithValue("@Estado", true); 

            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void EliminarUsuario(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE usuario 
                       SET {nameof(Usuario.Estado)} = @Estado
                       WHERE Id_usuario = @Id AND Estado = 1";
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
public Usuario? ObtenerPorID(int id)
{
    Usuario? res = null;
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT Id_usuario,Apellido, Nombre, Email, Clave,AvatarFile,Rol,RolNombre,Estado
                      FROM usuario 
                      WHERE Id_usuario = @Id AND Estado = 1";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
        using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Usuario
                    {
                        Id_usuario = reader.GetInt32(nameof(Usuario.Id_usuario)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        RolNombre = reader.GetString(nameof(Usuario.RolNombre)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Usuario.Estado)))
                    };
                }
            }
        }
    }
    return res;
}
public void ActualizarUsuario(Usuario actualizarUsuario)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE usuario
                    SET 
                        {nameof(Usuario.Nombre)} = @Nombre, 
                        {nameof(Usuario.Apellido)} = @Apellido, 
                        {nameof(Usuario.Email)} = @Email, 
                        {nameof(Usuario.Clave)} = @Clave, 
                        {nameof(Usuario.Rol)} = @Rol, 
                        {nameof(Usuario.RolNombre)} = @RolNombre
                    WHERE Id_usuario = @Id AND Estado = true";
         using(MySqlCommand command = new MySqlCommand(query, connection))
        {

            command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
            command.Parameters.AddWithValue("@Nombre", actualizarUsuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", actualizarUsuario.Apellido);
            command.Parameters.AddWithValue("@Email", actualizarUsuario.Email);
            command.Parameters.AddWithValue("@Clave", actualizarUsuario.Clave);
            command.Parameters.AddWithValue("@Rol", actualizarUsuario.Rol);
            command.Parameters.AddWithValue("@RolNombre", actualizarUsuario.RolNombre);

            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }
}
public void ActualizarUsuarioPerfil(Usuario actualizarUsuario)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE usuario
                    SET 
                        {nameof(Usuario.Nombre)} = @Nombre, 
                        {nameof(Usuario.Apellido)} = @Apellido, 
                        {nameof(Usuario.Email)} = @Email
                    WHERE Id_usuario = @Id AND Estado = true";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {

            command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
            command.Parameters.AddWithValue("@Nombre", actualizarUsuario.Nombre);
            command.Parameters.AddWithValue("@Apellido", actualizarUsuario.Apellido);
            command.Parameters.AddWithValue("@Email", actualizarUsuario.Email);

            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }
}

public void ActualizarUsuarioClave(Usuario actualizarUsuario)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE usuario
                    SET 
                        {nameof(Usuario.Clave)} = @Clave
                    WHERE Id_usuario = @Id AND Estado = true";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {

            command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
            command.Parameters.AddWithValue("@Clave", actualizarUsuario.Clave);

            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }
}
}