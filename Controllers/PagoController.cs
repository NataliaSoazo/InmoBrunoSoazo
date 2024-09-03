using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO.Models;


namespace PROYECTO_BRUNO_SOAZO.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public PagoController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        RepositorioPago rp = new RepositorioPago();
        IList<Pago> lista = new List<Pago>();
        try
        {
            lista = rp.GetPagos();
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
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "Ocurrio un error al obtener la lista de pagos";
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
    }

    public IActionResult Editar(int id)

    {
        RepositorioContrato repoContrato = new RepositorioContrato();
        ViewBag.Contratos = repoContrato.GetContratos();

        if (id > 0)
        {
            RepositorioPago rp = new RepositorioPago();
            var pago = rp.GetPago(id);
            return View(pago);
        }
        else
        {
            return View();
        }
    }

    public IActionResult Guardar(Pago pago)
    {
        try
        {
            RepositorioPago rp = new RepositorioPago();

            if (pago.Id > 0)
            {
                rp.ModificarPago(pago);
                return RedirectToAction(nameof(Index));
            }
            else
                rp.AltaPago(pago);
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
            RepositorioPago rp = new RepositorioPago();
            rp.EliminarPago(id);
            TempData["Mensaje"] = "El pago ha sido eliminado correctamente.";
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
        RepositorioPago rp = new RepositorioPago();
        var p = rp.GetPago(id);
        return View(p);
    }
}
