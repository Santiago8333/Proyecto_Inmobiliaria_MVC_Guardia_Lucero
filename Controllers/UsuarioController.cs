using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class UsuarioController : Controller
{
private readonly IConfiguration configuration;
private readonly ILogger<UsuarioController> _logger;
private readonly RepositorioUsuario repo;

    public UsuarioController(IConfiguration configuration,ILogger<UsuarioController> logger)
    {
        _logger = logger;
        repo = new RepositorioUsuario();
        this.configuration = configuration;
   
    }
[Authorize(Policy = "Administrador")]
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
{
    var UsuarioQueryable = repo.ObtenerTodos().AsQueryable();
    var paginacion = await Paginacion<Usuario>.CrearPaginacion(UsuarioQueryable, pageNumber, pageSize);

    return View(paginacion);
}
/*
public IActionResult Index()
{
return View();
}
*/

public IActionResult Login()
{
return View();
}

/*
public IActionResult Register()
{
return View();
}
*/
    [HttpPost]
    public async Task<IActionResult> Login(Usuario loginusuario)
    {
        
        if (ModelState.IsValid)
        {
            
            // Aquí iría tu lógica de autenticación
            // Hash de la contraseña ingresada por el usuario durante el login
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginusuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]), // Asegúrate de usar la misma sal que usaste al registrar el usuario
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            var e = await repo.ObtenerPorEmailAsync(loginusuario.Email);
            //Console.WriteLine("Clave de la cuenta: " + e.Email);
            //Console.WriteLine("Clave ingresada: " + loginusuario.Clave+" hasehd: "+hashed);
            if (e == null || e.Clave != hashed)
				{
                   
					TempData["Mensaje"] = "Credenciales ingresadas incorrectas";
                    TempData["Mensaje2"] = "Credenciales ingresadas incorrectas";
                    TempData["MensajeTipo"] = "danger";
					return RedirectToAction("Login");
				}
            Console.WriteLine("Clave de la cuenta: " + e.Clave);
            //Console.WriteLine("Clave ingresada: " + loginusuario.Clave);
            var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, e.Email),
					new Claim("FullName", e.Nombre + " " + e.Apellido),
					new Claim(ClaimTypes.Role, e.RolNombre),
				};
            var claimsIdentity = new ClaimsIdentity(
						claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity));
            TempData["Mensaje"] = "Credenciales Correctas";
            return RedirectToAction("","Home");
        }
        TempData["Mensaje"] = "Error de credenciales";
        return View();
    }
    //Get logout usuario
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuario");
    }
public async Task<IActionResult> Agregar(Usuario nuevoUsuario)
{
    //verificar si el email ya esta registrado
    var e = await repo.ObtenerPorEmailAsync(nuevoUsuario.Email);
    if(e != null){
    if(nuevoUsuario.Email == e.Email){
        TempData["Mensaje"] = "Error: EL email de ese usuario ya esta registrado.";
        return RedirectToAction("Index");
    }
    }
    //Console.WriteLine("afuera: " + nuevoUsuario);
    if (ModelState.IsValid)
    {
        //Console.WriteLine("dentro: " + nuevoUsuario);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: nuevoUsuario.Clave,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]), 
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
		nuevoUsuario.Clave = hashed;
        //verificar rengo
        if(nuevoUsuario.Rol == 1){
            nuevoUsuario.RolNombre = "Administrador";
        }else{
            nuevoUsuario.RolNombre = "Empleado";
        }

        repo.AgregarUsuario(nuevoUsuario);
        TempData["Mensaje"] = "Usuario agregado exitosamente.";
        return RedirectToAction("Index");
    }

    TempData["Mensaje"] = "Hubo un error al agregar el Usuario.";
    
    return RedirectToAction("Index");
}
public IActionResult Eliminar(int id)
{
    var usuario = repo.ObtenerPorID(id);
    if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        repo.EliminarUsuario(id);
TempData["Mensaje"] = "Usuario Eliminado.";
 return RedirectToAction("Index");

}
}