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
                c.Monto_total,
                c.Fecha_desde,
                c.Fecha_hasta,
                c.FechaTerminacion,
                inq.Email AS Emailinquilino,
                i.Tipo AS Inmuebletipo,
                pro.Email AS EmailPropietario
            FROM
                contrato c
            JOIN inmuebles i ON i.Id_inmueble = c.Id_inmueble
            JOIN inquilinos inq ON inq.Id_inquilinos = c.Id_inquilino
            JOIN propietarios pro ON pro.Id_propietarios = i.Id_propietario
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
                    Monto = reader.GetDecimal("Monto"),
                    Monto_total = reader.GetDecimal("Monto_total"),
                    Fecha_desde = reader.GetDateTime("Fecha_desde"),
                    Fecha_hasta = reader.GetDateTime("Fecha_hasta"),
                    FechaTerminacion = reader.GetDateTime("FechaTerminacion"),
                    Emailinquilino = reader.GetString("Emailinquilino"),
                    Inmuebletipo = reader.GetString("Inmuebletipo"),
                    EmailPropietario = reader.GetString("EmailPropietario"),
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
                            c.Monto_total,
                            c.Fecha_desde,
                            c.Fecha_hasta,
                            c.Monto_Pagar,
                            c.FechaTerminacion,
                            c.Meses,
                            inq.Email AS Emailinquilino,
                            i.Tipo AS Inmuebletipo,
                            c.Estado,
                            pro.Email AS EmailPropietario,
                            i.Direccion AS Inmuebledireccion
                        FROM
                            contrato c
                        JOIN inmuebles i ON i.Id_inmueble = c.Id_inmueble
                        JOIN inquilinos inq ON inq.Id_inquilinos = c.Id_inquilino
                        JOIN propietarios pro ON pro.Id_propietarios = i.Id_propietario
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
                    Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                    Monto_total = reader.GetDecimal(nameof(Contrato.Monto_total)),
                    Fecha_desde = reader.GetDateTime(nameof(Contrato.Fecha_desde)),
                    Fecha_hasta = reader.GetDateTime(nameof(Contrato.Fecha_hasta)),
                    FechaTerminacion = reader.GetDateTime(nameof(Contrato.FechaTerminacion)),
                    Emailinquilino = reader.GetString(nameof(Contrato.Emailinquilino)),
                    Inmuebletipo = reader.GetString(nameof(Contrato.Inmuebletipo)),
                    Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.Estado))),
                    EmailPropietario = reader.GetString(nameof(Contrato.EmailPropietario)),
                    Inmuebledireccion = reader.GetString(nameof(Contrato.Inmuebledireccion)),
                    Monto_Pagar = reader.GetDecimal(nameof(Contrato.Monto_Pagar)),
                    Meses = reader.GetInt32(nameof(Contrato.Meses))
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
        var query = $@"INSERT INTO contrato ({nameof(Contrato.Id_inquilino)},{nameof(Contrato.Id_inmueble)},{nameof(Contrato.Monto)},{nameof(Contrato.Monto_total)},{nameof(Contrato.Fecha_desde)},{nameof(Contrato.Fecha_hasta)},{nameof(Contrato.FechaTerminacion)},{nameof(Contrato.Monto_Pagar)},{nameof(Contrato.Meses)},{nameof(Contrato.Estado)})
                    VALUES (@Id_inquilino, @Id_inmueble, @Monto,@Monto_total,@Fecha_desde,@Fecha_hasta,@FechaTerminacion,@Monto_Pagar,@Meses,@Estado)";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id_inquilino", nuevoContrato.Id_inquilino);
            command.Parameters.AddWithValue("@Id_inmueble", nuevoContrato.Id_inmueble);
            command.Parameters.AddWithValue("@Monto", nuevoContrato.Monto);
            command.Parameters.AddWithValue("@Monto_total", nuevoContrato.Monto_total);
            command.Parameters.AddWithValue("@Fecha_desde", nuevoContrato.Fecha_desde);
            command.Parameters.AddWithValue("@Fecha_hasta", nuevoContrato.Fecha_hasta);
            command.Parameters.AddWithValue("@FechaTerminacion", nuevoContrato.FechaTerminacion);
            command.Parameters.AddWithValue("@Monto_Pagar", nuevoContrato.Monto_total);
            command.Parameters.AddWithValue("@Meses", nuevoContrato.Meses);
            command.Parameters.AddWithValue("@Estado", true);
            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void ActualizarContrato(Contrato actualizarContrato)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE contrato 
               SET 
                   {nameof(Contrato.Id_inquilino)} = @Id_inquilino, 
                   {nameof(Contrato.Id_inmueble)} = @Id_inmueble, 
                   {nameof(Contrato.Monto)} = @Monto,
                   {nameof(Contrato.Fecha_desde)} = @Fecha_desde,
                   {nameof(Contrato.Fecha_hasta)} = @Fecha_hasta
               WHERE Id_contrato = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id_inquilino", actualizarContrato.Id_inquilino);
            command.Parameters.AddWithValue("@Id_inmueble", actualizarContrato.Id_inmueble);
            command.Parameters.AddWithValue("@Monto", actualizarContrato.Monto);
            command.Parameters.AddWithValue("@Fecha_desde", actualizarContrato.Fecha_desde);
            command.Parameters.AddWithValue("@Fecha_hasta", actualizarContrato.Fecha_hasta);
            command.Parameters.AddWithValue("@Id", actualizarContrato.Id_contrato);
            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }

    }

}
public List<Pago> ObtenerPagoDelContrato(int id)
{

List<Pago> pagos = new List<Pago>();
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT
                        p.Id_pago,
                        p.Id_contrato,
                        p.Fecha_pago,
                        p.Monto,
                        p.Estado,
                        p.Detalle,
                        c.Monto AS MontoTotalApagar
                    FROM
                        pago p
                    JOIN contrato c ON
                        c.Id_contrato = p.Id_contrato
                    WHERE
                        c.Id_contrato = @Id";
        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
            pagos.Add(new Pago
                {
                     Id_pago = reader.GetInt32("Id_pago"),
                     Id_contrato = reader.GetInt32("Id_contrato"),
                     Fecha_pago = reader.GetDateTime("Fecha_pago"),
                     Detalle = reader.GetString("Detalle"),
                     Monto = reader.GetDecimal("Monto"),
                     Estado = reader.GetBoolean("Estado"),
                     MontoTotalApagar = reader.GetDecimal("MontoTotalApagar")
                });
                
            }
            connection.Close();
        }
    }
    
    return pagos.Any() ? pagos : pagos = new List<Pago>();;
}
public List<Pago> ObtenerPagoActivosDelContrato(int id)
{
List<Pago> pagos = new List<Pago>();
using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT
                        p.Id_pago,
                        p.Id_contrato,
                        p.Fecha_pago,
                        p.Monto,
                        p.Estado,
                        p.Detalle,
                        c.Monto AS MontoTotalApagar
                    FROM
                        pago p
                    JOIN contrato c ON
                        c.Id_contrato = p.Id_contrato
                    WHERE
                        c.Id_contrato = @Id AND p.Estado = true";
        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
            pagos.Add(new Pago
                {
                     Id_pago = reader.GetInt32("Id_pago"),
                     Id_contrato = reader.GetInt32("Id_contrato"),
                     Fecha_pago = reader.GetDateTime("Fecha_pago"),
                     Detalle = reader.GetString("Detalle"),
                     Monto = reader.GetDecimal("Monto"),
                     Estado = reader.GetBoolean("Estado"),
                     MontoTotalApagar = reader.GetDecimal("MontoTotalApagar")
                });
                
            }
            connection.Close();
        }
    }
    
    return pagos.Any() ? pagos : pagos = new List<Pago>();;
}
public void AgregarPago(Pago nuevoPago)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
    var query = $@"INSERT INTO pago ({nameof(Pago.Id_contrato)},{nameof(Pago.Detalle)},{nameof(Pago.Fecha_pago)},{nameof(Pago.Monto)},{nameof(Pago.Estado)})
                VALUES (@Id_contrato,@Detalle, @Fecha_pago, @Monto,@Estado)";
     using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id_contrato", nuevoPago.Id_contrato);
            command.Parameters.AddWithValue("@Detalle", nuevoPago.Detalle);
            command.Parameters.AddWithValue("@Fecha_pago", nuevoPago.Fecha_pago);
            command.Parameters.AddWithValue("@Monto", nuevoPago.Monto);
            command.Parameters.AddWithValue("@Estado", true);
            connection.Open();
            command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
            connection.Close();
        }
    }
}
public void ActualizarContratoMontoPagar(Contrato contrato)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = $@"UPDATE contrato 
               SET 
                   {nameof(Contrato.Monto_Pagar)} = @Monto_Pagar
               WHERE Id_contrato = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", contrato.Id_contrato);
            command.Parameters.AddWithValue("@Monto_Pagar", contrato.Monto_Pagar);
            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }

    }
}

public void EliminarPago(int id)
{
using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {        var query = $@"UPDATE pago 
                       SET {nameof(Pago.Estado)} = @Estado
                       WHERE Id_pago = @Id";
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
public Pago? ObtenerPagoPorID(int id)
{
    Pago? res = null;
  using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
         var query = @"SELECT
                        p.Id_pago,
                        p.Id_contrato,
                        p.Fecha_pago,
                        p.Monto,
                        p.Estado
                    FROM
                        pago p
                    WHERE
                        p.Id_pago = @Id AND p.Estado = TRUE";
     using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agrega el parámetro id
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Pago
                    {
                    Id_pago = reader.GetInt32(nameof(Pago.Id_pago)),
                    Id_contrato = reader.GetInt32(nameof(Pago.Id_contrato)),
                    Fecha_pago = reader.GetDateTime(nameof(Pago.Fecha_pago)),
                    Monto = reader.GetInt32(nameof(Pago.Monto)),
                    Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.Estado)))

                    };
                }
            }
        }
    }
     return res; 
}
public void ActualizarPago(Pago actualizarPago)
{
    using(MySqlConnection connection = new MySqlConnection(ConectionString))
    {
       var query = $@"UPDATE pago SET
                   {nameof(Pago.Fecha_pago)} = @Fecha_pago, 
                   {nameof(Pago.Monto)} = @Monto
               WHERE Id_pago = @Id AND Estado = true";
 using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Fecha_pago", actualizarPago.Fecha_pago);
            command.Parameters.AddWithValue("@Monto", actualizarPago.Monto);
            command.Parameters.AddWithValue("@Id", actualizarPago.Id_pago);
            connection.Open();
            command.ExecuteNonQuery(); 
            connection.Close();
        }
    }
}
public Contrato? ObtenerContratoPorFecha(int id, DateTime FechaDesde, DateTime FechaHasta)
{
    Contrato? res = null;
    
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"SELECT * 
                      FROM contrato 
                      WHERE Id_inmueble = @Id
                      AND Estado = 1
                      AND ((Fecha_desde <= @Fecha_hasta AND Fecha_hasta >= @Fecha_desde));";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            // Agregar parámetros
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Fecha_desde", FechaDesde);
            command.Parameters.AddWithValue("@Fecha_hasta", FechaHasta);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    res = new Contrato
                    {
                        Id_contrato = reader.GetInt32(nameof(Contrato.Id_contrato)),
                        Fecha_desde = reader.GetDateTime(nameof(Contrato.Fecha_desde)),
                        Fecha_hasta = reader.GetDateTime(nameof(Contrato.Fecha_hasta)),
                        Monto = reader.GetInt32(nameof(Contrato.Monto)),
                        Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Contrato.Estado)))
                        // Agrega más propiedades si es necesario
                    };
                }
            }
        }
    }

    return res;
}
public List<Pago> ObtenerTodosPagosAnulados()
{
    List<Pago> contratos = new List<Pago>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"
            SELECT * FROM pago WHERE Estado = 0";
        
        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Pago
                {
                    Id_contrato = reader.GetInt32("Id_contrato"),
                    Id_pago = reader.GetInt32("Id_pago"),
                    Fecha_pago = reader.GetDateTime("Fecha_pago"),
                    Monto = reader.GetDecimal("Monto"),
                    Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))

                });
            }
            connection.Close();
        }
        return contratos;
    }
}

}