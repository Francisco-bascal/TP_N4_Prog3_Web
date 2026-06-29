using TP_N4_Prog3_Web.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TP_N4_Prog3_Web.DTOs;

namespace TP_N4_Prog3_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsuarioService _servicio;
        public AuthController(IUsuarioService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            try
            {
                var resultado = await _servicio.GetUsuarioByCredencialesAsync(login.Nombre, login.Pass);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View(login);
                }

                //Información que almacena la cookie sobre el usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, resultado.Value.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, resultado.Value.Nombre),
                    new Claim(ClaimTypes.Role, resultado.Value.Rol)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                TempData["Exito"] = "Usuario Logueado Correctamente";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View(login);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); //Esto Implementarlo con SWEET ALERT
        }
    }
}