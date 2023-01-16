using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class EstadoCuentaViewModel
    {
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo obligatorio.")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Entre {2} y {1}")]
        public string Cliente { get; set; }
        public EnviarMailViewModel EnviarMailViewModel { get; set; }

    }
}