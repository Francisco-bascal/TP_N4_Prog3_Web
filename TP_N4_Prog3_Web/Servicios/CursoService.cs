using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Repositorios;
using APP_PRUEBA_1.Servicios.Validation;
using Microsoft.AspNetCore.Authorization;

namespace APP_PRUEBA_1.Servicios
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _repositorio;
        public CursoService(ICursoRepository repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<ICollection<Curso>> GetCursosAsync() 
        {
            return await _repositorio.GetCursosAsync();
        }
        public async Task<Result<Curso>> GetCursoByIdAsync(int id) 
        {
            var resultado = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);
            if (!resultado.IsValid) return Result<Curso>.Failure("El id del curso no puede ser igual o menor que 0");

            var existe = await _repositorio.GetCursoByIdAsync(id);
            if (existe == null) return Result<Curso>.Failure($"No existe el curso con el id: {id}");

            return Result<Curso>.Success(existe);
        }

        //Quedó inutilizado por las Data Tables
        public async Task<Result<IEnumerable<Curso>>> GetCursosByNameAsync(string? busqueda) 
        {
            var existe = await _repositorio.GetCursosByNameAsync(busqueda);
            //if (existe == null) return Result<ICollection<Curso>>.Failure("Ningún curso cumple con el criterio de búsqueda");
            return Result<IEnumerable<Curso>>.Success(existe);
        }
        public async Task<Result<Curso>> PostCursoAsync(Curso curso) 
        {
            var resultado = ValidationService.Validar<Curso>(curso, ValidationService.ValidarModeloCurso);
            if (!resultado.IsValid) return resultado;

            await _repositorio.PostCursoAsync(curso);
            return resultado;
        }
        public async Task<Result<Curso>> PutCursoAsync(Curso curso) 
        {
            var resultado = ValidationService.Validar<Curso>(curso, ValidationService.ValidarModeloCurso);
            if (!resultado.IsValid) return resultado;

            await _repositorio.PutCursoAsync(curso);
            return resultado;
        }
        public async Task<Result<Curso>> DeleteCursoByIdAsync(int id) 
        {
            var resultado = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);
            if (!resultado.IsValid) return Result<Curso>.Failure("El id del curso no puede ser igual o menor que 0");

            var existe = await _repositorio.GetCursoByIdAsync(id);
            if (existe == null) return Result<Curso>.Failure($"No existe el curso con el id: {id}");

            //previsional
            if (existe.IdEmpleados.Any()) return Result<Curso>.Failure("No se puede eliminar un curso que tenga empleados asignados");

            await _repositorio.DeleteCursoByIdAsync(id);
            return Result<Curso>.Success(existe);
        }
    }
}