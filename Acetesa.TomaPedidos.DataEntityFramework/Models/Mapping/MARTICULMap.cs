using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class MARTICULMap : EntityTypeConfiguration<MARTICUL>
    {
        public MARTICULMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_artic);

            // Properties
            this.Property(t => t.cc_gruart)
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.cc_artic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_famiart)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_marca)
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.cc_gruartec)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cc_subgruart)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_modelo)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.cd_artic)
                .IsFixedLength()
                .HasMaxLength(55);

            this.Property(t => t.cn_parara)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cd_artictc)
                .IsFixedLength()
                .HasMaxLength(60);

            this.Property(t => t.cc_unmed)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_undalt)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_stocks)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_standard)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ci_consig)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_nacional)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_critico)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_obsleto)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_barras)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.ct_graf)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.ci_abc)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_catart)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_estado)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_rotacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cn_partnumber)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cb_uso)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cn_item)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cc_catalogo)
                .HasMaxLength(25);

            this.Property(t => t.cc_tipoartic)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_costeo)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_eval_precio)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_tipArt)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_costo_kardexpaqbobi)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_articant)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_color)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_peso_pt)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_mprima)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_simbolo)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_costeo_pocl)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_codpeso)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_gruartecduf)
                .HasMaxLength(3);

            this.Property(t => t.cc_subgruartduf)
                .HasMaxLength(1);

            this.Property(t => t.c_fl_afecto_percepcion)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("MARTICUL");
            this.Property(t => t.cc_gruart).HasColumnName("cc_gruart");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.cc_famiart).HasColumnName("cc_famiart");
            this.Property(t => t.cc_marca).HasColumnName("cc_marca");
            this.Property(t => t.cc_gruartec).HasColumnName("cc_gruartec");
            this.Property(t => t.cc_subgruart).HasColumnName("cc_subgruart");
            this.Property(t => t.cc_modelo).HasColumnName("cc_modelo");
            this.Property(t => t.cd_artic).HasColumnName("cd_artic");
            this.Property(t => t.cn_parara).HasColumnName("cn_parara");
            this.Property(t => t.cd_artictc).HasColumnName("cd_artictc");
            this.Property(t => t.cc_unmed).HasColumnName("cc_unmed");
            this.Property(t => t.df_ultcom).HasColumnName("df_ultcom");
            this.Property(t => t.fm_ultcom).HasColumnName("fm_ultcom");
            this.Property(t => t.df_ultven).HasColumnName("df_ultven");
            this.Property(t => t.fm_ultven).HasColumnName("fm_ultven");
            this.Property(t => t.cb_undalt).HasColumnName("cb_undalt");
            this.Property(t => t.cb_activo).HasColumnName("cb_activo");
            this.Property(t => t.cb_stocks).HasColumnName("cb_stocks");
            this.Property(t => t.fq_smin).HasColumnName("fq_smin");
            this.Property(t => t.fq_smax).HasColumnName("fq_smax");
            this.Property(t => t.fq_nivrepos).HasColumnName("fq_nivrepos");
            this.Property(t => t.cb_standard).HasColumnName("cb_standard");
            this.Property(t => t.ci_consig).HasColumnName("ci_consig");
            this.Property(t => t.cb_nacional).HasColumnName("cb_nacional");
            this.Property(t => t.cb_critico).HasColumnName("cb_critico");
            this.Property(t => t.cb_obsleto).HasColumnName("cb_obsleto");
            this.Property(t => t.cc_barras).HasColumnName("cc_barras");
            this.Property(t => t.fm_precprom).HasColumnName("fm_precprom");
            this.Property(t => t.ct_graf).HasColumnName("ct_graf");
            this.Property(t => t.fm_ulco_d).HasColumnName("fm_ulco_d");
            this.Property(t => t.fm_ulve_d).HasColumnName("fm_ulve_d");
            this.Property(t => t.fm_precprom_d).HasColumnName("fm_precprom_d");
            this.Property(t => t.ci_abc).HasColumnName("ci_abc");
            this.Property(t => t.cb_catart).HasColumnName("cb_catart");
            this.Property(t => t.fm_ultpu).HasColumnName("fm_ultpu");
            this.Property(t => t.fm_ultpu_d).HasColumnName("fm_ultpu_d");
            this.Property(t => t.fd_stock_cero).HasColumnName("fd_stock_cero");
            this.Property(t => t.fm_consumo).HasColumnName("fm_consumo");
            this.Property(t => t.fq_sinicial).HasColumnName("fq_sinicial");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.cb_rotacion).HasColumnName("cb_rotacion");
            this.Property(t => t.cn_partnumber).HasColumnName("cn_partnumber");
            this.Property(t => t.cb_uso).HasColumnName("cb_uso");
            this.Property(t => t.cn_item).HasColumnName("cn_item");
            this.Property(t => t.fq_embalaje).HasColumnName("fq_embalaje");
            this.Property(t => t.cc_catalogo).HasColumnName("cc_catalogo");
            this.Property(t => t.cc_tipoartic).HasColumnName("cc_tipoartic");
            this.Property(t => t.fq_espesor).HasColumnName("fq_espesor");
            this.Property(t => t.fq_ancho).HasColumnName("fq_ancho");
            this.Property(t => t.fq_largo).HasColumnName("fq_largo");
            this.Property(t => t.cc_costeo).HasColumnName("cc_costeo");
            this.Property(t => t.cb_eval_precio).HasColumnName("cb_eval_precio");
            this.Property(t => t.cc_tipArt).HasColumnName("cc_tipArt");
            this.Property(t => t.cc_costo_kardexpaqbobi).HasColumnName("cc_costo_kardexpaqbobi");
            this.Property(t => t.df_creacion).HasColumnName("df_creacion");
            this.Property(t => t.fq_sku).HasColumnName("fq_sku");
            this.Property(t => t.cc_articant).HasColumnName("cc_articant");
            this.Property(t => t.cc_color).HasColumnName("cc_color");
            this.Property(t => t.cb_peso_pt).HasColumnName("cb_peso_pt");
            this.Property(t => t.cb_mprima).HasColumnName("cb_mprima");
            this.Property(t => t.cc_simbolo).HasColumnName("cc_simbolo");
            this.Property(t => t.cc_costeo_pocl).HasColumnName("cc_costeo_pocl");
            this.Property(t => t.cc_codpeso).HasColumnName("cc_codpeso");
            this.Property(t => t.cc_gruartecduf).HasColumnName("cc_gruartecduf");
            this.Property(t => t.cc_subgruartduf).HasColumnName("cc_subgruartduf");
            this.Property(t => t.c_fl_afecto_percepcion).HasColumnName("c_fl_afecto_percepcion");
            this.Property(t => t.fq_peso_teorico).HasColumnName("fq_peso_teorico");
        }
    }
}
