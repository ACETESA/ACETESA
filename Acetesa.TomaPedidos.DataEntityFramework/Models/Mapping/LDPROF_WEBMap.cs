using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class LDPROF_WEBMap : EntityTypeConfiguration<LDPROF_WEB>
    {
        public LDPROF_WEBMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cn_proforma, t.cn_item });

            // Properties
            this.Property(t => t.cn_proforma)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cn_item)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_artic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_lista)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.fm_precio)
                .HasPrecision(12, 4);

            this.Property(t => t.fm_precio_fin)
                .HasPrecision(12, 4);

            // Table & Column Mappings
            this.ToTable("LDPROF_WEB");
            this.Property(t => t.cn_proforma).HasColumnName("cn_proforma");
            this.Property(t => t.cn_item).HasColumnName("cn_item");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.fq_cantidad).HasColumnName("fq_cantidad");
            this.Property(t => t.fq_stock).HasColumnName("fq_stock");
            this.Property(t => t.cc_lista).HasColumnName("cc_lista");
            this.Property(t => t.fm_precio).HasColumnName("fm_precio");
            this.Property(t => t.fm_precio_fin).HasColumnName("fm_precio_fin");
            this.Property(t => t.fm_total).HasColumnName("fm_total");
            this.Property(t => t.fq_peso).HasColumnName("fq_peso");

            // Relationships
            this.HasRequired(t => t.LCPROF_WEB)
                .WithMany(t => t.LDPROF_WEB)
                .HasForeignKey(d => d.cn_proforma);
            this.HasRequired(t => t.MARTICUL)
                .WithMany(t => t.LDPROF_WEB)
                .HasForeignKey(d => d.cc_artic);
            this.HasRequired(t => t.TLISTAPRECIO)
                .WithMany(t => t.LDPROF_WEB)
                .HasForeignKey(d => new { d.cc_artic, d.cc_lista });

        }
    }
}
