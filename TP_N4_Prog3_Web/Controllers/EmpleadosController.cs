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
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _servicio;
        private readonly IDepartamentoService _servicioDepartamento;
        private readonly ICursoService _servicioCurso;
        public EmpleadosController(IEmpleadoService servicio, IDepartamentoService servicioDepartamento, ICursoService servicioCurso)
        {
            _servicio = servicio;
            _servicioDepartamento = servicioDepartamento;
            _servicioCurso = servicioCurso;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleadosAsync() 
        {
            try
            {
                ViewBag.Departamentos = await _servicioDepartamento.GetDepartamentosAsync(); //para que no se rompa la vista
                var empleados = await _servicio.GetEmpleadosAsync();
                return View(empleados);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleadoByIdAsync(int id) 
        {
            try
            {
                var resultado = await _servicio.GetEmpleadoByIdAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors); //para mostrar los errores en un alert 
                    return RedirectToAction("GetEmpleados");
                }
                return View("Detalles", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View("GetEmpleados");
            } 
        }

        //Quedó inutilizado por las Data Tables
        [HttpGet]
        public async Task<IActionResult> GetEmpleadosFiltro(string? busqueda, int? departamentoId) //se hacen nullables porque pueden no recibir nada si se da enter sin seleccionar algo
        {
            try
            {
                ViewBag.Departamentos = await _servicioDepartamento.GetDepartamentosAsync(); //Para que no se rompa la vista al inicio
                var resultado = await _servicio.GetEmpleadosFiltradosAsync(busqueda, departamentoId);

                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetEmpleados");
                }

                //Valores que se pasan a la vista para hacer los filtros
                ViewBag.Busqueda = busqueda;
                ViewBag.DepartamentoSeleccionado = departamentoId;
                return View("GetEmpleados", resultado.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetEmpleados");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> Create() //para mostrar el formulario de agregación de empleado
        {
            var departamentos = await _servicioDepartamento.GetDepartamentosAsync();
            //Se pasan los departamentos para el filtro de la vista (Esto quedó obsoleto por las Data Tables)
            ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Nombre");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> PostEmpleadoAsync(Empleado empleado) 
        {
            try
            {
                var resultado = await _servicio.PostEmpleadoAsync(empleado);
                if (!resultado.IsValid) 
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return View("Create", empleado);
                }
                TempData["Exito"] = "Empleado creado correctamente";
                return RedirectToAction("GetEmpleados");
            }
            catch (DbUpdateException ex)
            {
                TempData["Errores"] = ex.Message;
                return View("Create", empleado);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return View("Create", empleado);
            }   
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(int id)
        {
            try
            {
                var departamentos = await _servicioDepartamento.GetDepartamentosAsync();
                var resultado = await _servicio.GetEmpleadoByIdAsync(id);

                if (!resultado.IsValid) //no debería fallar al ser un GetById pero se implementa de todos modos por seguridad
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    //Se pasan los departamentos para el filtro de la vista (Esto quedó obsoleto por las Data Tables)
                    ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Nombre", resultado?.Value?.IdDepartamento); //se vuelven a pasar los Departamentos en caso de que la creación no sea válida
                    return RedirectToAction("GetEmpleados");
                }
                ViewBag.Cursos = await _servicioCurso.GetCursosAsync(); //Se pasan los cursos para la selección del checkbox
                //Se pasan los departamentos para el filtro de la vista (Esto quedó obsoleto por las Data Tables)
                ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Nombre", resultado?.Value?.IdDepartamento);
                return View(resultado?.Value);
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetEmpleados");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> EditAsync(Empleado empleado, List<int> CursosSeleccionados)
        {
            try
            {
                //Si no se marca ningún checkbox se envía al repo limpiar la propiedad Muchos a Muchos. 
                //Sino se le envían los cursos a los que está asignado.
                empleado.IdCursos = (CursosSeleccionados ?? new List<int>())
                    .Select(id => new Curso { IdCurso = id }).ToList();

                var resultado = await _servicio.PutEmpleadoAsync(empleado);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    //Se pasan los departamentos para el filtro de la vista (Esto quedó obsoleto por las Data Tables)
                    var departamentos = await _servicioDepartamento.GetDepartamentosAsync();
                    ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Nombre", empleado.IdDepartamento);
                    ViewBag.Cursos = await _servicioCurso.GetCursosAsync(); //Se pasan los cursos para la selección del checkbox

                    return View(empleado);
                }
                TempData["Exito"] = "Empleado editado exitosamente";
                return RedirectToAction("GetEmpleados");
            }
            catch (Exception ex)
            {
                TempData["Errores"] = ex.Message;
                var departamentos = await _servicioDepartamento.GetDepartamentosAsync();
                //Se pasan los departamentos para el filtro de la vista (Esto quedó obsoleto por las Data Tables)
                ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Nombre", empleado.IdDepartamento);
                ViewBag.Cursos = await _servicioCurso.GetCursosAsync(); //Se pasan los cursos para la selección del checkbox
                return View(empleado);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteEmpleadoAsync(int id) 
        {
            try
            {
                var resultado = await _servicio.DeleteEmpleadoAsync(id);
                if (!resultado.IsValid)
                {
                    TempData["Errores"] = string.Join("|", resultado.Errors);
                    return RedirectToAction("GetEmpleados");
                }
                TempData["Exito"] = "Empleado eliminado exitosamente";
                return RedirectToAction("GetEmpleados");
            }
            catch (Exception ex) 
            {
                TempData["Errores"] = ex.Message;
                return RedirectToAction("GetEmpleados");
            }
        }
    }
}