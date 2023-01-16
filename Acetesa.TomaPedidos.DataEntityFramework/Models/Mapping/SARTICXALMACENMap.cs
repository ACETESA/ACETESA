using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class SarticxalmacenMap : EntityTypeConfiguration<Sarticxalmacen>
    {
        public SarticxalmacenMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_anho = t.cc_anho, cc_mes = t.cc_mes, cc_artic = t.cc_artic, cc_locac = t.cc_locac, cc_almac = t.cc_almac });

            // Properties
            this.Property(t => t.cc_anho)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.cc_mes)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_artic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_locac)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_almac)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            // Table & Column Mappings
            this.ToTable("SARTICXALMACEN");
            this.Property(t => t.cc_anho).HasColumnName("cc_anho");
            this.Property(t => t.cc_mes).HasColumnName("cc_mes");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.cc_locac).HasColumnName("cc_locac");
            this.Property(t => t.cc_almac).HasColumnName("cc_almac");
            this.Property(t => t.fq_saldo_inicial).HasColumnName("fq_saldo_inicial");
            this.Property(t => t.fq_ingresos).HasColumnName("fq_ingresos");
            this.Property(t => t.fq_egresos).HasColumnName("fq_egresos");
            this.Property(t => t.fq_saldo_final).HasColumnName("fq_saldo_final");
            this.Property(t => t.fm_monto_ing_nac).HasColumnName("fm_monto_ing_nac");
            this.Property(t => t.fm_monto_sal_nac).HasColumnName("fm_monto_sal_nac");
            this.Property(t => t.fm_monto_ing_ext).HasColumnName("fm_monto_ing_ext");
            this.Property(t => t.fm_monto_sal_ext).HasColumnName("fm_monto_sal_ext");
            this.Property(t => t.fm_costo_prom_nac).HasColumnName("fm_costo_prom_nac");
            this.Property(t => t.fm_costo_prom_ext).HasColumnName("fm_costo_prom_ext");
            this.Property(t => t.fm_saldo_nac).HasColumnName("fm_saldo_nac");
            this.Property(t => t.fm_saldo_ext).HasColumnName("fm_saldo_ext");
            this.Property(t => t.fm_saldo_final_nac).HasColumnName("fm_saldo_final_nac");
            this.Property(t => t.fm_saldo_final_ext).HasColumnName("fm_saldo_final_ext");
            this.Property(t => t.fm_costo_nac).HasColumnName("fm_costo_nac");
            this.Property(t => t.fm_costo_ext).HasColumnName("fm_costo_ext");
            this.Property(t => t.fm_monto_ajus_nac).HasColumnName("fm_monto_ajus_nac");
            this.Property(t => t.fm_monto_ajus_ext).HasColumnName("fm_monto_ajus_ext");
            this.Property(t => t.fm_saldo_ant_ajus_nac).HasColumnName("fm_saldo_ant_ajus_nac");
            this.Property(t => t.fm_saldo_ant_ajus_ext).HasColumnName("fm_saldo_ant_ajus_ext");
        }
    }
}
