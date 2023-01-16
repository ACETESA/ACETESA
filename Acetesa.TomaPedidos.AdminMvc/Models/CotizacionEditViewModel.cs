using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class CotizacionEditViewModel
    {
        public string cn_proforma { get; set; }
        [Required(ErrorMessage = "Debe ingresar un Cliente.")]
        public string cc_tipana { get; set; }
        [Required(ErrorMessage = "Debe ingresar un Cliente.")]
        [Display(Name = "Razón Social")]
        public string cc_analis { get; set; }
        [Required(ErrorMessage = "Debe ingresar un Cliente.")]
        public string cd_razsoc { get; set; }
        [Display(Name = "Moneda")]
        [Required(ErrorMessage = "Debe seleccionar la {0}.")]
        public string cc_moneda { get; set; }
        [Display(Name = "Condición Venta")]
        [Required(ErrorMessage = "Debe seleccionar la {0}.")]
        public string cc_vta { get; set; }

        private DateTime _fechaActual = DateTime.Today;
        private string _fechaEmision;
        [Display(Name = "Fecha Emisión")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public string FechaEmision
        {
            get
            {
                if (string.IsNullOrEmpty(_fechaEmision) || string.IsNullOrWhiteSpace(_fechaEmision))
                {
                    _fechaEmision = _fechaActual.ToString("dd/MM/yyyy",
                    CultureInfo.CreateSpecificCulture("es-PE"));
                }
                return _fechaEmision;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _fechaEmision = value;
            }
        }
        public DateTime df_proceso { get; set; }
        public DateTime df_emision { get; set; }
        public string Tienda { get; set; }
        [Display(Name = "Precios")]
        public int igv_bo {get; set; }
        public decimal fm_tipcam { get; set; }
        public decimal fm_valvta { get; set; }
        public decimal fm_igv { get; set; }
        public decimal fm_totvta { get; set; }
        public string cb_estado { get; set; }

        [Display(Name = "Tipo Cambio")]
        public double? n_i_val_venta { get; set; }

        public CotizacionDetailViewModel CotizacionDetailViewModel { get; set; }
        public ClienteNewViewModel ClienteNewViewModel { get; set; }
        public EnviarMailViewModel EnviarMailViewModel { get; set; }

        //[Display(Name = "Atención")]
        //[Required(ErrorMessage = "Debe ingresar {0}")]
        //[MinLength(6, ErrorMessage = "Debe ingresar como mínimo {1} caracteres.")]
        //[MaxLength(100, ErrorMessage = "Debe ingresar como máximo {1} caracteres.")]
        ////[RegularExpression(@"^[A-Za-zñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "Solo letras")]
        //public string cd_atencion { get; set; }
        [Display(Name = "Sucursal")]
        public string cn_suc { get; set; }
        [Display(Name = "Contacto")]
        public string cn_contacto { get; set; }
        public ContactoEntregaDirectaNewViewModel ContactoEntregaDirectaNewViewModel { get; set; }
        [Display(Name = "Imprimir PrecioTN")]
        public int imprimirPrecioTN { get; set; }
        [Display(Name = "Observación")]
        public string observacion { get; set; }

        [Display(Name = "Zona Liberada")]
        public int zonaLiberada_bo { get; set; }

        [Display(Name = "Mostrar IGV")]
        public int mostrarIGV_bo { get; set; }

        [Display(Name = "Visita Cliente")]
        public int? VisitaClienteID { get; set; } = null;

    }
}