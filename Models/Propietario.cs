using System.ComponentModel.DataAnnotations;

namespace PROYECTO_BRUNO_SOAZO.Models;

public class Propietario
{
    [Display(Name = "CÓDIGO")]
    public int Id { get; set; }
    [Display(Name = "NOMBRE")]
    public string? Nombre { get; set; }
    [Display(Name = "APELLIDO")]
    public string? Apellido { get; set; }
    [Display(Name = "E-MAIL")]
    public string? Email { get; set; }
     [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "DNI")]
    public string? Dni { get; set; }
    [Display(Name = "TELEFONO")]
    public string? Telefono { get; set; }
    [Display(Name = "DOMICILIO")]
    public string? Domicilio { get; set; }
    [Display(Name = "CIUDAD")]
    public string? Ciudad { get; set; }
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    
}