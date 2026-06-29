using TP_N4_Prog3_Web.Models;

namespace TP_N4_Prog3_Web.Repositorios
{
    public interface IUsuarioRepository
    {
        Task<ICollection<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task<Usuario?> GetUsuarioByCredencialesAsync(string nombreUsuario, string contraseña);
        Task<Usuario?> GetUsuarioByNameAsync(string nombre);
        Task<ICollection<Usuario>> GetUsuarioByNameOrLastNameAsync(string busqueda);
        Task PostUsuarioAsync(Usuario usuario);
        Task PutUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioByIdAsync(int id);
    }
}
