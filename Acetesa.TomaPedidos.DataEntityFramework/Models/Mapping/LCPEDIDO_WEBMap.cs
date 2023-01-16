using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class LCPEDIDO_WEBMap : EntityTypeConfiguration<LCPEDIDO_WEB>
    {
        public LCPEDIDO_WEBMap()
        {
            // Primary Key
            this.HasKey(t => t.cn_pedido);

            // Properties
            this.Property(t => t.cn_pedido)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cn_proforma)
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

            this.Property(t => t.cn_suc)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

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

            this.Property(t => t.cb_recojo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_estado)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.fm_tipcam).HasPrecision(10, 4);

            // Table & Column Mappings
            this.ToTable("LCPEDIDO_WEB");
            this.Property(t => t.cn_pedido).HasColumnName("cn_pedido");
            this.Property(t => t.cn_proforma).HasColumnName("cn_proforma");
            this.Property(t => t.cc_tipana).HasColumnName("cc_tipana");
            this.Property(t => t.cc_analis).HasColumnName("cc_analis");
            this.Property(t => t.cn_suc).HasColumnName("cn_suc");
            this.Property(t => t.cd_razsoc).HasColumnName("cd_razsoc");
            this.Property(t => t.cc_moneda).HasColumnName("cc_moneda");
            this.Property(t => t.cc_vta).HasColumnName("cc_vta");
            this.Property(t => t.df_proceso).HasColumnName("df_proceso");
            this.Property(t => t.df_emision).HasColumnName("df_emision");
            this.Property(t => t.fm_tipcam).HasColumnName("fm_tipcam");
            this.Property(t => t.fm_valvta).HasColumnName("fm_valvta");
            this.Property(t => t.fm_igv).HasColumnName("fm_igv");
            this.Property(t => t.fm_totvta).HasColumnName("fm_totvta");
            this.Property(t => t.cb_recojo).HasColumnName("cb_recojo");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");

            // Relationships
            this.HasOptional(t => t.LCPROF_WEB)
                .WithMany(t => t.LCPEDIDO_WEB)
                .HasForeignKey(d => d.cn_proforma);
            this.HasRequired(t => t.TCONDVTA)
                .WithMany(t => t.LCPEDIDO_WEB)
                .HasForeignKey(d => d.cc_vta);
            this.HasRequired(t => t.TMONEDA)
                .WithMany(t => t.LCPEDIDO_WEB)
                .HasForeignKey(d => d.cc_moneda);
            this.HasRequired(t => t.TSUCCLIE)
                .WithMany(t => t.LCPEDIDO_WEB)
                .HasForeignKey(d => new { d.cc_tipana, d.cc_analis, d.cn_suc });

        }
    }
}
