using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public interface ICursoService
    {
        Task<ICollection<Curso>> GetCursosAsync();
        Task<Result<Curso>> GetCursoByIdAsync(int id);
        Task<Result<IEnumerable<Curso>>> GetCursosByNameAsync(string? busqueda);
        Task<Result<Curso>> PostCursoAsync(Curso curso);
        Task<Result<Curso>> PutCursoAsync(Curso curso);
        Task<Result<Curso>> DeleteCursoByIdAsync(int id);
    }
}