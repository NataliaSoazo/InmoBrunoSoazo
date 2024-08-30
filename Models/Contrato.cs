using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROYECTO_BRUNO_SOAZO.Models;
    public class Contrato
    {
        [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "INICIO DEL CONTRATO")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "CULMINACIÓN")]
        public DateTime FechaTerm { get; set; }
        [Display(Name = "MONTO MENSUAL")]
        public double MontoMensual { get; set; }
        [Display(Name = "ARRENDATARIO")]
        public int IdInquilino { get; set; }
        
        [Required]
        public Inquilino? Arrendatario { get; set; }
        [Required]
        [Display(Name = "DATOS DEL INMUEBLE")]
        public int IdInmueble { get; set; }
        public Inmueble? Datos { get; set; }

    }