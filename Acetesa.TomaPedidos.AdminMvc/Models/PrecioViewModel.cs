using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class PrecioViewModel
    {
        [Display(Name = "Lista Precios")]
        [Required(ErrorMessage = "Campo requerido")]
        public string ListaPrecios { get; set; }

        [Display(Name = "Familia")]
        //[Required(ErrorMessage = "Campo requerido")]
        public string[] Familia { get; set; }

        [Display(Name = "Sub Familia")]
        public string SubFamilia { get; set; }

        public decimal TipoCambio { get; set; }
        public IEnumerable<PrecioModel> PrecioModels { get; set; }
        public int Stocks { get; set; }
    }
}