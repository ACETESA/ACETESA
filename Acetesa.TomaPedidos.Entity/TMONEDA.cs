using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class TMONEDA
    {
        public TMONEDA()
        {
            this.LCPEDIDO_WEB = new List<LCPEDIDO_WEB>();
            this.LCPROF_WEB = new List<LCPROF_WEB>();
        }

        public string cc_moneda { get; set; }
        public string cd_moneda { get; set; }
        public string cb_nac { get; set; }
        public string cd_simbolo { get; set; }
        public string cb_moncambio { get; set; }
        public string cb_indmon { get; set; }


        public virtual ICollection<LCPEDIDO_WEB> LCPEDIDO_WEB { get; set; }
        public virtual ICollection<LCPROF_WEB> LCPROF_WEB { get; set; }
    }
}
