using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios.Validation;

namespace TP_N4_Prog3_Web.Servicios
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
