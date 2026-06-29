using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Repositorios;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IDepartamentoRepository _repositorio;
        public DepartamentoService(IDepartamentoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        //El GetAll no requiere de ninguna validación como servicio por ende es el único que no implementa Result Pattern
        public async Task<IEnumerable<Departamento>> GetDepartamentosAsync()
        {
             return await _repositorio.GetDepartamentosAsync();
        }
        public async Task<Result<Departamento>> GetDepartamentoByIdAsync(int id) 
        {
            var resultado = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);
            if (!resultado.IsValid) return Result<Departamento>.Failure(resultado.Errors);

            var existe = await _repositorio.GetDepartamentoByIdAsync(id);
            if (existe == null) return Result<Departamento>.Failure($"No existe el departamento con el id: {id}");

            return Result<Departamento>.Success(existe);
        }

        //Quedó inutilizado por las Data Tables
        public async Task<Result<IEnumerable<Departamento>>> GetDepartamentosByNameOrIdAsync(string busqueda) 
        {
            if (string.IsNullOrWhiteSpace(busqueda)) return Result<IEnumerable<Departamento>>.Failure("El filtro de búsqueda no puede estar vacío");
            if (busqueda.Length > 50) return Result<IEnumerable<Departamento>>.Failure("El filtro de búsqueda no puede superar los 50 caracteres");

            var departamentos = await _repositorio.GetDepartamentoByNameOrId(busqueda);
            return Result<IEnumerable<Departamento>>.Success(departamentos);
        }
        public async Task<Result<Departamento>> PostDepartamentoAsync(Departamento departamento) 
        {
            var resultado = ValidationService.Validar<Departamento>(departamento, ValidationService.ValidarModeloDepartamento);
            if (!resultado.IsValid) return resultado;

            await _repositorio.PostDepartamentoAsync(departamento);
            return Result<Departamento>.Success(departamento);
        }
        public async Task<Result<Departamento>> PutDepartamentoAsync(Departamento departamento) 
        {
            var resultado = ValidationService.Validar<Departamento>(departamento, ValidationService.ValidarModeloDepartamento);
            if (!resultado.IsValid) return resultado;

            var existe = await _repositorio.GetDepartamentoByIdAsync(departamento.IdDepartamento);
            if (existe == null) return Result<Departamento>.Failure($"El departamento con el id: {departamento.IdDepartamento} no existe");

            await _repositorio.PutDepartamentoAsync(departamento);
            return Result<Departamento>.Success(departamento);
        }
        public async Task<Result<Departamento>> DeleteDepartamentoByIdAsync(int id) 
        {
            var resultadoId = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);
            if (!resultadoId.IsValid) return Result<Departamento>.Failure(resultadoId.Errors);

            var existe = await _repositorio.GetDepartamentoByIdAsync(id);
            if (existe == null) return Result<Departamento>.Failure($"No existe el departamento con el id: {id}");

            if (existe.Empleados.Count() > 0) return Result<Departamento>.Failure("No se puede eliminar un departamento con empleados asociados");

            await _repositorio.DeleteDepartamentoByIdAsync(id);
            return Result<Departamento>.Success(existe);
        }
    }
}