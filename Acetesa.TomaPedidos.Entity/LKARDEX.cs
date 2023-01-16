using System;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class Lkardex
    {
        public string cc_kardex { get; set; }
        public string cc_artic { get; set; }
        public string cc_almac { get; set; }
        public DateTime? df_kardex { get; set; }
        public decimal? nq_artic { get; set; }
        public decimal? fm_artic { get; set; }
        public decimal? fm_artic_d { get; set; }
        public string cb_tipo { get; set; }
        public decimal? fm_ultpu { get; set; }
        public decimal? fm_ultpu_d { get; set; }
        public string cc_notsal { get; set; }
        public string cc_noting { get; set; }
        public string cb_estado { get; set; }
        public int ni_corre { get; set; }
        public decimal? fm_costo_prom { get; set; }
        public decimal? fm_costo_prom_d { get; set; }
        public string cn_artxpaq { get; set; }
        public string cc_movi { get; set; }
        public string cb_servicio { get; set; }
    }
}
