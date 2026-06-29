using APP_PRUEBA_1.Models;
using System.ComponentModel.DataAnnotations;

namespace APP_PRUEBA_1.Servicios.Validation
{
    //Para las validaciones de modelo se usa una combinación de Data Annotations con Result Pattern.
    //Dado que para propiedades del modelo se considera que Data Annot es la "fuente única de verdad” del modelo.
    public class ValidationService 
    {
        public static Result<T> Validar<T>(T entidad, Func<T, IEnumerable<string>> funcionValidacion)
        {
            IEnumerable<string> errores = funcionValidacion(entidad); //se almacenan los errores de la entidad que maneja la función de validación

            if (errores.Any())
                return Result<T>.Failure(errores); //si tiene errores se retorna el método falla, que hace el resultado inválido y almacena los errores

            return Result<T>.Success(entidad); //si no tiene errores retorna éxito, que hace al resultado válido sin errores en su lista.
        }

        public static IEnumerable<String> IdMayorCero(int id) 
        {
            var errores = new List<string>();

            if (id <= 0)
                errores.Add("El id de un registro no puede ser igual o menor que 0");

            return errores;
        }

        public static IEnumerable<string> ValidarModeloEmpleado(Empleado empleado) 
        {
            var errores = new List<string>(); //Retorno de la función, si no se llena se retorna vacío y significa que no hubo errores de validación

            var resultado = new List<ValidationResult>();
            var contexto = new ValidationContext(empleado);
            bool esValido = Validator.TryValidateObject(empleado, contexto, resultado, true);

            if (empleado.IdEmpleado <= 0)
                errores.Add("El id de un empleado no puede ser igual o menor que 0");

            if (empleado.FechaIngreso > DateOnly.FromDateTime(DateTime.Today))
                errores.Add("Fecha de ingreso inválida");

            if (empleado.IdDepartamento <= 0)
                errores.Add("El id de un departamento no puede ser igual o menor que 0");

            if (empleado.CantidadHijos < 0)
                errores.Add("El empleado no puede tener una cantidad de hijos negativa");

            if (!esValido) errores = AppendValidationErrores(errores, resultado).ToList(); //junta todos los errores de validación

            return errores;
        }

        public static IEnumerable<string> ValidarModeloEmpleadoPost(Empleado empleado)
        {
            var errores = new List<string>(); //Retorno de la función, si no se llena se retorna vacío y significa que no hubo errores de validación

            var resultado = new List<ValidationResult>();
            var contexto = new ValidationContext(empleado);
            bool esValido = Validator.TryValidateObject(empleado, contexto, resultado, true);

            if (empleado.FechaIngreso > DateOnly.FromDateTime(DateTime.Today))
                errores.Add("Fecha de ingreso inválida");

            if (empleado.IdDepartamento <= 0)
                errores.Add("El id de un departamento no puede ser igual o menor que 0");

            if (empleado.CantidadHijos < 0)
                errores.Add("El empleado no puede tener una cantidad de hijos negativa");

            if (empleado.Dni < 0)
                errores.Add("El DNI del empleado no puede ser menor a 0");

            if (empleado.Dni > 70000000)
                errores.Add("El DNI del empleado no puede ser superior a 70.000.000");

            if (!esValido) errores = AppendValidationErrores(errores, resultado).ToList(); //junta todos los errores de validación

            return errores;
        }

        public static IEnumerable<String> ValidarModeloDepartamento(Departamento departamento) 
        {
            var errores = new List<string>();

            var resultado = new List<ValidationResult>();
            var contexto = new ValidationContext(departamento);
            bool esValido = Validator.TryValidateObject(departamento, contexto, resultado, true);

            if (!esValido) errores = AppendValidationErrores(errores, resultado).ToList();

            return errores;
        }

        public static IEnumerable<string> ValidarModeloCurso(Curso curso) 
        {
            var errores = new List<string>();

            var resultado = new List<ValidationResult>();
            var context = new ValidationContext(curso);
            bool esValido = Validator.TryValidateObject(curso, context, resultado, true);

            if (curso.Horas <= 0) errores.Add("La cantidad de horas del curso no puede ser igual o menor que 0");

            if (curso.Horas > 500) errores.Add("La cantidad de horas del curso no puede ser superior a 500");

            if (curso.FechaInicio > DateOnly.FromDateTime(DateTime.Now)) errores.Add("La fecha de inicio no puede ser posterior a la fecha actual");

            if (curso.FechaInicio < DateOnly.FromDateTime(DateTime.Now).AddYears(-3)) errores.Add("Ningún curso dura más de 3 años");

            if (!esValido) errores = AppendValidationErrores(errores, resultado).ToList();

            return errores;
        }

        public static IEnumerable<string> AppendValidationErrores(List<string> erroresResultFuente, List<ValidationResult> erroresDataFuente) 
        {
            var erroresData = erroresDataFuente.Where(r => r.ErrorMessage != null).Select(r => r.ErrorMessage ?? "Error de validación").ToList(); //Extraemos los errores de data anotations

            erroresResultFuente.AddRange(erroresData);

            return erroresResultFuente;
        }
    }
}

/*
        //Función de validación de ejemplo
        public static IEnumerable<string> ValidarEnteroPositivo(int entero)
        {
            var errores = new List<string>();

            if (entero < 0)
                errores.Add("Debe ser un entero positivo");

            return errores;
        } 
*/

/* Función de validación de Result Pattern que hace las validaciones hardcodeadas
 public static IEnumerable<string> ValidarModeloEmpleado(Empleados empleado) 
        {
            var errores = new List<string>();

            if (empleado.IdEmpleado <= 0)
                errores.Add("El id de un empleado no puede ser igual o menor que 0");

            if (String.IsNullOrWhiteSpace(empleado.Nombre))
                errores.Add("El nombre del empleado no puede estar vacío");

            if (String.IsNullOrWhiteSpace(empleado.Apellido))
                errores.Add("El apellido del empleado no puede estar vacío");

            if (!string.IsNullOrWhiteSpace(empleado.Nombre) && empleado.Nombre.Length > 50)
                errores.Add("El nombre del empleado no puede superar los 50 caracteres");

            if (!string.IsNullOrWhiteSpace(empleado.Apellido) && empleado.Apellido.Length > 50)
                errores.Add("El apellido del empleado no puede superar los 50 caracteres");

            if (empleado.FechaIngreso > DateOnly.FromDateTime(DateTime.Today))
                errores.Add("Fecha de ingreso inválida");

            if(empleado.IdDepartamento <= 0)
                errores.Add("El id de un departamento no puede ser igual o menor que 0");

            return errores;
        }
*/