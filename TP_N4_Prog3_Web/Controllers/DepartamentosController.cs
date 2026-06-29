using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios;
using APP_PRUEBA_1.Servicios.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace APP_PRUEBA_1.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IDepartamentoService _servicio;
        public DepartamentosController(IDepartamentoService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetDepartamentosAsync() 
        {
            try
            {
                var departamentos = await _servicio.GetDepartamentosAsync();
                return View(departamentos);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetDepartamentoByIdAsync(int id) 
        {
            Result<Departamento> resultado;
            try
            {
                resultado = await _servicio.GetDepartamentoByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetDepartamentos");
                }
                return View("Detalles", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetDepartamentos");
            }
        }

        //Quedó inutilizado por las Data Tables
        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> GetDepartamentoByNameOrIdAsync(string busqueda) 
        {
            Result<IEnumerable<Departamento>> resultado;
            try
            {
                resultado = await _servicio.GetDepartamentosByNameOrIdAsync(busqueda);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetDepartamentos");
                }
                return View("GetDepartamentos", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetDepartamentos");
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
        public async Task<IActionResult> PostDepartamentoAsync(Departamento departamento) 
        {
            Result<Departamento> resultado;
            try
            {
                resultado = await _servicio.PostDepartamentoAsync(departamento);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View("Create", departamento);
                }
                TempData["Exito"] = "Departamento creado exitosamente";
                return RedirectToAction("GetDepartamentos");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View("Create", departamento);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(int id) 
        {
            Result<Departamento> resultado;
            try
            {
                resultado = await _servicio.GetDepartamentoByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetDepartamentos");
                }
                return View(resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetDepartamentos");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(Departamento departamento) 
        {
            Result<Departamento> resultado;
            try
            {
                resultado = await _servicio.PutDepartamentoAsync(departamento);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View(departamento);
                }
                TempData["Exito"] = "Departamento editado exitosamente";
                return RedirectToAction("GetDepartamentos");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View(departamento);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteDepartamentoAsync(int id) 
        {
            Result<Departamento> resultado;
            try
            {
                resultado = await _servicio.DeleteDepartamentoByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetDepartamentos");
                }
                TempData["Exito"] = "Departamento eliminado exitosamente";
                return RedirectToAction("GetDepartamentos");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetDepartamentos");
            }
        }
    }
}