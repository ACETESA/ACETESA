using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class EnviarMailViewModel
    {
        private const string CorreoRegexp =
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        [Required(ErrorMessage = "Debe ingresar el asunto.")]
        //[RegularExpression(CorreoRegexp, ErrorMessage = "Correo electrónico no válido")]
        public string Asunto { get; set; }

        [Required(ErrorMessage = "Debe ingresar Correo Electrónico.")]
        //[RegularExpression(CorreoRegexp, ErrorMessage = "Correo electrónico no válido")]
        public string Para { get; set; }

        [Display(Name="CC")]
        //[Required(ErrorMessage = "Debe ingresar Correo Copia.")]
        [RegularExpression(CorreoRegexp, ErrorMessage = "Correo electrónico no válido")]
        public string ConCopia { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debe ingresar el Mensaje.")]
        public string Mensaje { get; set; }
    }
}