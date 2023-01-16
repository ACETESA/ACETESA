namespace Acetesa.TomaPedidos.Domain
{
    public class PrecioModel
    {
        public string cc_artic { get; set; }
        public string cd_artic { get; set; }
        public string cc_unmed { get; set; }
        public string fq_peso_teorico { get; set; }
        public string fq_unid_tm { get; set; }
        public string cd_moneda { get; set; }
        //public decimal fm_precio_mn { get; set; }
        //public decimal fm_precio_me { get; set; }
        public string fm_valorunit { get; set; }
        public string fm_valorunit2 { get; set; }
        public string fm_valorvta_tn { get; set; }
        public string fm_valorvta_tn2 { get; set; }
        public decimal ni_tipcam { get; set; }
        public decimal ArticStk { get; set; }

    }
}
