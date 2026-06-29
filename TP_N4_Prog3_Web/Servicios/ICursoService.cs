using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios.Validation;

namespace TP_N4_Prog3_Web.Servicios
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