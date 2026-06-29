using APP_PRUEBA_1.Models;

namespace APP_PRUEBA_1.Repositorios
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
