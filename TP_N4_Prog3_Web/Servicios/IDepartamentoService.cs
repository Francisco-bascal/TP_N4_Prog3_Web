using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public interface IDepartamentoService
    {
        Task<IEnumerable<Departamento>> GetDepartamentosAsync();
        Task<Result<Departamento>> GetDepartamentoByIdAsync(int id);
        Task<Result<IEnumerable<Departamento>>> GetDepartamentosByNameOrIdAsync(string busqueda);
        Task<Result<Departamento>> PostDepartamentoAsync(Departamento departamento);
        Task<Result<Departamento>> PutDepartamentoAsync(Departamento departamento);
        Task<Result<Departamento>> DeleteDepartamentoByIdAsync(int id);
    }
}
