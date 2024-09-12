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

    public IActionResult Editar(int? id, int? idContrato)
    {
        RepositorioContrato repoContrato = new RepositorioContrato();
        ViewBag.Contratos = repoContrato.GetContratos();

        var pago = new Pago();

        if (id.HasValue && id.Value > 0)
        {
            RepositorioPago rp = new RepositorioPago();
            pago = rp.GetPago(id.Value) ?? new Pago();
        }

        if (idContrato.HasValue)
        {
            pago.IdContrato = idContrato.Value;
        }

        return View(pago);
    }

    public IActionResult Guardar(Pago pago, int? idContrato)
    {
        if (idContrato.HasValue)
        {
            pago.IdContrato = idContrato.Value;
        }
        try
        {
            RepositorioPago rp = new RepositorioPago();

            if (pago.Id > 0)
            {
                rp.ModificarPago(pago);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                rp.AltaPago(pago);
                return RedirectToAction(nameof(Index));
            }
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

    public IActionResult PagosContrato(int id)
    {
        RepositorioPago rp = new RepositorioPago();
        IList<Pago> lista = new List<Pago>();
        try
        {
            lista = rp.GetPagos();
            lista = lista.Where(x => x.IdContrato == id).ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener la lista de pagos";
            return View("Index", lista);
        }
    }


}
