using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO;
using PROYECTO_BRUNO_SOAZO.Controllers;
using PROYECTO_BRUNO_SOAZO.Models;


namespace PROYECTO_BRUNO_SOAZOControllers;

public class PropietarioController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public PropietarioController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioPropietario rp = new RepositorioPropietario();
        var lista = rp.GetPropietarios();
        return View(lista);
    }
    public IActionResult Editar(int id)
    {
        if (id > 0){
            RepositorioPropietario rp = new RepositorioPropietario();
            var propietario = rp.getPropietario(id);
            return View(propietario); 
        } else {
            return View();
        }
    }
    
    public IActionResult Guardar( Propietario propietario)
    {  
        RepositorioPropietario rp = new RepositorioPropietario();
        if(propietario.Id > 0){
            rp.ModificarPropietario(propietario);

        }else
        rp.AltaPropietario(propietario);
        return RedirectToAction(nameof(Index));
    }
     public IActionResult Eliminar( int id)
    {  
        RepositorioPropietario rp = new RepositorioPropietario();
        rp.EliminarPersona(id);
        return RedirectToAction(nameof(Index));
    }    
    public IActionResult Detalles( int id)
    {  
        RepositorioPropietario rp = new RepositorioPropietario();
            var propietario = rp.getPropietario(id);
            return View(propietario); 
    }    
}