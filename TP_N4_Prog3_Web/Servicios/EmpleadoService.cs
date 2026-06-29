using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Repositorios;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repositorio;
        public EmpleadoService(IEmpleadoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        //El GetAll no requiere de ninguna validación como servicio por ende es el único que no implementa Result Pattern
        public async Task<ICollection<Empleado>> GetEmpleadosAsync() 
        {
            return await _repositorio.GetEmpleadosAsync();
        }
        public async Task<Result<Empleado>> GetEmpleadoByIdAsync(int id) 
        {
            var resultadoValidacionId = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);

            if (!resultadoValidacionId.IsValid) return Result<Empleado>.Failure(resultadoValidacionId.Errors);

            var empleado = await _repositorio.GetEmpleadoByIdAsync(id);

            if (empleado == null) return Result<Empleado>.Failure("Empleado no encontrado");

            return Result<Empleado>.Success(empleado);
        }

        //Quedó inutilizado por las Data Tables
        //Servicio para filtro de búsqueda ↨
        public async Task<Result<ICollection<Empleado>>> GetEmpleadosFiltradosAsync(string? busqueda, int? departamentoId) 
        {
            var empleados = await _repositorio.GetEmpleadosFiltradosAsync(busqueda, departamentoId);
            return Result<ICollection<Empleado>>.Success(empleados);
        }
        //Servicio para filtro de búsqueda ↨

        public async Task<Result<Empleado>> PutEmpleadoAsync(Empleado empleado) 
        {
            var resValEmpleado = ValidationService.Validar<Empleado>(empleado, ValidationService.ValidarModeloEmpleado);
            if (!resValEmpleado.IsValid) return resValEmpleado;

            var empleados = await _repositorio.GetEmpleadosAsync();
            if (empleados.Any(e => e.Dni.Equals(empleado.Dni) && !e.IdEmpleado.Equals(empleado.IdEmpleado))) //para que no se detecte a sí mismo como duplicado
                return Result<Empleado>.Failure($"Ya existe un empleado con el DNI: {empleado.Dni}");

            var existe = await _repositorio.GetEmpleadoByIdAsync(empleado.IdEmpleado);
            if (existe == null) return Result<Empleado>.Failure($"No existe el empleado con el id {empleado.IdEmpleado}");

            await _repositorio.PutEmpleadoAsync(empleado);
            return Result<Empleado>.Success(empleado);
        }

        public async Task<Result<Empleado>> DeleteEmpleadoAsync(int id) 
        {
            var resultadoValidacionId = ValidationService.Validar<int>(id, ValidationService.IdMayorCero);
            if (!resultadoValidacionId.IsValid) return Result<Empleado>.Failure("El id del empleado no puede ser menor o igual a 0");

            var existe = await _repositorio.GetEmpleadoByIdAsync(id);
            if (existe == null) return Result<Empleado>.Failure($"No existe el empleado con el id {id}");

            if (existe.Estado.Equals(true)) return Result<Empleado>.Failure("No se puede eliminar un empleado con estado activo");

            if (existe.FechaIngreso < (DateOnly.FromDateTime(DateTime.Now).AddYears(-5))) return Result<Empleado>.Failure("No se puede eliminar un empleado con más de 5 años de antigüedad");

            await _repositorio.DeleteEmpleadoByIdAsync(id);
            return Result<Empleado>.Success(existe);
        }

        public async Task<Result<Empleado>> PostEmpleadoAsync(Empleado empleado) 
        {
            var resValEmpleado = ValidationService.Validar<Empleado>(empleado, ValidationService.ValidarModeloEmpleadoPost); //No se puede usar el de validación de modelo normal, porque salta la validación del id, dado que el mismo se asigna al llegar a la base de datos no antes
            if (!resValEmpleado.IsValid) return resValEmpleado;

            var empleados = await _repositorio.GetEmpleadosAsync();
            if (empleados.Any(e => e.Dni.Equals(empleado.Dni)))
                return Result<Empleado>.Failure($"Ya existe un empleado con el DNI: {empleado.Dni}");

            await _repositorio.PostEmpleadoAsync(empleado);
            return Result<Empleado>.Success(empleado);
        }
    }
}

/*
  public async Task<Result<ICollection<Empleado>>> GetEmpleadosFiltradosAsync(string? busqueda, int? departamentoId) 
        {
            if (string.IsNullOrWhiteSpace(busqueda)) return Result<ICollection<Empleado>>.Failure("El filtro de búsqueda no puede estar vacío");
            if (busqueda.Length > 50) return Result<ICollection<Empleado>>.Failure("La búsqueda de un empleado no puede superar los 50 caracteres");
         
            var empleados = await _repositorio.GetEmpleadosByNameOrLastNameAsync(busqueda);
            return Result<ICollection<Empleado>>.Success(empleados);
        }
 */