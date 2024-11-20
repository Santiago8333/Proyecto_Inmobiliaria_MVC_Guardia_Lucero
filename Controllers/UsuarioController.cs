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
        Console.WriteLine("Login: "+loginusuario.Email);
        Console.WriteLine("Login: "+loginusuario.Clave);
        if (!ModelState.IsValid)
{
    Console.WriteLine("El modelo no es válido");
    foreach (var modelState in ModelState.Values)
    {
        foreach (var error in modelState.Errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }
    }
    TempData["Mensaje"] = "Error de credenciales";
    return View();
}
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
           
            //Console.WriteLine("Clave de la cuenta: " + e.Clave);
            //Console.WriteLine("Clave ingresada: " + loginusuario.Clave);
            var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, e.Email),
					new Claim("FullName", e.Nombre + " " + e.Apellido),
                    new Claim("AvatarUrl", e.AvatarUrl),
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
public async Task<IActionResult> Agregar(Usuario nuevoUsuario,IFormFile AvatarFile)
{
    //verificar si el email ya esta registrado
    var e = await repo.ObtenerPorEmailAsync(nuevoUsuario.Email);
    if(e != null){
    if(nuevoUsuario.Email == e.Email){
        TempData["Mensaje"] = "Error: EL email de ese usuario ya esta registrado.";
        return RedirectToAction("Index");
    }
    }
    
if (!ModelState.IsValid)
{
    Console.WriteLine("El modelo no es válido");
    foreach (var modelState in ModelState.Values)
    {
        foreach (var error in modelState.Errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }
    }
}
 ModelState.Remove("AvatarFile");
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
// Verificar si se subió un archivo de avatar
        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(AvatarFile.FileName);
            var extension = Path.GetExtension(AvatarFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await AvatarFile.CopyToAsync(stream);
            }

            Console.WriteLine("Archivo cargado: " + fileName);
            nuevoUsuario.AvatarUrl = $"/avatars/{newFileName}";
        }
        else
        {
            Console.WriteLine("No se subió un archivo de avatar.");
            nuevoUsuario.AvatarUrl = "/avatars/default-avatar.png";  // Asignar avatar por defecto
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
    // Verificar si tiene una imagen personalizada y no es la imagen por defecto
    if (!string.IsNullOrEmpty(usuario.AvatarUrl) && usuario.AvatarUrl != "/avatars/default-avatar.png")
    {
        
        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.AvatarUrl.TrimStart('/'));

        if (System.IO.File.Exists(avatarPath))
        {
            try
            {
                // Eliminar la imagen del servidor
                System.IO.File.Delete(avatarPath);
                Console.WriteLine("Imagen eliminada: " + avatarPath);
            }
            catch (Exception ex)
            {
                // Manejo de errores si la imagen no se puede eliminar
                Console.WriteLine("Error al eliminar la imagen: " + ex.Message);
            }
        }
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
            return RedirectToAction("Edicion", new { id = usuario.Id_usuario });
        }
        }
    }
        repo.ActualizarUsuario(actualizarUsuario);
        
        if(usuario.Email == @User.Identity.Name){
            
            await ActualizarClaimsYReautenticarEdicion(actualizarUsuario);
            //Console.WriteLine("ClaimTypes.Name IN: "+@User.Identity.Name);
        }
        
        TempData["Mensaje"] = "Usuario Modificado correctamente.";
        return RedirectToAction("Edicion", new { id = usuario.Id_usuario });
    }
TempData["Mensaje"] = "Hubo un error al Modificar el Usuario.";
return RedirectToAction("Index");
}


[Authorize]
public async Task<IActionResult> Perfil()
{
    var usuario = await repo.ObtenerPorEmailAsync(@User.Identity.Name);
    return View(usuario);
}
public async Task<IActionResult> ActualizarPerfil(Usuario actualizarUsuario)
{
if (ModelState.IsValid)
    {
        //verificar si hay usuario
        var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
        if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }
        //verificar si el email ya esta registrado
    if(usuario.Email == actualizarUsuario.Email){
    
    }else{
        var e =  await repo.ObtenerPorEmailAsync(actualizarUsuario.Email);
    if(e != null){
        if(actualizarUsuario.Email == e.Email){
            TempData["Mensaje"] = "Error al actualizar: EL email de ese usuario ya esta en uso.";
            return RedirectToAction("Perfil");
        }
        }
    }
    
        repo.ActualizarUsuarioPerfil(actualizarUsuario);
        
        if(usuario.Email == @User.Identity.Name){
            actualizarUsuario.RolNombre = usuario.RolNombre;
            actualizarUsuario.AvatarUrl = usuario.AvatarUrl;
            await ActualizarClaimsYReautenticar(actualizarUsuario);
            //Console.WriteLine("ClaimTypes.Name IN: "+@User.Identity.Name);
        }
        TempData["Mensaje"] = "Perfil Modificado correctamente.";
        return RedirectToAction("Perfil");
    }
    
TempData["Mensaje"] = "Hubo un error al Actualizar Perfil el Usuario.";
return RedirectToAction("Index");
}
[Authorize]
public IActionResult ActualizarClave(Usuario actualizarUsuario)
{
    if (ModelState.IsValid)
    {
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
            //verificar la contraseña vieja 
            string hashed2 = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: actualizarUsuario.ClaveAntigua,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]), 
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            actualizarUsuario.ClaveAntigua = hashed2;
            if(usuario.Clave != actualizarUsuario.ClaveAntigua){
                TempData["Mensaje"] = "Error: la clave antigua es incorrecta.";
                return RedirectToAction("Perfil");
            }

        }
        repo.ActualizarUsuarioClave(actualizarUsuario);
        TempData["Mensaje"] = "Clave Modificada correctamente.";
        return RedirectToAction("Perfil");
    }
TempData["Mensaje"] = "Hubo un error al Actualizar la Clave del Usuario.";
return RedirectToAction("Perfil");
}
//actualizar avatar
[Authorize(Policy = "Administrador")]
public async Task<IActionResult> ActualizarAvatar(Usuario actualizarUsuario,IFormFile AvatarFile)
{
    ModelState.Remove("AvatarFile");
    if (ModelState.IsValid)
    {
         var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
        if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }else{
// Verificar si se subió un archivo de avatar
        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(AvatarFile.FileName);
            var extension = Path.GetExtension(AvatarFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await AvatarFile.CopyToAsync(stream);
            }

            Console.WriteLine("Archivo cargado: " + fileName);
            actualizarUsuario.AvatarUrl = $"/avatars/{newFileName}";
        }
        else
        {
            Console.WriteLine("No se subió un archivo de avatar.");
            actualizarUsuario.AvatarUrl = "/avatars/default-avatar.png";  // Asignar avatar por defecto
        }
// Verificar si tiene una imagen personalizada y no es la imagen por defecto
    if (!string.IsNullOrEmpty(usuario.AvatarUrl) && usuario.AvatarUrl != "/avatars/default-avatar.png")
    {
        
        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.AvatarUrl.TrimStart('/'));

        if (System.IO.File.Exists(avatarPath))
        {
            try
            {
                // Eliminar la imagen del servidor
                System.IO.File.Delete(avatarPath);
                Console.WriteLine("Imagen eliminada: " + avatarPath);
            }
            catch (Exception ex)
            {
                // Manejo de errores si la imagen no se puede eliminar
                Console.WriteLine("Error al eliminar la imagen: " + ex.Message);
            }
        }
    }
        }
        TempData["Mensaje"] = "Avatar actualizado.";
        repo.ActualizarAvatar(actualizarUsuario);
        return RedirectToAction("index");
    }
TempData["Mensaje"] = "Hubo un error al Actualizar El avatar del Usuario.";
return RedirectToAction("index");
}
//actualizar avatar usuario
[Authorize]
public async Task<IActionResult> ActualizarUsuarioAvatar(Usuario actualizarUsuario,IFormFile AvatarFile)
{
ModelState.Remove("AvatarFile");
if (ModelState.IsValid)
    {
         var usuario = repo.ObtenerPorID(actualizarUsuario.Id_usuario);
        if (usuario == null)
        {
            TempData["Mensaje"] = "Usuario no encontrado.";
            return RedirectToAction("Index");
        }else{
// Verificar si se subió un archivo de avatar
        if (AvatarFile != null && AvatarFile.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(AvatarFile.FileName);
            var extension = Path.GetExtension(AvatarFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", newFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await AvatarFile.CopyToAsync(stream);
            }

            Console.WriteLine("Archivo cargado: " + fileName);
            actualizarUsuario.AvatarUrl = $"/avatars/{newFileName}";
            await ActualizarClaimsYReautenticarAvatar(actualizarUsuario);
        }
        else
        {
            Console.WriteLine("No se subió un archivo de avatar.");
            actualizarUsuario.AvatarUrl = "/avatars/default-avatar.png";  // Asignar avatar por defecto
        }
// Verificar si tiene una imagen personalizada y no es la imagen por defecto
    if (!string.IsNullOrEmpty(usuario.AvatarUrl) && usuario.AvatarUrl != "/avatars/default-avatar.png")
    {
        
        var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.AvatarUrl.TrimStart('/'));

        if (System.IO.File.Exists(avatarPath))
        {
            try
            {
                // Eliminar la imagen del servidor
                System.IO.File.Delete(avatarPath);
                Console.WriteLine("Imagen eliminada: " + avatarPath);
            }
            catch (Exception ex)
            {
                // Manejo de errores si la imagen no se puede eliminar
                Console.WriteLine("Error al eliminar la imagen: " + ex.Message);
            }
        }
    }
        }
        TempData["Mensaje"] = "Avatar actualizado.";
        repo.ActualizarAvatar(actualizarUsuario);
        return RedirectToAction("Perfil");
    }
TempData["Mensaje"] = "Hubo un error al Actualizar El avatar del Usuario.";
return RedirectToAction("Perfil");
}
//actualizar claim 
private async Task ActualizarClaimsYReautenticar(Usuario usuarioActualizado)
{
    // Crear una lista de claims actualizada
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioActualizado.Email),
        new Claim("FullName", usuarioActualizado.Nombre + " " + usuarioActualizado.Apellido),
        new Claim("AvatarUrl", usuarioActualizado.AvatarUrl),
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
private async Task ActualizarClaimsYReautenticarAvatar(Usuario usuarioActualizado)
{
    // Crear una lista de claims actualizada
    var claims = new List<Claim>
    {
        new Claim("AvatarUrl", usuarioActualizado.AvatarUrl),
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
private async Task ActualizarClaimsYReautenticarEdicion(Usuario usuarioActualizado)
{
    // Crear una lista de claims actualizada
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioActualizado.Email),
        new Claim("FullName", usuarioActualizado.Nombre + " " + usuarioActualizado.Apellido),
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
}