using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class TSUCCLIE
    {
        public TSUCCLIE()
        {
            this.LCPEDIDO_WEB = new List<LCPEDIDO_WEB>();
        }

        public string cc_tipana { get; set; }
        public string cc_analis { get; set; }
        public string cn_suc { get; set; }
        public string cc_dpto { get; set; }
        public string cd_suc { get; set; }
        public string ct_email { get; set; }
        public string cc_prov { get; set; }
        public string cc_distrito { get; set; }
        public string cc_zona { get; set; }
        public string cd_direc { get; set; }
        public string cn_telf1 { get; set; }
        public string cn_telf2 { get; set; }
        public string cn_telf3 { get; set; }
        public string cn_fax1 { get; set; }
        public string cn_fax2 { get; set; }
        public string cn_fax3 { get; set; }
        public string cb_activo { get; set; }
        public virtual ICollection<LCPEDIDO_WEB> LCPEDIDO_WEB { get; set; }
    }
}
