using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmobiliaria_MVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace Proyecto_Inmobiliaria_MVC.Controllers;

public class UsuarioController : Controller
{
private readonly ILogger<UsuarioController> _logger;
private RepositorioUsuario repo;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        repo = new RepositorioUsuario();
   
    }
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

            var e = await repo.ObtenerPorEmailAsync(loginusuario.Email);
            //Console.WriteLine("Clave de la cuenta: " + e.Email);
            Console.WriteLine("Clave ingresada: " + loginusuario.Clave);
            if (e == null || e.Clave != loginusuario.Clave)
				{
                   
					TempData["Mensaje"] = "Email: "+loginusuario.Email+" contraseña incorrecta: ."+loginusuario.Clave;
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
}