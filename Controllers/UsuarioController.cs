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
[Authorize(Policy = "Administrador")]
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
[Authorize(Policy = "Administrador")]
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
[Authorize(Policy = "Administrador")]
public IActionResult Detalle(int id)
{
     if (id == 0)
    {
        TempData["Mensaje"] = "Usuario no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var usuario = repo.ObtenerPorID(id);
        if (usuario == null){
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        return View(usuario);
    }
}
[Authorize(Policy = "Administrador")]
public IActionResult Edicion(int id)
{
      if (id == 0)
    {
        TempData["Mensaje"] = "Usuario no encontrado.";
        return RedirectToAction("Index");
    }
    else
    {
        var usuario = repo.ObtenerPorID(id);
        if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        
        return View(usuario);
    }

}
[Authorize(Policy = "Administrador")]
public async Task<IActionResult> Actualizar(Usuario actualizarUsuario)
{
if (ModelState.IsValid)
    {
        //verificar rengo
        if(actualizarUsuario.Rol == 1){
            actualizarUsuario.RolNombre = "Administrador";
        }else{
            actualizarUsuario.RolNombre = "Empleado";
        }
        //verificar contraseña
        var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
        if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }else{
            var Clavein = actualizarUsuario.Clave;
            if(Clavein != usuario.Clave){
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Clavein,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]), 
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            actualizarUsuario.Clave = hashed;
            }else{
                actualizarUsuario.Clave = usuario.Clave;
            }
        }
    //verificar si el email ya esta registrado
    if(usuario.Email == actualizarUsuario.Email){
    
    }else{
        var e =  await repo.ObtenerPorEmailAsync(actualizarUsuario.Email);
    if(e != null){
        if(actualizarUsuario.Email == e.Email){
            TempData["Mensaje"] = "Error al actualizar: EL email de ese usuario ya esta registrado.";
            return RedirectToAction("Index");
        }
        }
    }
        repo.ActualizarUsuario(actualizarUsuario);
        
        if(usuario.Email == @User.Identity.Name){
            await ActualizarClaimsYReautenticar(actualizarUsuario);
            //Console.WriteLine("ClaimTypes.Name IN: "+@User.Identity.Name);
        }
        
        TempData["Mensaje"] = "Usuario Modificado correctamente.";
        return RedirectToAction("Index");
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Usuario.";
return RedirectToAction("Index");
}

private async Task ActualizarClaimsYReautenticar(Usuario usuarioActualizado)
{
    // Crear una lista de claims actualizada
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioActualizado.Email),  // O el nuevo nombre de usuario
        new Claim(ClaimTypes.Role, usuarioActualizado.RolNombre) // El rol actualizado
        // Puedes añadir más claims si es necesario
    };

    // Crear una nueva identidad de usuario con las claims actualizadas
    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    // Crear un nuevo principal
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    // Re-autenticar al usuario actualizando su cookie de autenticación
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
}


[Authorize]
public IActionResult Perfil()
{
return View();

}
}