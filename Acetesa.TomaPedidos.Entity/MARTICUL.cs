using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class MARTICUL
    {
        public MARTICUL()
        {
            this.LDPEDIDO_WEB = new List<LDPEDIDO_WEB>();
            this.LDPROF_WEB = new List<LDPROF_WEB>();
        }

        public string cc_gruart { get; set; }
        public string cc_artic { get; set; }
        public string cc_famiart { get; set; }
        public string cc_marca { get; set; }
        public string cc_gruartec { get; set; }
        public string cc_subgruart { get; set; }
        public string cc_modelo { get; set; }
        public string cd_artic { get; set; }
        public string cn_parara { get; set; }
        public string cd_artictc { get; set; }
        public string cc_unmed { get; set; }
        public DateTime? df_ultcom { get; set; }
        public double? fm_ultcom { get; set; }
        public DateTime? df_ultven { get; set; }
        public double? fm_ultven { get; set; }
        public string cb_undalt { get; set; }
        public string cb_activo { get; set; }
        public string cb_stocks { get; set; }
        public double? fq_smin { get; set; }
        public double? fq_smax { get; set; }
        public double? fq_nivrepos { get; set; }
        public string cb_standard { get; set; }
        public string ci_consig { get; set; }
        public string cb_nacional { get; set; }
        public string cb_critico { get; set; }
        public string cb_obsleto { get; set; }
        public string cc_barras { get; set; }
        public double? fm_precprom { get; set; }
        public string ct_graf { get; set; }
        public double? fm_ulco_d { get; set; }
        public double? fm_ulve_d { get; set; }
        public double? fm_precprom_d { get; set; }
        public string ci_abc { get; set; }
        public string cb_catart { get; set; }
        public double? fm_ultpu { get; set; }
        public double? fm_ultpu_d { get; set; }
        public DateTime? fd_stock_cero { get; set; }
        public double? fm_consumo { get; set; }
        public double? fq_sinicial { get; set; }
        public string cb_estado { get; set; }
        public string cb_rotacion { get; set; }
        public string cn_partnumber { get; set; }
        public string cb_uso { get; set; }
        public string cn_item { get; set; }
        public double? fq_embalaje { get; set; }
        public string cc_catalogo { get; set; }
        public string cc_tipoartic { get; set; }
        public double? fq_espesor { get; set; }
        public double? fq_ancho { get; set; }
        public double? fq_largo { get; set; }
        public string cc_costeo { get; set; }
        public string cb_eval_precio { get; set; }
        public string cc_tipArt { get; set; }
        public string cc_costo_kardexpaqbobi { get; set; }
        public DateTime? df_creacion { get; set; }
        public double? fq_sku { get; set; }
        public string cc_articant { get; set; }
        public string cc_color { get; set; }
        public string cb_peso_pt { get; set; }
        public string cb_mprima { get; set; }
        public string cc_simbolo { get; set; }
        public string cc_costeo_pocl { get; set; }
        public string cc_codpeso { get; set; }
        public string cc_gruartecduf { get; set; }
        public string cc_subgruartduf { get; set; }
        public string c_fl_afecto_percepcion { get; set; }
        public double? fq_peso_teorico { get; set; }
        public virtual ICollection<LDPEDIDO_WEB> LDPEDIDO_WEB { get; set; }
        public virtual ICollection<LDPROF_WEB> LDPROF_WEB { get; set; }
    }
}
