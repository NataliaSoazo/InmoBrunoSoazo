using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PROYECTO_BRUNO_SOAZO.Models;
public class Inmueble

    {
    [Display(Name = "CÓDIGO")]
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Ingrese una dirección válida")]
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "DIRECCIÓN")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Tipo { get; set; }
    [Display(Name = "USO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Uso { get; set; }
    [Display(Name = "AMBIENTES")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public int Ambientes { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "PRECIO APROX.")]
    public double? Precio { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(20, MinimumLength = 9, ErrorMessage = "Ingrese una latitud válida")]
    [Display(Name = "LATITUD")]
    public string? Latitud { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(20, MinimumLength = 9, ErrorMessage = "Ingrese una longitud válida")]
    [Display(Name = "LONGITUD")]
    public string? Longitud { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "DISPONIBLE")]
    public string? Disponible { get; set; }
    [Display(Name = "DUEÑO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public int PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }
}

        public class TipoInmueble{
            public string Tipo { get; set; }
        }

    