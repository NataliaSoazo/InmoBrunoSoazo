using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO.Models;


namespace PROYECTO_BRUNO_SOAZO.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ContratoController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()

    { 
        RepositorioContrato rc = new RepositorioContrato();
        var lista = rc.GetContratos();
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
         
    {    RepositorioInmueble repoInmueble = new RepositorioInmueble();
         ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
         RepositorioInquilino repoInquilino = new RepositorioInquilino();
         ViewBag.Inquilinos = repoInquilino.GetInquilinos();

        if (id > 0){
            RepositorioContrato rc = new RepositorioContrato();
            var contrato = rc.GetContrato(id);
            return View(contrato); 
        } else {
            return View();
        }
    }

    public IActionResult Guardar(Contrato contrato)
    {
        try
        {   
            RepositorioContrato rc = new RepositorioContrato();
            Boolean validado = rc.validarContrato(contrato);

            if (validado == true)
            {

                if (contrato.Id > 0)
                {
                    rc.ModificarContrato(contrato);
                    return RedirectToAction(nameof(Index));

                }
                else
                    rc.AltaContrato(contrato);
                return RedirectToAction(nameof(Index));

            }
            else ViewBag.Error = "El contrato debe tener una duración mínima de dos años.";
            return RedirectToAction(nameof(Index));
        }
        catch (System.Exception)
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioContrato rc = new RepositorioContrato();
            rc.EliminarContrato(id);
            TempData["Mensaje"] = "El contrato ha sido eliminado correctamente.";
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
        RepositorioContrato rc = new RepositorioContrato();
            var i = rc.GetContrato(id);
            return View(i); 
    }   
}