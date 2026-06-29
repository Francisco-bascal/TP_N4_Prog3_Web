using APP_PRUEBA_1.Models;
using Microsoft.EntityFrameworkCore;

namespace APP_PRUEBA_1.Repositorios
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly RRHH2026_Db_Context _contexto;
        public DepartamentoRepository(RRHH2026_Db_Context contexto)
        {
            _contexto = contexto;
        }
        public async Task<IEnumerable<Departamento>> GetDepartamentosAsync() 
        {
            return await _contexto.Departamentos.ToListAsync();
        }
        public async Task<Departamento?> GetDepartamentoByIdAsync(int id) 
        {
            var departamento = await _contexto.Departamentos.Include(d => d.Empleados).FirstOrDefaultAsync(d => d.IdDepartamento.Equals(id));
            return departamento;
        }

        //Quedó inutilizado por las Data Tables
        public async Task<IEnumerable<Departamento>> GetDepartamentoByNameOrId(string busqueda) 
        {
            //var departamentos = await _contexto.Departamentos.Where(d => d.Nombre.Contains(busqueda)).ToListAsync();
            var query = _contexto.Departamentos.AsQueryable(); //almacenamos el inicio de la query

            if (int.TryParse(busqueda, out int id)) //Si el string "busqueda" se puede parsear a entero => buscamos departamentos cuyo id coincida con la misma
            {
                query = query.Where(d => d.IdDepartamento.Equals(id));
            }
            else //sino => solo buscamos los departamentos cuyo nombre contenga el string de la búsqueda
            {
                query = query.Where(d => d.Nombre.Contains(busqueda));
            }

            var departamentos = await query.ToListAsync();
            return departamentos;
        }
        public async Task PostDepartamentoAsync(Departamento departamento) 
        {
            await _contexto.Departamentos.AddAsync(departamento);
            await _contexto.SaveChangesAsync();
        }
        public async Task PutDepartamentoAsync(Departamento departamento) 
        {
            var existe = await _contexto.Departamentos.FindAsync(departamento.IdDepartamento);
            existe.Nombre = departamento.Nombre;
            existe.Ubicacion = departamento.Ubicacion;
            await _contexto.SaveChangesAsync();
        }
        public async Task DeleteDepartamentoByIdAsync(int id) 
        {
            var existe = await _contexto.Departamentos.FindAsync(id);
            _contexto.Remove(existe);
            await _contexto.SaveChangesAsync();
        }
    }
}