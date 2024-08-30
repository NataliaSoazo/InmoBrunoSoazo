using PROYECTO_BRUNO_SOAZO.Models;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Diagnostics.Metrics.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509.SigI;

namespace PROYECTO_BRUNO_SOAZO.Models;
public class RepositorioTipoInmueble
{
    readonly string ConnectionString = "Server=localhost;Database=proyecto-bruno-soazo;User=root;Password=;";

    public RepositorioTipoInmueble()
    {

    }
    
 public IList<TipoInmueble>ObtenerTipos()
		{
			 var tipos = new List<TipoInmueble>();
			 using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = @$"SELECT {nameof(TipoInmueble.Tipo)}
					FROM tipoinmuebles ORDER BY tipo ASC ";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						tipos.Add(new TipoInmueble
						{
							
                            Tipo =reader.GetString(nameof(TipoInmueble.Tipo)),
                            
							
						});	
					}
					connection.Close();
				}
			}
			return tipos;
		}
    
}