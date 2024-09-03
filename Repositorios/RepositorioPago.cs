using MySql.Data.MySqlClient;

namespace PROYECTO_BRUNO_SOAZO.Models;

public class RepositorioPago
{
    readonly string ConnectionString = "Server=localhost;Database=proyecto-bruno-soazo;User=root;Password=;";

    public RepositorioPago()
    {

    }

    public IList<Pago> GetPagos()
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Numero)}, {nameof(Pago.Fecha)}, {nameof(Pago.Referencia)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdContrato)}
                FROM pagos p";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Numero = reader.GetInt32(nameof(Pago.Numero)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Referencia = reader.GetString(nameof(Pago.Referencia)),
                            Importe = reader.GetDouble(nameof(Pago.Importe)),
                            Anulado = reader.GetString(nameof(Pago.Anulado)),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                        });
                    }

                }
            }
        }
        return pagos;
    }

    public int AltaPago(Pago pago)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"INSERT INTO pagos ( {nameof(Pago.Numero)}, {nameof(Pago.Fecha)}, {nameof(Pago.Referencia)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdContrato)}) 
                        VALUES (@{nameof(Pago.Numero)}, @{nameof(Pago.Fecha)}, @{nameof(Pago.Referencia)}, @{nameof(Pago.Importe)}, @{nameof(Pago.Anulado)}, @{nameof(Pago.IdContrato)});           
                        SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {

                command.Parameters.AddWithValue($"@{nameof(Pago.Numero)}", pago.Numero);
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Referencia)}", pago.Referencia);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Anulado)}", pago.Anulado);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdContrato)}", pago.IdContrato);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                pago.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Pago? GetPago(int id)
    {
        Pago? pagos = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Numero)}, {nameof(Pago.Fecha)}, {nameof(Pago.Referencia)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdContrato)}
                FROM pagos p
                WHERE p.{nameof(Pago.Id)} = @{nameof(Pago.Id)};";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pagos = new Pago
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Pago.Id))),
                            Numero = reader.GetInt32(reader.GetOrdinal(nameof(Pago.Numero))),
                            Fecha = reader.GetDateTime(reader.GetOrdinal(nameof(Pago.Fecha))),
                            Referencia = reader.GetString(reader.GetOrdinal(nameof(Pago.Referencia))),
                            Importe = reader.GetDouble(reader.GetOrdinal(nameof(Pago.Importe))),
                            Anulado = reader.GetString(reader.GetOrdinal(nameof(Pago.Anulado))),
                            IdContrato = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdContrato))),
                        };
                    }
                }
            }
        }
        return pagos;
    }

    public int ModificarPago(Pago pago)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE pagos SET 
                            {nameof(Pago.Numero)} = @{nameof(Pago.Numero)}, 
                            {nameof(Pago.Fecha)} = @{nameof(Pago.Fecha)},
                            {nameof(Pago.Referencia)} = @{nameof(Pago.Referencia)},
                            {nameof(Pago.Importe)} = @{nameof(Pago.Importe)},
                            {nameof(Pago.Anulado)} = @{nameof(Pago.Anulado)},
                            {nameof(Pago.IdContrato)} = @{nameof(Pago.IdContrato)}
                        WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Numero)}", pago.Numero);
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Referencia)}", pago.Referencia);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Anulado)}", pago.Anulado);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdContrato)}", pago.IdContrato);
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", pago.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }


    public int EliminarPago(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from pagos WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

}