using APP_PRUEBA_1.Repositorios;
using APP_PRUEBA_1.Servicios.Validation;
using APP_PRUEBA_1.Models;
using TP_N4_Prog3_Web.DTOs;
using TP_N4_Prog3_Web.ViewModels;

namespace APP_PRUEBA_1.Servicios
{
    public class ReporteService : IReporteService
    {
        private readonly IEmpleadoRepository _repo;
        private readonly IDepartamentoRepository _repoDepartamento;
        private readonly ICursoRepository _repoCurso;
        public ReporteService(IEmpleadoRepository repo, IDepartamentoRepository repoDepartamento, ICursoRepository repoCurso)
        {
            _repo = repo;
            _repoDepartamento = repoDepartamento;
            _repoCurso = repoCurso;
        }

        public async Task<Result<IEnumerable<EmpleadosPorDepartamentoVM>>> GetEmpleadosPorDepartamentoAsync()
        {
            var empleadosDesignar = await _repo.GetEmpleadosAsync();

            var empleadosRetornar = empleadosDesignar.GroupBy(e => e.IdDepartamentoNavigation.Nombre)
                .Select(g => new EmpleadosPorDepartamentoVM
                {
                    Departamento = g.Key, //el nombre del departamento
                    CantidadEmpleados = g.Count() //cantidad de empleados que tiene

                });

            return Result<IEnumerable<EmpleadosPorDepartamentoVM>>.Success(empleadosRetornar);
        }

        public async Task<Result<IEnumerable<EmpleadosAgrupadosPorDepartamentoVM>>> GetEmpleadosAgrupadosPorDepartamentoAsync()
        {
            var empleadosDesignar = await _repo.GetEmpleadosAsync();
            var empleadosRetornar = empleadosDesignar.GroupBy(e => e.IdDepartamentoNavigation.Nombre).Select(g => new EmpleadosAgrupadosPorDepartamentoVM
            {
                NombreDepartamento = g.Key, //Nombre del departamento
                Empleados = g.OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToList() //Lista de empleados ordenados por apellido y por nombre
            })
            .OrderBy(g => g.NombreDepartamento).ToList(); //finalmente se ordena por nombre de departamento

            return Result<IEnumerable<EmpleadosAgrupadosPorDepartamentoVM>>.Success(empleadosRetornar);
        }

        public async Task<Result<IEnumerable<Empleado>>> GetEmpleadosReporteFiltros(FiltroEmpleadoDTO filtro)
        {
            if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
            {
                if (filtro.Busqueda.Trim().Length <= 2)
                    return Result<IEnumerable<Empleado>>.Failure("El filtro de búsqueda debe contener al menos 3 caracteres");
            }

            if (filtro.FechaIngresoHasta.HasValue)
            {
                if (filtro.FechaIngresoHasta.Value > DateOnly.FromDateTime(DateTime.Now))
                    return Result<IEnumerable<Empleado>>.Failure("La fecha de filtrado \"Hasta\" no puede ser superior a la fecha actual");
            }

            if (filtro.FechaIngresoDesde.HasValue)
            {
                if (filtro.FechaIngresoDesde.Value > DateOnly.FromDateTime(DateTime.Now))
                    return Result<IEnumerable<Empleado>>.Failure("La fecha de filtrado \"Desde\" no puede ser superior a la fecha actual");
            }

            if (filtro.IdDepartamento.HasValue)
            {
                if (filtro.IdDepartamento.Value <= 0)
                    return Result<IEnumerable<Empleado>>.Failure("El id del departamento no puede ser menor que 1");
            }

            if (filtro.CantidadHijosMax.HasValue)
            {
                if (filtro.CantidadHijosMax.Value < 0)
                    return Result<IEnumerable<Empleado>>.Failure("La cantidad máxima de hijos no puede ser menor que 0");
            }

            if (filtro.CantidadHijosMin.HasValue)
            {
                if (filtro.CantidadHijosMin.Value < 0)
                    return Result<IEnumerable<Empleado>>.Failure("La cantidad mínima de hijos no puede ser menor que 0");
            }

            if (filtro.CantidadHijosMin.HasValue && filtro.CantidadHijosMax.HasValue)
            {
                if (filtro.CantidadHijosMax.Value < filtro.CantidadHijosMin.Value)
                    return Result<IEnumerable<Empleado>>.Failure("El filtro de límite superior de hijos no puede ser inferior al filtro de límite inferior de hijos");
            }

            if (filtro.FechaIngresoDesde.HasValue && filtro.FechaIngresoHasta.HasValue)
            {
                if (filtro.FechaIngresoHasta.Value < filtro.FechaIngresoDesde.Value)
                    return Result<IEnumerable<Empleado>>.Failure("La fecha de filtrado \"hasta\" no puede ser menor que la \"desde\"");
            }

            var empleados = await _repo.GetEmpleadosConFiltroReportes(filtro);
            return Result<IEnumerable<Empleado>>.Success(empleados);
        }

        //Se usa para llenar select lists de filtros de validación
        public async Task<Result<IEnumerable<Departamento>>> GetDepartamentosAsync()
        {
            var departamentos = await _repoDepartamento.GetDepartamentosAsync();
            return Result<IEnumerable<Departamento>>.Success(departamentos);
        }

        public async Task<Result<EmpleadosPorCursoVM>> GetEmpleadosPorCursoAsync(int idCurso) 
        {
            var curso = await _repoCurso.GetCursoByIdAsync(idCurso);

            if (curso == null)
                return Result<EmpleadosPorCursoVM>.Failure("Curso inexistente");

            var resultado = new EmpleadosPorCursoVM
            {
                NombreCurso = curso.NombreCurso,
                Empleados = curso.IdEmpleados
            };

            return Result<EmpleadosPorCursoVM>.Success(resultado);
        }

        public async Task<Result<IEnumerable<Curso>>> GetCursosAsync() 
        {
            return Result<IEnumerable<Curso>>.Success(await _repoCurso.GetCursosAsync());
        }
    }
}