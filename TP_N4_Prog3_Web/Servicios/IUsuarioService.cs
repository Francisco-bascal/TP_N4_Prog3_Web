using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public interface IUsuarioService
    {
        Task<ICollection<Usuario>> GetUsuariosAsync();
        Task<Result<Usuario>> GetUsuarioByIdAsync(int id);
        Task<Result<Usuario>> GetUsuarioByCredencialesAsync(string nombreUsuario, string contraseña);
        Task<Result<ICollection<Usuario>>> GetUsuarioByNameOrLastNameAsync(string? busqueda);
        Task<Result<Usuario>> PostUsuarioAsync(Usuario usuario);
        Task<Result<Usuario>> PutUsuarioAsync(Usuario usuario, int idUsuarioActual);
        Task<Result<Usuario>> DeleteUsuarioByIdAsync(int id, int idUsuarioActual);
    }
}
