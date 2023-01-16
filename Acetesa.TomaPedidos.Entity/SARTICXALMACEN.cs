using System;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class Sarticxalmacen
    {
        public string cc_anho { get; set; }
        public string cc_mes { get; set; }
        public string cc_artic { get; set; }
        public string cc_locac { get; set; }
        public string cc_almac { get; set; }
        public decimal? fq_saldo_inicial { get; set; }
        public decimal? fq_ingresos { get; set; }
        public decimal? fq_egresos { get; set; }
        public decimal? fq_saldo_final { get; set; }
        public decimal? fm_monto_ing_nac { get; set; }
        public decimal? fm_monto_sal_nac { get; set; }
        public decimal? fm_monto_ing_ext { get; set; }
        public decimal? fm_monto_sal_ext { get; set; }
        public decimal? fm_costo_prom_nac { get; set; }
        public decimal? fm_costo_prom_ext { get; set; }
        public decimal? fm_saldo_nac { get; set; }
        public decimal? fm_saldo_ext { get; set; }
        public decimal? fm_saldo_final_nac { get; set; }
        public decimal? fm_saldo_final_ext { get; set; }
        public decimal? fm_costo_nac { get; set; }
        public decimal? fm_costo_ext { get; set; }
        public decimal? fm_monto_ajus_nac { get; set; }
        public decimal? fm_monto_ajus_ext { get; set; }
        public decimal? fm_saldo_ant_ajus_nac { get; set; }
        public decimal? fm_saldo_ant_ajus_ext { get; set; }
    }
}
