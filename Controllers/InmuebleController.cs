using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO.Models;
using ZstdSharp.Unsafe;

namespace PROYECTO_BRUNO_SOAZO.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public InmuebleController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()

    { 
        RepositorioInmueble ri = new RepositorioInmueble();
        var lista = ri.ObtenerTodos();
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.Mensaje = TempData["Mensaje"];
        }else if (TempData.ContainsKey("Error"))
        {
            ViewBag.Error = TempData["Error"];
        }
        
        return View(lista);
    }
    public IActionResult Editar(int id)
         
    {    RepositorioPropietario repoPropietario = new RepositorioPropietario();
         ViewBag.Propietarios = repoPropietario.GetPropietarios();
         RepositorioTipoInmueble repoTipo = new RepositorioTipoInmueble();
         ViewBag.TipoInmuebles = repoTipo.ObtenerTipos();
         

        if (id > 0){
            RepositorioInmueble rp = new RepositorioInmueble();
            var inmueble = rp.GetInmueble(id);
            return View(inmueble); 
        } else {
            return View();
        }
    }
    
    public IActionResult Guardar( Inmueble inmueble)
    {
        try
        {   
            inmueble.Direccion = inmueble.Direccion.ToUpper();
            inmueble.Uso = inmueble.Uso.ToUpper();
            inmueble.Disponible = inmueble.Disponible.ToUpper();
            RepositorioInmueble rp = new RepositorioInmueble();
            if (inmueble.Id > 0)
            {
                rp.ModificarInmueble(inmueble);
                TempData["Mensaje"] = "El inmueble se  modificó correctamente.";
                
            }
            else{
                rp.AltaInmueble(inmueble);
            TempData["Mensaje"] = "Se agregó el inmueble correctamente.";
                }
            return RedirectToAction(nameof(Index));
        }
        catch (System.Exception)
      {
        TempData["Error"] = "No se pudo completar la operación.";
        return RedirectToAction(nameof(Index));
        
      }*/
    }
    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioInmueble rp = new RepositorioInmueble();
            rp.EliminarInmueble(id);
            TempData["Mensaje"] = "El inmueble ha sido eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se pudo completar la eliminación.";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Detalles( int id)
    {  
        RepositorioInmueble rp = new RepositorioInmueble();
            var i = rp.GetInmueble(id);
            return View(i); 
    }   
}