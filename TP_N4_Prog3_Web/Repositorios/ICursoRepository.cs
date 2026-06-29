using TP_N4_Prog3_Web.Models;

namespace TP_N4_Prog3_Web.Repositorios
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
