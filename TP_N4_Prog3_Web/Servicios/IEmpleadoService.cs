using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
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