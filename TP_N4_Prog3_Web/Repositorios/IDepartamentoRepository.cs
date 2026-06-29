using APP_PRUEBA_1.Models;

namespace APP_PRUEBA_1.Repositorios
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