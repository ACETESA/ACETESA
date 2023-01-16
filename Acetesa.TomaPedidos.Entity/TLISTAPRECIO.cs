using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class TLISTAPRECIO
    {
        public TLISTAPRECIO()
        {
            this.LDPEDIDO_WEB = new List<LDPEDIDO_WEB>();
            this.LDPROF_WEB = new List<LDPROF_WEB>();
        }

        public string cc_artic { get; set; }
        public string cc_lista { get; set; }
        public decimal? fm_preciounit { get; set; }
        public string cc_moneda { get; set; }
        public decimal? fm_costounit { get; set; }
        public decimal? fm_dcto1 { get; set; }
        public decimal? fm_dcto2 { get; set; }
        public string cb_estado { get; set; }
        public string cc_destino { get; set; }
        public decimal? fm_preciounit_ant { get; set; }
        public string c_fl_afecto_percepcion { get; set; }
        public virtual ICollection<LDPEDIDO_WEB> LDPEDIDO_WEB { get; set; }
        public virtual ICollection<LDPROF_WEB> LDPROF_WEB { get; set; }
    }
}
