using APP_PRUEBA_1.Models;
using TP_N4_Prog3_Web.DTOs;

namespace APP_PRUEBA_1.Repositorios
{
    public interface IEmpleadoRepository
    {
        Task<ICollection<Empleado>> GetEmpleadosAsync();
        Task<Empleado?> GetEmpleadoByIdAsync(int id);
        Task<ICollection<Empleado>> GetEmpleadosFiltradosAsync(string? busqueda, int? departamentoId);
        Task PostEmpleadoAsync(Empleado empleado);
        Task PutEmpleadoAsync(Empleado empleado);
        Task DeleteEmpleadoByIdAsync(int id);
        Task DeleteEmpleadoAsync(Empleado empleado);
        Task<IEnumerable<Empleado>> GetEmpleadosConFiltroReportes(FiltroEmpleadoDTO filtros);
    }
}