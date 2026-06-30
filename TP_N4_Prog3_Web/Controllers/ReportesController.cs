using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios;
using TP_N4_Prog3_Web.Servicios.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP_N4_Prog3_Web.DTOs;
using TP_N4_Prog3_Web.ViewModels;

//Decidir si operador tiene acceso a los reportes.

namespace TP_N4_Prog3_Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReporteService _servicio;
        public ReportesController(IReporteService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Reportes() 
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EmpleadosPorDepartamento() 
        {
            Result<IEnumerable<EmpleadosPorDepartamentoVM>> resultado;
            try
            {
                resultado = await _servicio.GetEmpleadosPorDepartamentoAsync();

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("Reportes");
                }

                return View(resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("Reportes");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EmpleadosAgrupadosPorDepartamento() 
        {
            Result<IEnumerable<EmpleadosAgrupadosPorDepartamentoVM>> resultado;
            try
            {
                resultado = await _servicio.GetEmpleadosAgrupadosPorDepartamentoAsync();

                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("Reportes");
                }

                return View(resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("Reportes");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReporteFiltroEspecializado(FiltroEmpleadoDTO filtros) 
        {
            Result<IEnumerable<Empleado>> resultado;
            Result<IEnumerable<Departamento>> departamentos; //nunca lanza Failure, si puede detonar excepción
            try
            {
                resultado = await _servicio.GetEmpleadosReporteFiltros(filtros);
                departamentos = await _servicio.GetDepartamentosAsync(); //Para cargar los departamentos para los filtros

                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("ReporteFiltroEspecializado");
                }

                var retornoEmpleados = new ReporteConFiltrosEmpleadoVM
                {
                    Filtros = filtros,
                    Empleados = resultado.Value,
                    Departamentos = departamentos.Value
                };

                return View(retornoEmpleados);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("Reportes");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ReporteEmpleadosPorCurso(int? idCurso) 
        {
            Result<EmpleadosPorCursoVM> resultado;
            try
            {
                //Se pasan los cursos para el SelectList de la generación de reportes
                ViewBag.Cursos = (await _servicio.GetCursosAsync()).Value;

                //Si no hay un curso seleccionado para la generación del reporte se hace un early return a la vista vacía
                if (!idCurso.HasValue)
                    return View();

                resultado = await _servicio.GetEmpleadosPorCursoAsync(idCurso.Value);
                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("Reportes"); 
                }

                return View(resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("Reportes");
            }
        }
    }
}