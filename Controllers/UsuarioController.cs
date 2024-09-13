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
using System.Net.WebSockets;

namespace PROYECTO_BRUNO_SOAZO;

public class UsuarioController : Controller
{
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment environment;

    private readonly ILogger<HomeController> _logger;

    public UsuarioController(ILogger<HomeController> logger, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _logger = logger;
        this.configuration = configuration;
        this.environment = environment;

    }
    public IActionResult Index()
    {
        RepositorioUsuario repositorio = new RepositorioUsuario();
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
    public IActionResult Crear()
    {
        RepositorioRol repoRol = new RepositorioRol();
        ViewBag.Roles = repoRol.ObtenerRoles();
        return View();
    }

    [HttpPost]
    public IActionResult Guardar(Usuario usuario)
    {
        RepositorioUsuario repositorio = new RepositorioUsuario();
        if (!ModelState.IsValid)//valida que el formulario coincida con el modelo
            return View();
        try
        {
            string hashedPassword = HashPassword(usuario.Clave);
            usuario.Clave = hashedPassword;
            usuario.Rol = usuario.Rol;
            usuario.Nombre = usuario.Nombre.ToUpper();
            usuario.Apellido = usuario.Apellido.ToUpper();
            int res = repositorio.AltaUsuario(usuario);
            if (usuario.AvatarFile != null && usuario.Id > 0)
            {
                GuardarAvatar(usuario); // Método separado para manejo de archivos
            }
            else{
                 usuario.AvatarURL = Path.Combine("/ImgSubidas","anonimo.jpg" );
                repositorio.ModificarUsuario(usuario);
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Mensaje"] = "Ocurrió un error al guardar el usuario";
            return RedirectToAction(nameof(Index));
        }
    }

    private string HashPassword(string password)
    {

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 1000,
            numBytesRequested: 256 / 8));
        return hashed;
    }

    private void GuardarAvatar(Usuario usuario)
    {
        try
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "ImgSubidas");
            // Crear el directorio si no existe
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // Crear un nombre de archivo único con el ID del usuario
            string fileName = "avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
            string fullPath = Path.Combine(path, fileName);
            // Guardar la imagen en el servidor
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                usuario.AvatarFile.CopyTo(stream);
            }
            // Guardar la ruta del AvatarURL en el modelo de usuario
            usuario.AvatarURL = Path.Combine("/ImgSubidas", fileName);
            // Actualizar el usuario en la base de datos
            RepositorioUsuario repositorio = new RepositorioUsuario();
            repositorio.EditarAvatar(usuario);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al guardar el avatar";
        }
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        try{
        if (id > 0)
        {
            RepositorioRol repoRol = new RepositorioRol();
            ViewBag.Roles = repoRol.ObtenerRoles();
            RepositorioUsuario repositorio = new RepositorioUsuario();
            var usuario = repositorio.getUsuario(id);
            return View(usuario);
        }
        else
        {
            return View();
        }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener los datos del usuario";
            return RedirectToAction(nameof(Index));
        }
    }
    [HttpPost]
    public IActionResult Editar(int id, Usuario u)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(u); // Retorna con errores de validación
            }

            RepositorioUsuario ru = new RepositorioUsuario();
            var usuarioExistente = ru.getUsuario(id);

            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = u.Nombre.ToUpper();
                usuarioExistente.Apellido = u.Apellido.ToUpper();
                usuarioExistente.Correo = u.Correo;
                if(u.AvatarFile!=null){
                    usuarioExistente.AvatarFile = u.AvatarFile;
                    GuardarAvatar(usuarioExistente);
                }
                ru.EditarDatos(usuarioExistente);
                TempData["Mensaje"] = "Datos del usuario actualizados correctamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al actualizar los datos del usuario";
            return RedirectToAction(nameof(Index));
        }
    }
     [HttpPost]
    public IActionResult CambiarContraseña(int id, Usuario u)
    {
    try
    {
               RepositorioUsuario ru = new RepositorioUsuario();
                var user = ru.getUsuario(id);
               if(user!= null){
                user.Clave = HashPassword(u.Clave);
                ru.EditarClave(user);
                TempData["Mensaje"] = "Clave editada correctamente";
                return RedirectToAction(nameof(Index));
               }else{
                 TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction(nameof(Index));
                        
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al actualizar los datos del usuario";
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

    [HttpPost]
    public async Task<IActionResult> Loguin(Log login)
    {


        var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
        if (ModelState.IsValid)
        {
            string comprobarHash = HashPassword(login.Clave);
            RepositorioUsuario repositorio = new RepositorioUsuario();
            var e = repositorio.ObtenerPorEmail(login.Usuario);
            if (e == null || e.Clave != comprobarHash)
            {
                ModelState.AddModelError("", "El email o la clave no son correctos");
                TempData["returnUrl"] = returnUrl;
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Correo),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                       new Claim(ClaimTypes.Role, e.Datos.rol),
                    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
            TempData.Remove("returnUrl");
            return RedirectToAction("Index", "Home");
        }

        TempData["returnUrl"] = returnUrl;
        return RedirectToAction("Index", "Home");


    }
    [Route("salir", Name = "logout")]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

}