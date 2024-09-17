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
        IList<Contrato> lista = new List<Contrato>();
        try
        {
            lista = rc.GetContratos();
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            else if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de contratos");
            TempData["Error"] = "Ocurrio un error al obtener la lista de contratos";
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
    }
    public IActionResult Editar(int id)

    {
        RepositorioContrato repoContrato = new RepositorioContrato();
        var contratos = repoContrato.GetContratos();
        RepositorioInmueble repoInmueble = new RepositorioInmueble();
        var inmuebles = repoInmueble.ObtenerTodos();
        RepositorioInquilino repoInquilino = new RepositorioInquilino();
        ViewBag.Inquilinos = repoInquilino.GetInquilinos();
        var inmueblesJson = Newtonsoft.Json.JsonConvert.SerializeObject(inmuebles);
        ViewBag.InmueblesJson = inmueblesJson;
        var contratosJson = Newtonsoft.Json.JsonConvert.SerializeObject(contratos);
        ViewBag.ContratosJson = contratosJson;

        if (id > 0)
        {
            RepositorioContrato rc = new RepositorioContrato();
            var contrato = rc.GetContrato(id);
            return View(contrato);
        }
        else
        {
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
            else TempData["Error"] = "El contrato debe tener una duración mínima de dos años.";
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

    public IActionResult Detalles(int id)
    {
        RepositorioContrato rc = new RepositorioContrato();
        var i = rc.GetContrato(id);
        return View(i);
    }

    public IActionResult VerVigentes()
    {
        RepositorioContrato rc = new RepositorioContrato();
        IList<Contrato> lista = new List<Contrato>();
        try
        {
            lista = rc.GetContratos();
            lista = lista.Where(x => x.FechaTerm > DateTime.Now).ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener la lista de contratos vigentes";
            return View(lista);
        }
    }

    public IActionResult ListarContratosInmueble(int id)
    {
        RepositorioContrato rc = new RepositorioContrato();
        IList<Contrato> lista = new List<Contrato>();
        try
        {
            lista = rc.GetContratos();
            lista = lista.Where(x => x.IdInmueble == id).ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener la lista de contratos vigentes";
            return View(lista);
        }
    }

    public IActionResult Renovar(int id)
    {
        RepositorioContrato rc = new RepositorioContrato();
        var contrato = rc.GetContrato(id);
        RepositorioContrato repoContrato = new RepositorioContrato();
        var contratos = repoContrato.GetContratos();
        RepositorioInmueble repoInmueble = new RepositorioInmueble();
        var inmuebles = repoInmueble.ObtenerTodos();
        RepositorioInquilino repoInquilino = new RepositorioInquilino();
        ViewBag.Inquilinos = repoInquilino.GetInquilinos();
        var inmueblesJson = Newtonsoft.Json.JsonConvert.SerializeObject(inmuebles);
        ViewBag.InmueblesJson = inmueblesJson;
        var contratosJson = Newtonsoft.Json.JsonConvert.SerializeObject(contratos);
        ViewBag.ContratosJson = contratosJson;
        if (contrato == null)
        {
            TempData["Error"] = "Contrato no encontrado";
            return RedirectToAction(nameof(Index));
        }
        var nuevoContrato = new Contrato
        {
            IdInmueble = contrato.IdInmueble,
            IdInquilino = contrato.IdInquilino,
            MontoMensual = contrato.MontoMensual,
            FechaInicio = DateTime.Now,
            FechaTerm = DateTime.Now.AddYears(1)
        };

        // Pasar el nuevo contrato a la vista
        return View("editar", nuevoContrato);
    }
}

