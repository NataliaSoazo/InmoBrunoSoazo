using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO;
using PROYECTO_BRUNO_SOAZO.Controllers;
using PROYECTO_BRUNO_SOAZO.Models;


namespace PROYECTO_BRUNO_SOAZOControllers;

public class InquilinoController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public InquilinoController(ILogger<HomeController> logger)
    {

        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var lista = rp.GetInquilinos();
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.Mensaje = TempData["Mensaje"];
        }
        return View(lista);
    }

    public IActionResult Editar(int id)
    {
        if (id > 0)
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            var inquilino = rp.GetInquilino(id);
            return View(inquilino);
        }
        else
        {
            return View();
        }
    }

    public IActionResult Guardar(Inquilino inquilino)
    {
        try
        {
            inquilino.Nombre = inquilino.Nombre.ToUpper();
            inquilino.Apellido = inquilino.Apellido.ToUpper();
            inquilino.Email = inquilino.Email.ToUpper();
            inquilino.Domicilio = inquilino.Domicilio.ToUpper();
            inquilino.Ciudad = inquilino.Ciudad.ToUpper();
            RepositorioInquilino rp = new RepositorioInquilino();

            if (inquilino.Id > 0)
            {
                rp.ModificarInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido modificado";

            }
            else
            {
                rp.AltaInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido guardado";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "Ocurrió un error al guardar el inquilino";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            rp.EliminarInquilino(id);
            TempData["Mensaje"] = "El inquilino ha sido eliminado";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Mensaje"] = "Ocurrió un error al eliminar el inquilino";
            return RedirectToAction(nameof(Index));
        }
    }
    public IActionResult Detalles(int id)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var inquilino = rp.GetInquilino(id);
        return View(inquilino);
    }

    // GET: inquilino/Busqueda
    public IActionResult Busqueda()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {//poner breakpoints para detectar errores
            throw;
        }
    }
    //  [Route("[controller]/Buscar/{q}", Name = "Buscar")]
    public IActionResult Buscar(string q)
    {
        try
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            var res = rp.BuscarPorNombre(q);
            return Json(new { Datos = res });
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }
}