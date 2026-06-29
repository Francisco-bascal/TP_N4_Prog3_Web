using APP_PRUEBA_1.Models;
using Microsoft.EntityFrameworkCore;
using TP_N4_Prog3_Web.DTOs;

namespace APP_PRUEBA_1.Repositorios
{
    //El repositorio solo debe ejecutar las acciones solicitadas, más no validar y manejar excepciones.
    //Esto significa que asume que las acciones que se le solicitan son válidas de realizar cuando se llaman desde el servicio.
    //Solo pueden ocurrir excepciones de la base de datos que se manejen en el controlador.
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly RRHH2026_Db_Context _contexto;
        public EmpleadoRepository(RRHH2026_Db_Context contexto)
        {
            _contexto = contexto;
        }

        public async Task<ICollection<Empleado>> GetEmpleadosAsync()
        {
            return await _contexto.Empleados.Include(e => e.IdDepartamentoNavigation).Include(e => e.IdCursos).ToListAsync(); //Include para mostrar el nombre del departamento
        }

        public async Task<Empleado?> GetEmpleadoByIdAsync(int id) 
        {
            var empleado = await _contexto.Empleados.Include(e => e.IdDepartamentoNavigation).Include(e => e.IdCursos).FirstOrDefaultAsync(e => e.IdEmpleado.Equals(id));
            return empleado;
        }
        public async Task PostEmpleadoAsync(Empleado empleado) 
        {
            await _contexto.Empleados.AddAsync(empleado);
            await _contexto.SaveChangesAsync();
        }

        //Quedó inutilizado por las Data Tables
        //Repo para Filtro de Búsqueda ↨
        public async Task<ICollection<Empleado>> GetEmpleadosFiltradosAsync(string? busqueda, int? departamentoId)
        {
            IQueryable<Empleado> query = _contexto.Empleados;
            if (!string.IsNullOrWhiteSpace(busqueda)) 
                query = query.Where(e => e.Nombre.ToLower().Contains(busqueda.ToLower()) || e.Apellido.Contains(busqueda.ToLower()));

            if (departamentoId.HasValue)
                query = query.Where(e => e.IdDepartamento.Equals(departamentoId.Value));

            return await query.ToListAsync();
        }
        //Repo para Filtro de Búsqueda ↨
        public async Task PutEmpleadoAsync(Empleado empleado)
        {
            var existe = await _contexto.Empleados.Include(e => e.IdCursos).FirstOrDefaultAsync(e => e.IdEmpleado.Equals(empleado.IdEmpleado));

            existe.Nombre = empleado.Nombre;
            existe.Apellido = empleado.Apellido;
            existe.Dni = empleado.Dni;
            existe.Estado = empleado.Estado;
            existe.FechaIngreso = empleado.FechaIngreso;
            existe.CantidadHijos = empleado.CantidadHijos;
            existe.IdDepartamento = empleado.IdDepartamento;

            existe.IdCursos.Clear();

            //Reasignación segura de los cursos por checkbox al empleado
            foreach (var curso in empleado.IdCursos)
            {
                var cursoExistente = await _contexto.Cursos.FindAsync(curso.IdCurso);

                if (cursoExistente != null)
                {
                    existe.IdCursos.Add(cursoExistente);
                }
            }

            await _contexto.SaveChangesAsync();
        }

        public async Task DeleteEmpleadoByIdAsync(int id)
        {
            var existe = await _contexto.Empleados.FindAsync(id);

            _contexto.Empleados.Remove(existe);
            await _contexto.SaveChangesAsync();
        }
        //En desuso
        public async Task DeleteEmpleadoAsync(Empleado empleado) 
        {
            _contexto.Empleados.Attach(empleado);
            _contexto.Empleados.Remove(empleado);

            await _contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<Empleado>> GetEmpleadosConFiltroReportes(FiltroEmpleadoDTO filtros) 
        {
            var query = _contexto.Empleados.AsNoTracking().Include(e => e.IdDepartamentoNavigation).AsQueryable();

            if (filtros.IdDepartamento.HasValue) 
            {
                query = query.Where(e => e.IdDepartamento.Equals(filtros.IdDepartamento.Value));
            }

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda)) 
            {
                query = query.Where(e => e.Nombre.ToLower().Contains(filtros.Busqueda.ToLower().Trim()) || e.Apellido.ToLower().Contains(filtros.Busqueda.ToLower().Trim()));
            }

            if (filtros.Estado.HasValue) 
            {
                query = query.Where(e => e.Estado.Equals(filtros.Estado.Value));
            }

            if (filtros.CantidadHijosMin.HasValue) 
            {
                query = query.Where(e => e.CantidadHijos >= filtros.CantidadHijosMin.Value);
            }

            if (filtros.CantidadHijosMax.HasValue)
            {
                query = query.Where(e => e.CantidadHijos <= filtros.CantidadHijosMax.Value);
            }

            if (filtros.FechaIngresoDesde.HasValue) 
            {
                query = query.Where(e => e.FechaIngreso >= filtros.FechaIngresoDesde.Value);
            }

            if (filtros.FechaIngresoHasta.HasValue)
            {
                query = query.Where(e => e.FechaIngreso <= filtros.FechaIngresoHasta.Value);
            }

            return await query.OrderBy(e => e.Apellido).ThenBy(e => e.Nombre).ToListAsync();
        }
    }
}
/*Antes Retornaban bool: 
-DeleteEmpleadoByIdAsync()
-PutEmpleadoAsync()
*/

/* Previo al cambio de enfoque:
public async Task PutEmpleadoAsync(Empleados empleado)
        {
            var entidad = new Empleados
            {
                IdEmpleado = empleado.IdEmpleado
            };

            _contexto.Empleados.Attach(entidad); //Adjuntar al contexto como entidad a modificar para evitar doble búsqueda

            //Designar propiedades a modificar
            entidad.Nombre = empleado.Nombre;
            entidad.Apellido = empleado.Apellido;
            entidad.FechaIngreso = empleado.FechaIngreso;
            entidad.IdDepartamento = empleado.IdDepartamento;

            //Marcarlas como modificadas para que al hacer savechanges se genere una consulta que las actualize
            _contexto.Entry(entidad).Property(e => e.Nombre).IsModified = true;
            _contexto.Entry(entidad).Property(e => e.Apellido).IsModified = true;
            _contexto.Entry(entidad).Property(e => e.FechaIngreso).IsModified = true;
            _contexto.Entry(entidad).Property(e => e.IdDepartamento).IsModified = true;

            await _contexto.SaveChangesAsync();
        }

        public async Task DeleteEmpleadoByIdAsync(int id) 
        {
            var entidad = new Empleados
            {
                IdEmpleado = id
            };

            _contexto.Empleados.Attach(entidad);
            _contexto.Empleados.Remove(entidad);

            await _contexto.SaveChangesAsync();
        }

*/

/*
 //Repo simplificado:
public async Task UpdateEmpleadoAsync(Empleados empleado)
{
    _contexto.Empleados.Update(empleado);
    await _contexto.SaveChangesAsync();
}
*/