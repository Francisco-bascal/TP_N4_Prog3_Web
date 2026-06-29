using TP_N4_Prog3_Web.Models;
using TP_N4_Prog3_Web.Servicios.Validation;

namespace TP_N4_Prog3_Web.Servicios
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
