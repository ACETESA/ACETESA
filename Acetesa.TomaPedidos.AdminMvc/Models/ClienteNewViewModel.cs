using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class ClienteNewViewModel
    {
        private const int RazonSocialMinimumLength = 3;
        private const int RazonSocialMaximunLength = 65;
        private const int DomicilioMinimumLength = 3;
        private const int DomicilioMaximunLength = 100;
        private const string RazonSocialRegexp = @"^[A-Za-zñÑáéíóúÁÉÍÓÚ\s\.]+$";

        private const string CorreoRegexp =
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        [Required(ErrorMessage = "Debe ingresar el RUC.")]
        [RegularExpression(@"^(([1][0])|([2][0])|(\d{2}))\d{9}$", ErrorMessage = "{0} incorrecto.")]
        public string Ruc { get; set; }
        [Required(ErrorMessage = "Debe ingresar la Razón Social.")]
        [StringLength(RazonSocialMaximunLength, MinimumLength = RazonSocialMinimumLength, 
            ErrorMessage = "Entre {2} y {1} caracteres")]
        //[RegularExpression(RazonSocialRegexp, ErrorMessage = "Solo letras, tildes y espacio.")]
        public string RazonSocial { get; set; }

        [RegularExpression(CorreoRegexp, ErrorMessage = "Correo electrónico no válido")]
        public string Email { get; set; }

        [StringLength(DomicilioMaximunLength, MinimumLength = DomicilioMinimumLength,
    ErrorMessage = "Entre {2} y {1} caracteres")]
        [Required(ErrorMessage = "Debe ingresar el domicilio.")]
        public string Domicilio { get; set; }

        
        public string Telefono { get; set; }
        public string cc_sector { get; set; }
        public string cc_departamento { get; set; }
        public string cc_provincia { get; set; }
        public string cc_distrito { get; set; }
    }
}