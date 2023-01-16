using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class StockViewModel
    {
        [Display(Name = "Familia")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Familia { get; set; }
        [Display(Name = "Sub Familia")]
        [Required(ErrorMessage = "Campo requerido")]
        public string SubFamilia { get; set; }
        [Display(Name = "Tipo")]
        //jlazaro
        //[Required(ErrorMessage = "Campo requerido")]
        public string Tipo { get; set; }

        public IEnumerable<StockModel> StockModels { get; set; }

        //jlazaro
        public List<string> CabecerasSP { get; set; }
        //jlazaro
        public List<Dictionary<string, object>> DatosSP { get; set; }
    }
}