using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios.Validation;
using TP_N4_Prog3_Web.DTOs;
using TP_N4_Prog3_Web.ViewModels;

namespace APP_PRUEBA_1.Servicios
{
    public interface IReporteService
    {
        Task<Result<IEnumerable<EmpleadosPorDepartamentoVM>>> GetEmpleadosPorDepartamentoAsync();
        Task<Result<IEnumerable<EmpleadosAgrupadosPorDepartamentoVM>>> GetEmpleadosAgrupadosPorDepartamentoAsync();
        Task<Result<IEnumerable<Empleado>>> GetEmpleadosReporteFiltros(FiltroEmpleadoDTO filtro);
        Task<Result<IEnumerable<Departamento>>> GetDepartamentosAsync();
        Task<Result<EmpleadosPorCursoVM>> GetEmpleadosPorCursoAsync(int idCurso);
        Task<Result<IEnumerable<Curso>>> GetCursosAsync();
    }
}
