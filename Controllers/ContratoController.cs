using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IActionResult Index()
    {
        RepositorioContrato rc = new RepositorioContrato();
        IList<Contrato> lista = new List<Contrato>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;

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
    [Authorize]
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
            if (contrato.Anulado == true)
            {
                TempData["Error"] = "No se puede modificar un contrato anulado.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(contrato);
            }
        }
        else
        {
            return View();
        }
    }
    [Authorize]
    public IActionResult Guardar(Contrato contrato)
    {
        RepositorioUsuario ru = new RepositorioUsuario();
        try
        {
            RepositorioContrato rc = new RepositorioContrato();
           // Boolean validado = rc.validarContrato(contrato);
            var usuario = ru.ObtenerPorEmail(User.Identity.Name);
           // if (validado == true)
           // {

                if (contrato.Id > 0)
                {
                    rc.ModificarContrato(contrato);
                    return RedirectToAction(nameof(Index));

                }
                else
                    contrato.IdUsuarioComenzo = usuario.Id;
                contrato.IdUsuarioTermino = null;
                rc.AltaContrato(contrato);
                return RedirectToAction(nameof(Index));
           // }
           // else TempData["Error"] = "El contrato debe tener una duración mínima de dos años.";
           // return RedirectToAction(nameof(Index));
        }
        catch (System.Exception)
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        RepositorioUsuario ru = new RepositorioUsuario();
        var usuario = ru.ObtenerPorEmail(User.Identity.Name);
        try
        {
            RepositorioContrato rc = new RepositorioContrato();
            rc.AnularContrato(id, usuario.Id);

            TempData["Mensaje"] = "El contrato ha sido eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se pudo completar la eliminación." + usuario.Id;
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize]
    public IActionResult Detalles(int id)
    {
        RepositorioUsuario ru = new RepositorioUsuario();
        RepositorioContrato rc = new RepositorioContrato();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        var i = rc.GetContrato(id);
        var usuarioC = ru.getUsuario(i.IdUsuarioComenzo);
        ViewBag.UsuarioC = usuarioC;
        if (i.IdUsuarioTermino.HasValue)
        {
            var usuarioT = ru.getUsuario(i.IdUsuarioTermino.Value);
            ViewBag.UsuarioT = usuarioT;
        }

        return View(i);
    }
    [Authorize]
    public IActionResult VerVigentes()
    {
        RepositorioContrato rc = new RepositorioContrato();
        IList<Contrato> lista = new List<Contrato>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            lista = rc.GetContratos();
            lista = lista.Where(x =>
                x.FechaTerm > DateTime.Now &&  // Contratos cuya fecha de término es mayor a la fecha actual
                x.Anulado == false             // Contratos que NO están anulados
            ).ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener la lista de contratos vigentes";
            return View(lista);
        }
    }
    [Authorize]
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
    [Authorize]
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
        if (contrato.FechaTerm > DateTime.Now && contrato.Anulado == false)
        {
            TempData["Error"] = "El contrato aun no ha terminado";
            return RedirectToAction(nameof(Index));
        }
        var nuevoContrato = new Contrato
        {
            IdInmueble = contrato.IdInmueble,
            IdInquilino = contrato.IdInquilino,
            MontoMensual = contrato.MontoMensual,
            FechaInicio = DateTime.Now,
            FechaTerm = DateTime.Now.AddYears(1),
        };

        // Pasar el nuevo contrato a la vista
        return View("editar", nuevoContrato);
    }
    
    public IActionResult FinalizarContrato(int id)
    {
        
        RepositorioContrato rc = new RepositorioContrato();
        var i = rc.GetContrato(id);
       if(!i.Anulado){
        var calcularMulta = CalcularMulta(i);
        ViewBag.Multa = calcularMulta;   
        return View(i);
       }
       else{
        TempData["Error"] = "El contrato está anulado";
         return RedirectToAction(nameof(Index));
       }
    }
    [HttpPost]
     public IActionResult Finalizar(int id)
    {   
         RepositorioContrato rc = new RepositorioContrato();
        var contrato = rc.GetContrato(id);
        var i = rc.GetContrato(id);
        var deuda = Adeuda(i);
        if(deuda>0){
         ViewBag.Error = "El contrato tiene " + deuda + " meses de deudas, no se puede finalizar";
         return View("FinalizarContrato", i);
        }
        if (YaPagoMulta(i)){
            rc.FinalizarContrato(i);
           return RedirectToAction(nameof(Index));
        }
        else{
              ViewBag.error = "Debes asentar la multa correspondiente para finalizar el contrato."; 
             return View("FinalizarContrato", i);
        }
        
    }
    
    private  int Adeuda(Contrato i){
        RepositorioPago rp = new RepositorioPago();
        List<Pago> cuotas = new List<Pago>();
        var lista = rp.ObtenerPagosPorContrato(i.Id);
        int anosTranscurridos = DateTime.Now.Year- i.FechaInicio.Year;
        int mesesTranscurridos = DateTime.Now.Month - i.FechaInicio.Month;
        int TiempoTranscurrido = anosTranscurridos* 12 + mesesTranscurridos;
        
         if(lista!=null){
             foreach (var item in (List<Pago>) lista)
          
                    {
                       if(item.Referencia =="CUOTA"){
                        cuotas.Add(item);
                       }
                    }
        
        var deuda = TiempoTranscurrido-cuotas.Count();
        return deuda;
         }
         else{
            return mesesTranscurridos;
         }


        
    }
    public double CalcularMulta(Contrato i){
        RepositorioContrato rp = new RepositorioContrato();
        var contrato = rp.GetContrato(i.Id);
        var aniosFaltantes = contrato.FechaTerm.Year-DateTime.Now.Year;
        var mesesFaltantes = contrato.FechaTerm.Month -DateTime.Now.Month;
        var tiempoFaltante = aniosFaltantes* 12 + mesesFaltantes;

        var aniosAcordados = contrato.FechaTerm.Year-contrato.FechaInicio.Year;
        var mesesAcordados =  contrato.FechaTerm.Month-contrato.FechaInicio.Month;
        var tiempoAcordado = aniosAcordados*12+mesesAcordados;
        var  multa = 0.0;
        if(tiempoFaltante/tiempoAcordado > 0.5){
            multa = contrato.MontoMensual*2;
        }
        else{
            multa = contrato.MontoMensual;
        }
        return multa;
    }

    public Boolean YaPagoMulta(Contrato i){
        Boolean  pago = false;
        var multa = CalcularMulta(i);
        RepositorioPago rp = new RepositorioPago();
        List<Pago> cuotas = new List<Pago>();
        var lista = rp.ObtenerPagosPorContrato(i.Id);
        if(lista!=null){
             foreach (var item in (List<Pago>) lista)
          
                    {
                       if(item.Referencia =="MULTA" && item.Importe == multa){
                        pago = true;
                       }
                    }
             }
             return pago;
    }         
}

