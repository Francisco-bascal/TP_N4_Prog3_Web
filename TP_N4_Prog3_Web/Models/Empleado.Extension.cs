using System.ComponentModel.DataAnnotations.Schema;

namespace TP_N4_Prog3_Web.Models
{
    public partial class Empleado
    {
        [NotMapped]
        public bool EsElegibleParaBaja 
        { 
            get 
            {
                //if (this.CantidadHijos >= 4) return false;
                if (this.Estado.Equals(true)) return false; //Si está activo no se puede eliminar 
                if (this.FechaIngreso < DateOnly.FromDateTime(DateTime.Now.AddYears(-5))) return false; //Si su antiguedad es mayor a 5 años no se puede eliminar
                return true;
            } 
        }
    }
}
