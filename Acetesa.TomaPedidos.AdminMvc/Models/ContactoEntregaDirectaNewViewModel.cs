using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class ContactoEntregaDirectaNewViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar la Sucursal.")]
        public string Surcursal { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre del contacto")]
        public string Nombres { get; set; }
        [Display(Name = "Teléfono N°1")]
        public string Telefono { get; set; }
        [Display(Name = "Teléfono N°2")]
        public string Telefono2 { get; set; }
        public string Email { get; set; }
        public bool ContVenta { get; set; }
        public bool ContCobranza { get; set; }
        public bool ContAlmacen { get; set; }
        [Display(Name = "Cargo Laboral")]
        public string CargoLaboral { get; set; }
        [Display(Name = "Envío de documentos")]
        public string EnvioDocs { get; set; }
        public string Analisis { get; set; }

    }
}