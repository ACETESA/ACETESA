using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class LCPROF_WEBMap : EntityTypeConfiguration<LCPROF_WEB>
    {
        public LCPROF_WEBMap()
        {
            // Primary Key
            this.HasKey(t => t.cn_proforma);

            // Properties
            this.Property(t => t.cn_proforma)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cc_tipana)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_analis)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cd_razsoc)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.cc_moneda)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_vta)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.fm_tipcam).HasPrecision(10, 4);

            this.Property(t => t.cb_estado)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            //this.Property(t => t.cd_atencion)
            //    .IsRequired()
            //    .IsFixedLength()
            //    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("LCPROF_WEB");
            this.Property(t => t.cn_proforma).HasColumnName("cn_proforma");
            this.Property(t => t.cc_tipana).HasColumnName("cc_tipana");
            this.Property(t => t.cc_analis).HasColumnName("cc_analis");
            this.Property(t => t.cd_razsoc).HasColumnName("cd_razsoc");
            this.Property(t => t.cc_moneda).HasColumnName("cc_moneda");
            this.Property(t => t.cc_vta).HasColumnName("cc_vta");
            this.Property(t => t.df_proceso).HasColumnName("df_proceso");
            this.Property(t => t.df_emision).HasColumnName("df_emision");
            this.Property(t => t.fm_tipcam).HasColumnName("fm_tipcam");
            this.Property(t => t.fm_valvta).HasColumnName("fm_valvta");
            this.Property(t => t.fm_igv).HasColumnName("fm_igv");
            this.Property(t => t.fm_totvta).HasColumnName("fm_totvta");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            //this.Property(t => t.cd_atencion).HasColumnName("cd_atencion");

            // Relationships
            this.HasRequired(t => t.MCLIENTE)
                .WithMany(t => t.LCPROF_WEB)
                .HasForeignKey(d => new { d.cc_tipana, d.cc_analis });
            this.HasRequired(t => t.TCONDVTA)
                .WithMany(t => t.LCPROF_WEB)
                .HasForeignKey(d => d.cc_vta);
            this.HasRequired(t => t.TMONEDA)
                .WithMany(t => t.LCPROF_WEB)
                .HasForeignKey(d => d.cc_moneda);

        }
    }
}
