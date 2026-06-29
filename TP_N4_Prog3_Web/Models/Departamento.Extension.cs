namespace TP_N4_Prog3_Web.Models
{
    public partial class Departamento
    {
        public bool PuedeEliminarse 
        {
            get 
            {
                if (this.Empleados.Any()) return false;
                return true;
            }
        }
    }
}
