using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APP_PRUEBA_1.Controllers
{
    public class CursosController : Controller
    {
        private readonly ICursoService _servicio;
        public CursosController(ICursoService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetCursosAsync()
        {
            try
            {
                var Cursos = await _servicio.GetCursosAsync();
                return View(Cursos);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetCursoByIdAsync(int id)
        {
            try
            {
                var resultado = await _servicio.GetCursoByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetCursos");
                }
                return View("Detalles", resultado.Value);
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetCursos");
            }
        }

        //Quedó inutilizado por las Data Tables
        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetCursosByNameAsync(string? busqueda)
        {
            try
            {
                var resultado = await _servicio.GetCursosByNameAsync(busqueda);
                return View("GetCursos", resultado.Value);
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetCursos");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> CreateAsync(Curso curso)
        {
            try
            {
                var resultado = await _servicio.PostCursoAsync(curso);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View(curso);
                }

                TempData["Exito"] = "Curso agregado correctamente";
                return RedirectToAction("GetCursos");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return View(curso);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(int id)
        {
            try
            {
                var resultado = await _servicio.GetCursoByIdAsync(id);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetCursos");
                }

                return View(resultado.Value);
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetCursos");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(Curso curso)
        {
            try
            {
                var resultado = await _servicio.PutCursoAsync(curso);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View("Edit", curso);
                }

                TempData["Exito"] = "Curso actualizado correctamente";
                return RedirectToAction("GetCursos");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return View("Edit", curso);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var resultado = await _servicio.DeleteCursoByIdAsync(id);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetCursos");
                }

                TempData["Exito"] = "Curso eliminado correctamente";
                return RedirectToAction("GetCursos");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetCursos");
            }
        }
    }
}