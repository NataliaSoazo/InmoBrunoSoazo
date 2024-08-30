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
            string sql = $@"SELECT i.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)},
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

							}
                            
                        });
                     }

                }
            }
         }
         return contratos;
    }

    public int AltaContrato(Contrato contrato){
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"INSERT INTO contratos ( {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)}, {nameof(Contrato.IdInmueble)}) 
                VALUES (@{nameof(Contrato.FechaInicio)}, @{nameof(Contrato.FechaTerm)}, @{nameof(Contrato.MontoMensual)}, @{nameof(Contrato.IdInquilino)},@{nameof(Contrato.IdInmueble)});           
                SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaTerm)}", contrato.FechaTerm);
                command.Parameters.AddWithValue($"@{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
               
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                contrato.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Contrato? GetContrato(int id)
    {
        Contrato? contrato = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
                string sql = $@"SELECT i.{nameof(Contrato.Id)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaTerm)}, {nameof(Contrato.MontoMensual)}, {nameof(Contrato.IdInquilino)}, 
                      p.{nameof(Inquilino.Nombre)}, p.{nameof(Inquilino.Apellido)}, {nameof(Contrato.IdInmueble)}, m.{nameof(Inmueble.Direccion)}
                FROM contratos i INNER JOIN Inquilinos p ON i.{nameof(Contrato.IdInquilino)} = p.{nameof(Inquilino.Id)}
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

							}
                        };
                    }
                }
            }
            
        }
        return contrato;
    }

    public int ModificarContrato(Contrato contrato){
        using (var connection = new MySqlConnection(ConnectionString)){
            string sql = $@"UPDATE contratos SET 
                    {nameof(Contrato.FechaInicio)} = @{nameof(Contrato.FechaInicio)},
                    {nameof(Contrato.FechaTerm)} = @{nameof(Contrato.FechaTerm)},
                    {nameof(Contrato.MontoMensual)} = @{nameof(Contrato.MontoMensual)},
                    {nameof(Contrato.IdInquilino)} = @{nameof(Contrato.IdInquilino)},
                    {nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)}
                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}";           
            using (var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id)}", contrato.Id);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaInicio)}", contrato.FechaInicio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FechaTerm)}", contrato.FechaTerm);
                command.Parameters.AddWithValue($"{nameof(Contrato.MontoMensual)}", contrato.MontoMensual);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}",contrato.IdInmueble);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
        return 0;
    }

    public int  EliminarContrato(int id){
        using(var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from contratos WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
         return 0;
    }
    public Boolean validarContrato(Contrato contrato){
        int difAnios = contrato.FechaTerm.Year - contrato.FechaInicio.Year;
       
        if(contrato.FechaInicio < contrato.FechaTerm && difAnios>=2){
            return true;
        }else
        return false;
    }

}