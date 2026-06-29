using TP_N4_Prog3_Web.Models;

namespace TP_N4_Prog3_Web.Repositorios
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetDepartamentosAsync();
        Task<Departamento?> GetDepartamentoByIdAsync(int id);
        Task<IEnumerable<Departamento>> GetDepartamentoByNameOrId(string busqueda);
        Task PostDepartamentoAsync(Departamento departamento);
        Task PutDepartamentoAsync(Departamento departamento);
        Task DeleteDepartamentoByIdAsync(int id);
    }
}