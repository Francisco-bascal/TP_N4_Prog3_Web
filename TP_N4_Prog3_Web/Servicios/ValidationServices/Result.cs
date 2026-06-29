namespace APP_PRUEBA_1.Servicios.Validation
{
    public class Result<T>
    {
        public bool IsValid { get; }
        public IEnumerable<string> Errors { get; } = new List<string>();
        public T? Value { get; }

        private Result(T? value, bool valido, IEnumerable<string>? errores = null)
        {
            Value = value;
            Errors = errores ?? new List<string>();
            IsValid = valido;
        }

        public static Result<T> Success(T valor) => new Result<T>(valor, true);
        public static Result<T> Failure(string error) => new Result<T>(default, false, new List<string> { error });
        public static Result<T> Failure(IEnumerable<string> errores) => new Result<T>(default, false, errores);
    }
}