using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class PedidoEditViewModel
    {
        public string cn_pedido { get; set; }
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

        private string _sfechaEntrega;
        [Display(Name = "Fecha Entrega")]
        [Required(ErrorMessage = "Debe ingresar {0}")]
        public string FechaEntrega
        {
            get
            {
                if (string.IsNullOrEmpty(_sfechaEntrega) || string.IsNullOrWhiteSpace(_sfechaEntrega))
                {
                    _sfechaEntrega = _fechaActual.ToString("dd/MM/yyyy",
                    CultureInfo.CreateSpecificCulture("es-PE"));
                }
                return _sfechaEntrega;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _sfechaEntrega = value;
            }
        }
        public DateTime df_proceso { get; set; }
        public DateTime df_emision { get; set; }
        public string Tienda { get; set; }
        [Display(Name = "Precios")]
        public int igv_bo { get; set; }
        [Display(Name = "Zona Liberada")]
        public int zonaLiberada_bo { get; set; }
        [Display(Name = "Mostrar IGV")]
        public int mostrarIGV_bo { get; set; }
        public decimal fm_tipcam { get; set; }
        public decimal fm_valvta { get; set; }
        public decimal fm_igv { get; set; }
        public decimal fm_totvta { get; set; }
        public string cb_estado { get; set; }
        //[Required(ErrorMessage = "Debe seleccionar sucursal.")]
        [Display(Name = "Sucursal cliente")]
        public string cn_suc { get; set; }
        [Display(Name = "Tipo de Entrega")]
        //[DefaultValue(false)]
        public string cb_recojo { get; set; }
        [Display(Name = "Tipo Cambio")]
        public double? n_i_paralelo_venta { get; set; }
        [RegularExpression(@"^.{0,20}?$", ErrorMessage = "Debe tener un máximo de 20 dígitos.")]
        [Display(Name = "Número OC")]
        public string cn_ocompra { get; set; }
        [Display(Name = "Observación")]
        public string Vt_observacion { get; set; }

        [Display(Name = "Lugar de Entrega")]
        public string Cn_lug { get; set; }

        [Display(Name = "Agencia Transporte")]
        public string CC_transp { get; set; }

        [Display(Name = "Contacto Transporte")]
        public string ContactoTransporte { get; set; }

        [Display(Name = "Contacto Entrega")]
        public string IdContactoEntregaDirecta { get; set; }

        public bool EsProforma { get; set; }
        public PedidoDetailViewModel PedidoDetailViewModel { get; set; }
        public ClienteNewViewModel ClienteNewViewModel { get; set; }
        public EnviarMailViewModel EnviarMailViewModel { get; set; }
        public TransportistaNewViewModel TransportistaNewViewModel { get; set; }
        public ContactoTransporteNewModel ContactoTransporteNewModel { get; set; }
        
        public LugarEntregaNewViewModel LugarEntregaNewViewModel { get; set; }
        public ContactoEntregaDirectaNewViewModel ContactoEntregaDirectaNewViewModel { get; set; }
    }
}