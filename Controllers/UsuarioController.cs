using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_BRUNO_SOAZO.Controllers;
using PROYECTO_BRUNO_SOAZO.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace PROYECTO_BRUNO_SOAZO;

public class UsuarioController : Controller
{
    private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioUsuario repositorio;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment, RepositorioUsuario repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
        }
    public IActionResult Index()
    {
        
        IList<Usuario> lista = new List<Usuario>();
        try
        {
            lista = repositorio.GetUsuarios();
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
            //_logger.LogError(ex, "Error al obtener la lista de usuarios");
            TempData["Error"] = "Ocurrio un error al obtener la lista de usuarios";
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
    }
    public ActionResult Crear()
    {
        ViewBag.Roles = Usuario.ObtenerRoles();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
  
    public ActionResult CrearUsuario(Usuario usuario)
    {
        if (!ModelState.IsValid)
            return View();
        try
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
            usuario.Clave = hashed;
            usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;
            var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
            int res = repositorio.AltaUsuario(usuario);
            if (usuario.AvatarFile != null && usuario.Id > 0)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "ImgSubidas");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Path.GetFileName(usuario.AvatarFile.FileName);//este nombre se puede repetir
                string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                usuario.Avatar = Path.Combine("/ImgSubidas", fileName);
                // Esta operación guarda la foto en memoria en el ruta que necesitamos
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    usuario.AvatarFile.CopyTo(stream);
                }
                repositorio.ModificarUsuario(usuario);
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }
    }

    public IActionResult Editar(int id)
    {
        if (id > 0)
        {
            RepositorioUsuario repositorio = new RepositorioUsuario();
            var usuario = repositorio.getUsuario(id);
            return View(usuario);
        }
        else
        {
            return View();
        }
    }

    
   

    public IActionResult Guardar(Usuario usuario)
    {
        try
        {
            RepositorioUsuario repositorio = new RepositorioUsuario();
            usuario.Nombre = usuario.Nombre.ToUpper();
            usuario.Apellido = usuario.Apellido.ToUpper();

            if (usuario.Id > 0)
            {
                repositorio.ModificarUsuario(usuario);
                TempData["Mensaje"] = "El usuario ha sido modificado";

            }
            else
            {
                repositorio.AltaUsuario(usuario);
                TempData["Mensaje"] = "El usuario ha sido guardado";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "Ocurrió un error al guardar el usuario";
            return RedirectToAction(nameof(Index));
        }

    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioUsuario repositorio = new RepositorioUsuario();
            repositorio.EliminarUsuario(id);
            TempData["Mensaje"] = "El usuario ha sido eliminado";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "Ocurrio un error al eliminar el usuario";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Detalles(int id)
    {
        RepositorioUsuario repositorio = new RepositorioUsuario();
        var usuario = repositorio.getUsuario(id);
        return View(usuario);
    }

    // GET: usuario/Busqueda
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
    //[Route("[controller]/Buscar/{q}", Name = "Buscar")]
    public IActionResult Buscar(string q)
    {
        try
        {
            RepositorioUsuario repositorio = new RepositorioUsuario();
            var res = repositorio.BuscarPorNombre(q);
            return Json(new { Datos = res });
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }
}