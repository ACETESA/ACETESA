using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TsubgruartecMap : EntityTypeConfiguration<Tsubgruartec>
    {
        public TsubgruartecMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_gruartec = t.cc_gruartec, cc_subgruart = t.cc_subgruart });

            // Properties
            this.Property(t => t.cc_gruartec)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cc_subgruart)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cd_subgruart)
                .IsFixedLength()
                .HasMaxLength(35);

            // Table & Column Mappings
            this.ToTable("TSUBGRUARTEC");
            this.Property(t => t.cc_gruartec).HasColumnName("cc_gruartec");
            this.Property(t => t.cc_subgruart).HasColumnName("cc_subgruart");
            this.Property(t => t.cd_subgruart).HasColumnName("cd_subgruart");
        }
    }
}
