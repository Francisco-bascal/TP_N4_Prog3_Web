using APP_PRUEBA_1.Models;
using APP_PRUEBA_1.Repositorios;
using APP_PRUEBA_1.Servicios.Validation;

namespace APP_PRUEBA_1.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repositorio;
        public UsuarioService(IUsuarioRepository repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<ICollection<Usuario>> GetUsuariosAsync() 
        {
            return await _repositorio.GetUsuariosAsync();
        }
        public async Task<Result<Usuario>> GetUsuarioByIdAsync(int id) 
        {
            var existe = await _repositorio.GetUsuarioByIdAsync(id);

            if (existe == null) return Result<Usuario>.Failure($"El usuario con el id: {id} no existe");

            return Result<Usuario>.Success(existe);
        }

        //Se usa para el login
        public async Task<Result<Usuario>> GetUsuarioByCredencialesAsync(string nombreUsuario, string contraseña) 
        {
            var existe = await _repositorio.GetUsuarioByCredencialesAsync(nombreUsuario, contraseña);

            if (existe == null) return Result<Usuario>.Failure("El usuario con las credenciales ingresadas no existe");

            return Result<Usuario>.Success(existe);
        }

        //Quedó inutilizado por las Data Tables
        public async Task<Result<ICollection<Usuario>>> GetUsuarioByNameOrLastNameAsync(string? busqueda) 
        {
            var usuarios = await _repositorio.GetUsuarioByNameOrLastNameAsync(busqueda);
            return Result<ICollection<Usuario>>.Success(usuarios);
        }
        public async Task<Result<Usuario>> PostUsuarioAsync(Usuario usuario) 
        {
            var existe = await _repositorio.GetUsuarioByNameAsync(usuario.Nombre);
            
            if (existe != null) return Result<Usuario>.Failure("Ya existe un usuario con este nombre de usuario");
            await _repositorio.PostUsuarioAsync(usuario);
            return Result<Usuario>.Success(usuario);
        }
        public async Task<Result<Usuario>> PutUsuarioAsync(Usuario usuario, int idUsuarioActual) 
        {
            var existe = await _repositorio.GetUsuarioByIdAsync(usuario.IdUsuario); //para verificar existencia
            var usuarios = await _repositorio.GetUsuariosAsync();
            var admins = usuarios.Where(u => u.Rol.Equals("Administrador"));

            if (existe == null) return Result<Usuario>.Failure($"No existe el usuario con el id {usuario.IdUsuario}");

            if (usuario.IdUsuario.Equals(idUsuarioActual)) return Result<Usuario>.Failure("No puedes editar tu propio usuario");

            var verificacionNombre = await _repositorio.GetUsuarioByNameAsync(usuario.Nombre);
            if (verificacionNombre != null && verificacionNombre.IdUsuario != usuario.IdUsuario) return Result<Usuario>.Failure("Ya existe un usuario con este nombre de usuario");

            //Al menos 1 administrador:
            if (admins.Count() == 1 && existe.Rol.Equals("Administrador") && !usuario.Rol.Equals("Administrador")) return Result<Usuario>.Failure("Debe quedar por lo menos 1 administrador en el sistema");
            if (existe.IdUsuario.Equals(1) && !usuario.Rol.Equals("Administrador")) return Result<Usuario>.Failure("No se puede degradar al administrador general del sistema");

            await _repositorio.PutUsuarioAsync(usuario);
            return Result<Usuario>.Success(usuario);
        }

        public async Task<Result<Usuario>> DeleteUsuarioByIdAsync(int id, int idUsuarioActual) 
        {
            var usuarios = await _repositorio.GetUsuariosAsync();
            var admins = usuarios.Where(u => u.Rol.Equals("Administrador"));
            var existe = await _repositorio.GetUsuarioByIdAsync(id);

            if (existe == null) return Result<Usuario>.Failure($"No existe el usuario con el id {id}");

            if (id.Equals(idUsuarioActual)) return Result<Usuario>.Failure("No puedes eliminar tu propio usuario");

            if (admins.Count() == 1 && existe.Rol.Equals("Administrador")) return Result<Usuario>.Failure("No se puede eliminar al único administrador");

            if (existe.IdUsuario.Equals(1)) return Result<Usuario>.Failure("Usuario Protegido: No se puede eliminar al administrador general del sistema");

            //Falta la regla de no autoeliminación
            await _repositorio.DeleteUsuarioByIdAsync(id);
            return Result<Usuario>.Success(existe);
        }
    }
}