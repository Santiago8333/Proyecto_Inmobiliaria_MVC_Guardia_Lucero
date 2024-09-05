using System.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Proyecto_Inmobiliaria_MVC.Models;

public class RepositorioContrato{
    string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_mvc_guardia_lucero;SslMode=none";

public List<Contrato> ObtenerTodos()
{
    List<Contrato> contratos = new List<Contrato>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"
            SELECT
                c.Id_contrato,
                c.Id_inmueble,
                c.Id_inquilino,
                c.Monto,
                c.Fecha_desde,
                c.Fecha_hasta,
                inq.Email AS Emailinquilino,
                i.Tipo AS Inmuebletipo
            FROM
                contrato c
            JOIN inmuebles i ON i.Id_inmueble = c.Id_inmueble
            JOIN inquilinos inq ON inq.Id_inquilinos = c.Id_inquilino
            WHERE c.Estado = true";
        
        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    Id_contrato = reader.GetInt32("Id_contrato"),
                    Id_inmueble = reader.GetInt32("Id_inmueble"),
                    Id_inquilino = reader.GetInt32("Id_inquilino"),
                    Monto = reader.GetInt32("Monto"),
                    Fecha_desde = reader.GetDateTime("Fecha_desde"),
                    Fecha_hasta = reader.GetDateTime("Fecha_hasta"),
                    Emailinquilino = reader.GetString("Emailinquilino"),
                    Inmuebletipo = reader.GetString("Inmuebletipo"),
                });
            }
            connection.Close();
        }
        return contratos;
    }
}
public Contrato? ObtenerPorID(int id)
{
    Contrato? res = null;

    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT
                            c.Id_contrato,
                            c.Id_inmueble,
                            c.Id_inquilino,
                            c.Monto,
                            c.Fecha_desde,
                            c.Fecha_hasta,
                            inq.Email AS Emailinquilino,
                            i.Tipo AS Inmuebletipo,
                            c.Estado
                        FROM
                            contrato c
                        JOIN inmuebles i ON i.Id_inmueble = c.Id_inmueble
                        JOIN inquilinos inq ON inq.Id_inquilinos = c.Id_inquilino
                      WHERE c.Id_contrato = @Id AND c.Estado = true";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Contrato
                    {
                    Id_contrato = reader.GetInt32(nameof(Contrato.Id_contrato)),
                    Id_inmueble = reader.GetInt32(nameof(Contrato.Id_inmueble)),
                    Id_inquilino = reader.GetInt32(nameof(Contrato.Id_inquilino)),
                    Monto = reader.GetInt32(nameof(Contrato.Monto)),
                    Fecha_desde = reader.GetDateTime(nameof(Contrato.Fecha_desde)),
                    Fecha_hasta = reader.GetDateTime(nameof(Contrato.Fecha_hasta)),
                    Emailinquilino = reader.GetString(nameof(Contrato.Emailinquilino)),
                    Inmuebletipo = reader.GetString(nameof(Contrato.Inmuebletipo)),
                    Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.Estado)))
                    };
                }
            }
        }
    }

    return res; 
}
public void EliminarContrato(int id)
{
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE contrato
                       SET {nameof(Contrato.Estado)} = @Estado
                       WHERE Id_contrato = @Id";
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
public void AgregarContrato(Contrato nuevoContrato)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"INSERT INTO contrato ({nameof(Contrato.Id_inquilino)},{nameof(Contrato.Id_inmueble)},{nameof(Contrato.Monto)},{nameof(Contrato.Fecha_desde)},{nameof(Contrato.Fecha_hasta)},{nameof(Contrato.Estado)})
                    VALUES (@Id_inquilino, @Id_inmueble, @Monto,@Fecha_desde,@Fecha_hasta, @Estado)";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id_inquilino", nuevoContrato.Id_inquilino);
            command.Parameters.AddWithValue("@Id_inmueble", nuevoContrato.Id_inmueble);
            command.Parameters.AddWithValue("@Monto", nuevoContrato.Monto);
            command.Parameters.AddWithValue("@Fecha_desde", nuevoContrato.Fecha_desde);
            command.Parameters.AddWithValue("@Fecha_hasta", nuevoContrato.Fecha_hasta);
            command.Parameters.AddWithValue("@Estado", true);
            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
}