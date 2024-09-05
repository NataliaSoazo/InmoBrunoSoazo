using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace PROYECTO_BRUNO_SOAZO.Models
{
	public enum enRoles
	{
		Administrador = 1,
		Empleado = 2,
	}

	public class Usuario
	{
		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }
		[Required]
        [Display(Name = "NOMBRE")]
		public string Nombre { get; set; }
		[Required]
        [Display(Name = "APELLIDO")]
		public string Apellido { get; set; }
		[Required, EmailAddress]
        [Display(Name = "E- MAIL")]
		public string Correo { get; set; }
		[Required, DataType(DataType.Password)]
        [Display(Name = "CLAVE")]
		public string Clave { get; set; }
        
		public string Avatar { get; set; }
		[NotMapped]//Para EF
		[Display(Name = "AVATAR")]
		public IFormFile AvatarFile { get; set; }
        [Display(Name = "ROL")]
		public int Rol { get; set; }
		[NotMapped]//Para EF
		[Display(Name = "ROL")]
		public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}
	}
}
