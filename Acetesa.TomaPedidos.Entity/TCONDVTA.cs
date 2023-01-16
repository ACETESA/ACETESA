using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class TCONDVTA
    {
        public TCONDVTA()
        {
            this.LCPEDIDO_WEB = new List<LCPEDIDO_WEB>();
            this.LCPROF_WEB = new List<LCPROF_WEB>();
        }

        public string cc_vta { get; set; }
        public string cd_vta { get; set; }
        public decimal? fq_pond { get; set; }
        public decimal? fq_diasplazo { get; set; }
        public decimal? fm_porcmin { get; set; }
        public decimal? fm_porcmax { get; set; }
        public string cb_gasto { get; set; }
        public decimal? fq_dias { get; set; }
        public decimal? fq_letras { get; set; }
        public string cb_condvta { get; set; }
        public string cb_no_fact { get; set; }
        public string cb_todos { get; set; }
        public virtual ICollection<LCPEDIDO_WEB> LCPEDIDO_WEB { get; set; }
        public virtual ICollection<LCPROF_WEB> LCPROF_WEB { get; set; }
    }
}
