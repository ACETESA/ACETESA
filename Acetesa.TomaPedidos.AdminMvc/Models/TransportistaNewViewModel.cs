using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class TransportistaNewViewModel
    {
        private const int RazonSocialMinimumLength = 3;
        private const int RazonSocialMaximunLength = 45;
        private const string RazonSocialRegexp = @"^[A-Za-zñÑáéíóúÁÉÍÓÚ\s\.]+$";

        [Display(Name = "RUC")]
        [Required(ErrorMessage = "Debe ingresar el RUC.")]
        [RegularExpression(@"^(([1][0])|([2][0])|(\d{2}))\d{9}$", ErrorMessage = "{0} incorrecto.")]
        public string Ruc { get; set; }

        [Display(Name = "Razón Social")]
        [Required(ErrorMessage = "Debe ingresar la Razón Social.")]
        [StringLength(RazonSocialMaximunLength, MinimumLength = RazonSocialMinimumLength,ErrorMessage = "Entre {2} y {1} caracteres")]
        [RegularExpression(RazonSocialRegexp, ErrorMessage = "Solo letras, tildes y espacio.")]
        public string RazonSocial { get; set; }

        [Display(Name = "Domicilio")]
        [Required(ErrorMessage = "Debe ingresar el domicilio.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Entre 3 y 60 caracteres")]
        public string Domicilio { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe seleccionar el Departamento.")]
        public string Departamento { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Debe seleccionar el Provincia.")]
        public string Provincia { get; set; }

        [Display(Name = "Distrito")]
        [Required(ErrorMessage = "Debe seleccionar el Distrito.")]
        public string Distrito { get; set; }

        public string Cliente { get; set; }
    }
}