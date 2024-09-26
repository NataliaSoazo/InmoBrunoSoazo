using MySql.Data.MySqlClient;

namespace PROYECTO_BRUNO_SOAZO.Models;

public class RepositorioContrato
{
    readonly string ConnectionString = "Server=localhost;Database=proyecto-bruno-soazo;User=root;Password=;";

    public RepositorioContrato()
    {

    }

    public IList<Contrato> GetContratos()
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT i.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)}, {nameof(Contrato.Anulado)},
                      p.{nameof(Inquilino.Nombre)}, p.{nameof(Inquilino.Apellido)}, {nameof(Contrato.IdInmueble)}, m.{nameof(Inmueble.Direccion)}
                FROM Contratos i 
                INNER JOIN Inquilinos p ON i.{nameof(Contrato.IdInquilino)} = p.{nameof(Inquilino.Id)}
                INNER JOIN Inmuebles m ON i.{nameof(Contrato.IdInmueble)} = m.{nameof(Inmueble.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaTerm = reader.GetDateTime(nameof(Contrato.FechaTerm)),
                            MontoMensual = reader.GetDouble(nameof(Contrato.MontoMensual)),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),


                            Arrendatario = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),

                            },
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Datos = new Inmueble
                            {

                                Direccion = reader.GetString(nameof(Inmueble.Direccion))

                            },
                            Anulado = reader.GetBoolean(nameof(Contrato.Anulado))

                        });
                    }

                }
            }
        }
        return contratos;
    }

    public int AltaContrato(Contrato contrato)
    {
        int id = 0;
        try
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = $@"INSERT INTO contratos ({nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)}, {nameof(Contrato.IdInmueble)}, {nameof(Contrato.IdUsuarioComenzo)}, {nameof(Contrato.IdUsuarioTermino)}, {nameof(Contrato.Anulado)}) 
            VALUES (@{nameof(Contrato.FechaInicio)}, @{nameof(Contrato.FechaTerm)}, @{nameof(Contrato.MontoMensual)}, @{nameof(Contrato.IdInquilino)}, @{nameof(Contrato.IdInmueble)}, @{nameof(Contrato.IdUsuarioComenzo)}, null, false);           
            SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                    command.Parameters.AddWithValue($"@{nameof(Contrato.FechaTerm)}", contrato.FechaTerm);
                    command.Parameters.AddWithValue($"@{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                    command.Parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                    command.Parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
                    command.Parameters.AddWithValue($"@{nameof(Contrato.IdUsuarioComenzo)}", contrato.IdUsuarioComenzo);

                    connection.Open();
                    id = Convert.ToInt32(command.ExecuteScalar());
                    contrato.Id = id;
                }
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones: puedes registrar el error o volver a lanzarlo
            Console.WriteLine($"Error al insertar contrato: {ex.Message}");
            throw; // o manejarlo de otra manera
        }
        return id;
    }

    public Contrato? GetContrato(int id)
    {
        Contrato? contrato = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT i.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)},
                      p.{nameof(Inquilino.Nombre)}, p.{nameof(Inquilino.Apellido)}, {nameof(Contrato.IdInmueble)}, m.{nameof(Inmueble.Direccion)}, 
                      i.{nameof(Contrato.IdUsuarioComenzo)}, i.{nameof(Contrato.IdUsuarioTermino)}, i.{nameof(Contrato.Anulado)}
                FROM contratos i 
                INNER JOIN Inquilinos p ON i.{nameof(Contrato.IdInquilino)} = p.{nameof(Inquilino.Id)}
                INNER JOIN Inmuebles m ON i.{nameof(Contrato.IdInmueble)} = m.{nameof(Inmueble.Id)}
                WHERE i.{nameof(Contrato.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaTerm = reader.GetDateTime(nameof(Contrato.FechaTerm)),
                            MontoMensual = reader.GetDouble(nameof(Contrato.MontoMensual)),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Arrendatario = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),

                            },
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Datos = new Inmueble
                            {
                                Direccion = reader.GetString(nameof(Inmueble.Direccion))

                            },
                            IdUsuarioComenzo = reader.GetInt32(nameof(Contrato.IdUsuarioComenzo)),
                            IdUsuarioTermino = !reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.IdUsuarioTermino))) ?
                            reader.GetInt32(reader.GetOrdinal(nameof(Contrato.IdUsuarioTermino))) :
                            (int?)null,
                            Anulado = reader.GetBoolean(nameof(Contrato.Anulado))
                        };
                    }
                }
            }

        }
        return contrato;
    }

    public int ModificarContrato(Contrato contrato)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE contratos SET 
                    {nameof(Contrato.FechaInicio)} = @{nameof(Contrato.FechaInicio)},
                    {nameof(Contrato.FechaTerm)} = @{nameof(Contrato.FechaTerm)},
                    {nameof(Contrato.MontoMensual)} = @{nameof(Contrato.MontoMensual)},
                    {nameof(Contrato.IdInquilino)} = @{nameof(Contrato.IdInquilino)},
                    {nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)}
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id)}", contrato.Id);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaTerm)}", contrato.FechaTerm);
                command.Parameters.AddWithValue($"{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
        return 0;
    }

    public int AnularContrato(int id, int IdUsuarioTermino)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE contratos SET {nameof(Contrato.Anulado)} = true, {nameof(Contrato.IdUsuarioTermino)} = @IdUsuarioTermino WHERE {nameof(Contrato.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
                command.Parameters.AddWithValue($"@IdUsuarioTermino", IdUsuarioTermino); 

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }
    public Boolean validarContrato(Contrato contrato)
    {
        if (contrato.FechaInicio < contrato.FechaTerm)
        {
            return true;
        }
        else
            return false;
    }

    public List<Inmueble> obtenerInmDisp(DateTime fechaInicio, DateTime fechaFin)
    {
        var contratos = GetContratos();
        RepositorioInmueble ri = new RepositorioInmueble();
        var inmuebles = ri.ObtenerTodos();

         var inmueblesOcupadosIds = contratos
        .Where(contrato =>
            !contrato.Anulado &&  // Solo considerar contratos que NO están anulados (Anulado = false)
            (
                (fechaInicio > contrato.FechaInicio && fechaInicio < contrato.FechaTerm) ||  // Fecha inicio está dentro de un contrato existente
                (fechaFin > contrato.FechaInicio && fechaFin < contrato.FechaTerm) ||        // Fecha fin está dentro de un contrato existente
                (fechaInicio <= contrato.FechaInicio && fechaFin >= contrato.FechaTerm)      // El nuevo contrato abarca todo el periodo de un contrato existente
            )
        )
        .Select(contrato => contrato.IdInmueble)
        .Distinct()
        .ToList();


        // Filtrar inmuebles disponibles
        var inmueblesDisponibles = inmuebles
            .Where(inmueble => !inmueblesOcupadosIds.Contains(inmueble.Id))
            .ToList();

        return inmueblesDisponibles;
    }

    public int FinalizarContrato(Contrato contrato)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE contratos SET 
                    {nameof(Contrato.FechaFinalizacion)} = @{nameof(Contrato.FechaFinalizacion)},{nameof(Contrato.Anulado)} = true      
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id)}", contrato.Id);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaFinalizacion)}", contrato.FechaFinalizacion);
                command.Parameters.AddWithValue($"@{nameof(Contrato.Anulado)}", true);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
        return 0;
    }

}