using System.ComponentModel.DataAnnotations;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class PedidoDetailViewModel
    {
        public string cn_pedido { get; set; }


        public string cn_item { get; set; }
        [Display(Name = "Grupo")]
        //[Required(ErrorMessage = "Debe seleccionar un {0}.")]
        public string cc_grupo { get; set; }
        [Display(Name = "Sub Grupo")]
        //[Required(ErrorMessage = "Debe seleccionar un {0}.")]
        public string cc_subgrupo { get; set; }
        [Display(Name = "Artículo")]
        [Required(ErrorMessage = "Debe seleccionar un {0}.")]
        public string cc_artic { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un artículo.")]
        public string cd_artic { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un artículo.")]
        public string cc_unmed { get; set; }
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Debe ingresar la Cantidad.")]
        //[RegularExpression(@"^(([1-9](([0-9])|([1-9])){1,5})|([1-9]))$", ErrorMessage = "Formato no válido para {0} [1-999999]")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Formato no válido para {0} [1-99] hasta 2 decimales")]
        public decimal fq_cantidad { get; set; }
        [Display(Name = "Precio TM")]
        public decimal fm_precio_tonelada { get; set; }
        [Display(Name = "Peso Unitario")]
        public decimal fq_peso_teorico { get; set; }
        public decimal fq_stock { get; set; }
        //[Required(ErrorMessage = "Debe existir código precio de lista.")]
        public string cc_lista { get; set; }
        public int igv_bo { get; set; }

        [Display(Name = "Precio Lista")]
        public decimal fm_precio { get; set; }
        [Display(Name = "Precio Lista 2")]
        public decimal fm_precio2 { get; set; }
        [Display(Name = "Precio Final")]
        [Required(ErrorMessage = "Debe ingresar {0}.")]
        //[RegularExpression(@"^([0].\d{1,4})|([1-9]{1,5}[0]?(.\d{1,4})?)$", ErrorMessage = "Formato no válido para {0} [1-9999]")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Formato no válido para {0} [1-9999] hasta 4 decimales")]
        public decimal fm_precio_fin { get; set; }
        public decimal fm_total { get; set; }

        public decimal fm_valvta { get; set; }
        public decimal fm_igv { get; set; }
        public decimal fm_totvta { get; set; }
        public bool EsProforma { get; set; }
        public int zonaLiberada_bo { get; set; }
    }
}