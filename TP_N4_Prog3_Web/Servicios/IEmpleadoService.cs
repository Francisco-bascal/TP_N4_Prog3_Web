using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios.Validation;

namespace TP_N4_Prog3_Web.Servicios
{
    public interface IEmpleadoService
    {
        Task<ICollection<Empleado>> GetEmpleadosAsync();
        Task<Result<Empleado>> GetEmpleadoByIdAsync(int id);
        Task<Result<ICollection<Empleado>>> GetEmpleadosFiltradosAsync(string? busqueda, int? departamentoId);
        Task<Result<Empleado>> PutEmpleadoAsync(Empleado empleado);
        Task<Result<Empleado>> PostEmpleadoAsync(Empleado empleado);
        Task<Result<Empleado>> DeleteEmpleadoAsync(int id);
    }
}