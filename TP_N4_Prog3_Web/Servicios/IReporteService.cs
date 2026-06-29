using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios.Validation;
using TP_N4_Prog3_Web.DTOs;
using TP_N4_Prog3_Web.ViewModels;

namespace TP_N4_Prog3_Web.Servicios
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
