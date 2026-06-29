using APP_PRUEBA_1.Models;

namespace APP_PRUEBA_1.Repositorios
{
    public interface ICursoRepository
    {
        Task<ICollection<Curso>> GetCursosAsync();
        Task<Curso> GetCursoByIdAsync(int id);
        Task<IEnumerable<Curso>> GetCursosByNameAsync(string? busqueda);
        Task PostCursoAsync(Curso curso);
        Task PutCursoAsync(Curso curso);
        Task DeleteCursoByIdAsync(int id);
    }
}
