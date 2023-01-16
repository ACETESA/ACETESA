using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class ContactoTransporteNewModel
    {
        private const string CorreoRegexp = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Debe ingresar el nombre.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Entre 3 y 50 caracteres")]
        public string Nombres { get; set; }

        [Display(Name = "Telefono 1")]
        public string Telefono1 { get; set; }

        [Display(Name = "Telefono 2")]
        public string Telefono2 { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(CorreoRegexp, ErrorMessage = "Correo electrónico no válido")]
        public string Email { get; set; }

        public string Transportista { get; set; }
    }
}