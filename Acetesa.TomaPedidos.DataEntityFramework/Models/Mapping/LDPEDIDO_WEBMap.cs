using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class LDPEDIDO_WEBMap : EntityTypeConfiguration<LDPEDIDO_WEB>
    {
        public LDPEDIDO_WEBMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cn_pedido, t.cn_item });

            // Properties
            this.Property(t => t.cn_pedido)
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
                .HasPrecision(12,4);

            // Table & Column Mappings
            this.ToTable("LDPEDIDO_WEB");
            this.Property(t => t.cn_pedido).HasColumnName("cn_pedido");
            this.Property(t => t.cn_item).HasColumnName("cn_item");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.fq_cantidad).HasColumnName("fq_cantidad");
            this.Property(t => t.fq_peso).HasColumnName("fq_peso");
            this.Property(t => t.fq_stock).HasColumnName("fq_stock");
            this.Property(t => t.cc_lista).HasColumnName("cc_lista");
            this.Property(t => t.fm_precio).HasColumnName("fm_precio");
            this.Property(t => t.fm_precio_fin).HasColumnName("fm_precio_fin");
            this.Property(t => t.fm_total).HasColumnName("fm_total");

            // Relationships
            this.HasRequired(t => t.LCPEDIDO_WEB)
                .WithMany(t => t.LDPEDIDO_WEB)
                .HasForeignKey(d => d.cn_pedido);
            this.HasRequired(t => t.MARTICUL)
                .WithMany(t => t.LDPEDIDO_WEB)
                .HasForeignKey(d => d.cc_artic);
            this.HasRequired(t => t.TLISTAPRECIO)
                .WithMany(t => t.LDPEDIDO_WEB)
                .HasForeignKey(d => new { d.cc_artic, d.cc_lista });

        }
    }
}
