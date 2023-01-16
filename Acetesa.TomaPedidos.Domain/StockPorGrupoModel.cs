using System;

namespace Acetesa.TomaPedidos.Domain
{
    public class StockPorGrupoModel
    {
        public string cn_serienp { get; set; }
        public string cn_notpedvta { get; set; }
        public DateTime df_notpedvta { get; set; }
        public string cc_analiscli { get; set; }
        public string cd_razsoc { get; set; }
        public string cb_estado { get; set; }
        public string cc_artic { get; set; }
        public string cd_artic { get; set; }
        public decimal fq_cantped { get; set; }
        public decimal fq_cant_desp { get; set; }
        public string Alm_Codigo { get; set; }

        public string Art_Grupo { get; set; }
        public string Art_SubGrupo { get; set; }

        public string cq_espesor { get; set; }
        public string cq_base { get; set; }
        public string cq_altura { get; set; }
        public string cq_largo { get; set; }
        public string cq_diametro { get; set; }

        public decimal fq_embalaje { get; set; }
        public decimal fq_unidades { get; set; }
        public decimal fq_peso { get; set; }

        public decimal tot_un { get; set; }
        public decimal tot_tn { get; set; }

        public string cd_desc { get; set; }


        public decimal ReservadoUN { get; set; }
        public decimal fq_stk_disp { get; set; }

        public decimal ReservadoTN { get; set; }
        public decimal fq_DispUN { get; set; }
        public decimal fq_DispTN { get; set; }
    }
}
