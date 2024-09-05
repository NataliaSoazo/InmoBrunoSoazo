using PROYECTO_BRUNO_SOAZO;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Diagnostics.Metrics.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509.SigI;

namespace PROYECTO_BRUNO_SOAZO.Models;

public class RepositorioUsuario
{
    readonly string ConnectionString = "Server=localhost;Database=proyecto-bruno-soazo;User=root;Password=;";

    public RepositorioUsuario()
    {

    }

    public IList<Usuario> GetUsuarios()
    {
        var usuarios = new List<Usuario>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Usuario.Id)},
            {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)},  {nameof(Usuario.Correo)}, 
            {nameof(Usuario.Avatar)}, {nameof(Usuario.Rol)}
            FROM usuarios ORDER BY apellido LIMIT 10 OFFSET 0 ;";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Correo = reader.GetString(nameof(Usuario.Correo)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),

                        });
                    }

                }
            }
        }
        return usuarios;
    }

    public int AltaUsuario(Usuario Usuario)
    {
        try
        {
            int id = 0;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @$"INSERT INTO usuarios ({nameof(Usuario.Nombre)},{nameof(Usuario.Apellido)},{nameof(Usuario.Correo)},{nameof(Usuario.Clave)},{nameof(Usuario.Avatar)},{nameof(Usuario.Rol)}
                                            VALUES (@{nameof(Usuario.Nombre)},@{nameof(Usuario.Apellido)},@{nameof(Usuario.Correo)},@{nameof(Usuario.Clave)},@{nameof(Usuario.Avatar)},@{nameof(Usuario.Rol)};         
             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Nombre)}", Usuario.Nombre);
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Apellido)}", Usuario.Apellido);
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Correo)}", Usuario.Correo);
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Clave)}", Usuario.Clave);
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Avatar)}", Usuario.Avatar);
                    command.Parameters.AddWithValue($"@{nameof(Usuario.Rol)}", Usuario.Rol);
                   

                    connection.Open();
                    id = Convert.ToInt32(command.ExecuteScalar());
                    Usuario.Id = id;
                    connection.Close();

                }
            }
            return id;
        }
        catch (Exception ex)
        {
            // Registrar el error o manejarlo adecuadamente
            Console.WriteLine($"Error al insertar el Usuario: {ex.Message}");
            // Puedes relanzar la excepción o manejarla según sea necesario
            throw;
        }
    }

    public Usuario? getUsuario(int id)
    {
        Usuario? usuario = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Usuario.Id)},{nameof(Usuario.Nombre)},{nameof(Usuario.Apellido)},{nameof(Usuario.Correo)},{nameof(Usuario.Avatar)},{nameof(Usuario.Rol)} 
            FROM usuarios WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Usuario.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                           Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Correo = reader.GetString(nameof(Usuario.Correo)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        };
                    }
                }
            }

        }
        return usuario;
    }

    public int ModificarUsuario(Usuario Usuario)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE Usuarios 
            SET {nameof(Usuario.Nombre)} = @{nameof(Usuario.Nombre)},
            {nameof(Usuario.Apellido)} = @{nameof(Usuario.Apellido)},
            {nameof(Usuario.Correo)} = @{nameof(Usuario.Correo)},
            {nameof(Usuario.Clave)} = @{nameof(Usuario.Clave)},
            {nameof(Usuario.Avatar)} = @{nameof(Usuario.Avatar)},
            {nameof(Usuario.Rol)} = @{nameof(Usuario.Rol)}
            WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)} ";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Usuario.Id)}", Usuario.Id);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Nombre)}", Usuario.Nombre);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Apellido)}", Usuario.Apellido);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Correo)}", Usuario.Correo);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Clave)}", Usuario.Clave);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Avatar)}", Usuario.Avatar);
                command.Parameters.AddWithValue($"@{nameof(Usuario.Rol)}", Usuario.Rol);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public int EliminarUsuario(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from usuarios WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Usuario.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public IList<Usuario> BuscarPorNombre(string nombre)
    {
        var res = new List<Usuario>();
        nombre = "%" + nombre + "%";

        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Usuario.Id)},{nameof(Usuario.Nombre)},{nameof(Usuario.Apellido)},{nameof(Usuario.Correo)},{nameof(Usuario.Clave)},{nameof(Usuario.Avatar)},{nameof(Usuario.Rol)} FROM usuarios
                      WHERE {nameof(Usuario.Nombre)} LIKE @{nameof(Usuario.Nombre)} OR {nameof(Usuario.Apellido)} LIKE @{nameof(Usuario.Nombre)}";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add(new MySqlParameter(nameof(nombre), MySqlDbType.VarChar) { Value = nombre });
                command.CommandType = CommandType.Text;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Usuario
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Usuario.Id))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Usuario.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Usuario.Apellido))),
                            Correo = reader.GetString(reader.GetOrdinal(nameof(Usuario.Correo))),
                            Clave = reader.GetString(reader.GetOrdinal(nameof(Usuario.Clave))),
                            Rol = reader.GetInt32(reader.GetOrdinal(nameof(Usuario.Rol))),
                           
                            

                        };
                        res.Add(p);
                    }
                }
            }
        }

        return res;
    }
}




