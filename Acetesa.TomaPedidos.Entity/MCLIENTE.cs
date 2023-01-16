using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class MCLIENTE
    {
        public MCLIENTE()
        {
            this.LCPROF_WEB = new List<LCPROF_WEB>();
        }

        public string cc_tipana { get; set; }
        public string cc_analis { get; set; }
        public string cc_pais { get; set; }
        public string cc_dpto { get; set; }
        public string cc_prov { get; set; }
        public string cc_sector { get; set; }
        public string cc_catclie { get; set; }
        public string cc_distrito { get; set; }
        public string cd_razsoc { get; set; }
        public string cc_zona { get; set; }
        public string cb_proced { get; set; }
        public string cd_direc { get; set; }
        public string cn_regind { get; set; }
        public string cn_sanit { get; set; }
        public string cn_regmerc { get; set; }
        public string ct_giro { get; set; }
        public DateTime? dt_constit { get; set; }
        public DateTime? dt_registro { get; set; }
        public string cn_telf1 { get; set; }
        public string cn_telf2 { get; set; }
        public string cn_telf3 { get; set; }
        public string cn_fax1 { get; set; }
        public string cn_fax2 { get; set; }
        public string cn_fax3 { get; set; }
        public string cb_monfac { get; set; }
        public string cb_limcred { get; set; }
        public string cb_cheque { get; set; }
        public string cb_sucursal { get; set; }
        public string cb_sector { get; set; }
        public string cb_activo { get; set; }
        public DateTime? dt_ultcomp { get; set; }
        public string ct_legal { get; set; }
        public string cn_rucleg { get; set; }
        public DateTime? dt_ultdeuda { get; set; }
        public decimal? fm_compmn { get; set; }
        public string cd_direcleg { get; set; }
        public decimal? fm_compme { get; set; }
        public string cn_telfleg { get; set; }
        public decimal? fm_acummn { get; set; }
        public string cn_faxleg { get; set; }
        public decimal? fm_acumme { get; set; }
        public decimal? fm_saldomn { get; set; }
        public decimal? fm_saldome { get; set; }
        public decimal? fq_descto { get; set; }
        public decimal? fm_cantporc { get; set; }
        public decimal? fm_desctomn { get; set; }
        public decimal? fm_montporcmn { get; set; }
        public decimal? fm_desctome { get; set; }
        public decimal? fm_montporcme { get; set; }
        public string cb_cheqdif { get; set; }
        public decimal? fm_diferidomn { get; set; }
        public decimal? fm_diferidome { get; set; }
        public string cb_flete { get; set; }
        public string cb_embalaje { get; set; }
        public string cb_moncred { get; set; }
        public decimal? fm_limcred { get; set; }
        public string cd_nomcom { get; set; }
        public string cd_appaterno { get; set; }
        public string cd_apmaterno { get; set; }
        public string cd_nombre1 { get; set; }
        public string cd_nombre2 { get; set; }
        public string c_fl_agente_percepcion { get; set; }
        public string cc_proyecto { get; set; }
        public string c_cod_documento_identidad { get; set; }
        public string c_fl_vinculacion { get; set; }
        public virtual ICollection<LCPROF_WEB> LCPROF_WEB { get; set; }
        public string ct_email { get; set; }
    }
}
