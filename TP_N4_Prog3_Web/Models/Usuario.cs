using System.ComponentModel.DataAnnotations;
namespace APP_PRUEBA_1.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50)]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255)]
        public string Pass { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; } = null!;
    }
}