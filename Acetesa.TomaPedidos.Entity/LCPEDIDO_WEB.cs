using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class LCPEDIDO_WEB
    {
        public LCPEDIDO_WEB()
        {
            this.LDPEDIDO_WEB = new List<LDPEDIDO_WEB>();
        }

        public string cn_pedido { get; set; }
        public string cn_proforma { get; set; }
        public string cc_tipana { get; set; }
        public string cc_analis { get; set; }
        public string cn_suc { get; set; }
        public string cd_razsoc { get; set; }
        public string cc_moneda { get; set; }
        public string cc_vta { get; set; }
        public DateTime df_proceso { get; set; }
        public DateTime df_emision { get; set; }
        public decimal fm_tipcam { get; set; }
        public decimal fm_valvta { get; set; }
        public decimal fm_igv { get; set; }
        public decimal fm_totvta { get; set; }
        public string cb_recojo { get; set; }
        //public string cc_tienda { get; set; }
        public string cb_estado { get; set; }
        public virtual LCPROF_WEB LCPROF_WEB { get; set; }
        public virtual TCONDVTA TCONDVTA { get; set; }
        public virtual TMONEDA TMONEDA { get; set; }
        public virtual TSUCCLIE TSUCCLIE { get; set; }
        public virtual ICollection<LDPEDIDO_WEB> LDPEDIDO_WEB { get; set; }
    }
}
