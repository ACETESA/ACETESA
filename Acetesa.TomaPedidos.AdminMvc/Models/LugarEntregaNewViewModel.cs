using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class LugarEntregaNewViewModel
    {
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Debe ingresar la dirección")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Entre 3 y 60 caracteres")]
        public string Direccion { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe seleccionar el Departamento.")]
        public string Departamento { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Debe seleccionar el Provincia.")]
        public string Provincia { get; set; }

        [Display(Name = "Distrito")]
        [Required(ErrorMessage = "Debe seleccionar el Distrito.")]
        public string Distrito { get; set; }

        [Display(Name = "Zona")]
        [Required(ErrorMessage = "Debe seleccionar la Zona.")]
        public string Zona { get; set; }

        public string Entrega { get; set; }
        public string Cobranza { get; set; }
        public string Analisis { get; set; }
    }
}