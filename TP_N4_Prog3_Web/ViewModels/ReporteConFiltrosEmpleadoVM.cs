using APP_PRUEBA_1.Models;
using TP_N4_Prog3_Web.DTOs;
namespace TP_N4_Prog3_Web.ViewModels
{
    public class ReporteConFiltrosEmpleadoVM
    {
        public FiltroEmpleadoDTO Filtros { get; set; } = default!;
        public IEnumerable<Empleado> Empleados { get; set; } = Enumerable.Empty<Empleado>();
        public IEnumerable<Departamento> Departamentos { get; set; } = Enumerable.Empty<Departamento>();
    }
}