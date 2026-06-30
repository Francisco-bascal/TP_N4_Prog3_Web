using System.ComponentModel.DataAnnotations;

namespace TP_N4_Prog3_Web.DTOs
{
    public class RegistroDTO
    {
        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Pass { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Pass", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPass { get; set; } = null!;
    }
}