using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class LCPROF_WEB
    {
        public LCPROF_WEB()
        {
            this.LCPEDIDO_WEB = new List<LCPEDIDO_WEB>();
            this.LDPROF_WEB = new List<LDPROF_WEB>();
        }
        public string cn_proforma { get; set; }
        public string cc_tipana { get; set; }
        public string cc_analis { get; set; }
        public string cd_razsoc { get; set; }
        public string cc_moneda { get; set; }
        public string cc_vta { get; set; }
        public DateTime df_proceso { get; set; }
        public DateTime df_emision { get; set; }
        public decimal fm_tipcam { get; set; }
        public decimal fm_valvta { get; set; }
        public decimal fm_igv { get; set; }
        public decimal fm_totvta { get; set; }
        public string cb_estado { get; set; }
        public int? VisitaClienteID { get; set; } = null;
        //public bool imprimirPrecioTN { get; set; }
        //public string cc_tienda { get; set; }
        //public string cd_atencion { get; set; }
        public virtual ICollection<LCPEDIDO_WEB> LCPEDIDO_WEB { get; set; }
        public virtual MCLIENTE MCLIENTE { get; set; }
        public virtual TCONDVTA TCONDVTA { get; set; }
        public virtual TMONEDA TMONEDA { get; set; }
        public virtual ICollection<LDPROF_WEB> LDPROF_WEB { get; set; }
    }
}
