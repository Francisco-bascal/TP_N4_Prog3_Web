using APP_PRUEBA_1.Models;
using Microsoft.EntityFrameworkCore;

namespace APP_PRUEBA_1.Repositorios
{
    public class CursoRepository : ICursoRepository
    {
        private readonly RRHH2026_Db_Context _contexto;
        public CursoRepository(RRHH2026_Db_Context contexto)
        {
            _contexto = contexto;
        }

        public async Task<ICollection<Curso>> GetCursosAsync() 
        {
            return await _contexto.Cursos.Include(c => c.IdEmpleados).ToListAsync();
        }
        public async Task<Curso> GetCursoByIdAsync(int id) 
        {
            return await _contexto.Cursos.Include(c => c.IdEmpleados).FirstOrDefaultAsync(c => c.IdCurso.Equals(id));
        }

        //Quedó inutilizado por las Data Tables
        public async Task<IEnumerable<Curso>> GetCursosByNameAsync(string? busqueda) 
        {
            IQueryable<Curso> query = _contexto.Cursos;

            if (!String.IsNullOrWhiteSpace(busqueda)) 
                query = query.Where(c => c.NombreCurso.Contains(busqueda));

            return await query.ToListAsync();
        }

        public async Task PostCursoAsync(Curso curso) 
        {
            await _contexto.Cursos.AddAsync(curso);
            await _contexto.SaveChangesAsync();
        }

        public async Task PutCursoAsync(Curso curso) 
        {
            var existe = await _contexto.Cursos.FindAsync(curso.IdCurso);
            existe.NombreCurso = curso.NombreCurso;
            existe.IdEmpleados = curso.IdEmpleados;
            existe.FechaInicio = curso.FechaInicio;
            existe.Horas = curso.Horas;
            await _contexto.SaveChangesAsync();
        }

        public async Task DeleteCursoByIdAsync(int id) 
        {
            var existe = await _contexto.Cursos.FindAsync(id);
            _contexto.Cursos.Remove(existe);
            await _contexto.SaveChangesAsync();
        }
    }
}