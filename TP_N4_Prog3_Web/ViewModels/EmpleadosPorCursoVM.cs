using APP_PRUEBA_1.Models;

namespace TP_N4_Prog3_Web.ViewModels
{
    public class EmpleadosPorCursoVM
    {
        public string NombreCurso { get; set; } = null!;
        public IEnumerable<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
