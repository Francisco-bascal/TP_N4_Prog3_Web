using TP_N4_Prog3_Web.Models;

namespace TP_N4_Prog3_Web.ViewModels
{
    public class EmpleadosAgrupadosPorDepartamentoVM
    {
        public string NombreDepartamento { get; set; } = null!;
        public List<Empleado> Empleados { get; set; } = new();
    }
}
