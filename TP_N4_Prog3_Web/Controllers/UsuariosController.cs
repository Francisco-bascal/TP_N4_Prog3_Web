using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios;
using APP_PRUEBA_1.Servicios.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APP_PRUEBA_1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _servicio;
        public UsuariosController(IUsuarioService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                return View(await _servicio.GetUsuariosAsync());
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return View();
            }
        }

        //Quedó inutilizado por las Data Tables
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetUsuariosFiltrados(string busqueda) 
        {
            try
            {
                var resultado = await _servicio.GetUsuarioByNameOrLastNameAsync(busqueda);
                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetUsuarios");
                }
                return View("GetUsuarios", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalles(int id)
        {
            try
            {
                var resultado = await _servicio.GetUsuarioByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetUsuarios");
                }
                return View(resultado.Value);
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            try
            {
                var resultado = await _servicio.PostUsuarioAsync(usuario);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View(usuario);
                }
                TempData["Exito"] = "Usuario creado correctamente";
                return RedirectToAction("GetUsuarios");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var resultado = await _servicio.GetUsuarioByIdAsync(id);
                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetUsuarios");
                }
                return View("Edit", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            Result<Usuario> resultado;
            try
            {
                //Código para obtención del id del usuario logeado desde los claims
                var claimUsuarioLogueado = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claimUsuarioLogueado == null)
                {
                    TempData["Errores"] = "No se pudo identificar al usuario actual";
                    return RedirectToAction("GetUsuarios");
                }
                int idUsuarioActual = int.Parse(claimUsuarioLogueado.Value);

                resultado = await _servicio.PutUsuarioAsync(usuario, idUsuarioActual);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View(usuario);
                }
                TempData["Exito"] = "Usuario editado correctamente";
                return RedirectToAction("GetUsuarios");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                //Se pasa el id del usuario logeado actual para evitar que se elimine a el mismo en el servicio
                var claimUsuarioLogueado = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claimUsuarioLogueado == null) 
                {
                    TempData["Errores"] = "No se pudo identificar al usuario actual";
                    return RedirectToAction("GetUsuarios");
                }
                int idUsuarioActual = int.Parse(claimUsuarioLogueado.Value);

                var resultado = await _servicio.DeleteUsuarioByIdAsync(id, idUsuarioActual);
                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetUsuarios");
                }

                TempData["Exito"] = "Usuario eliminado correctamente";
                return RedirectToAction("GetUsuarios");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetUsuarios");
            }
        }
    }
}