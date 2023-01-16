using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TgruartecMap : EntityTypeConfiguration<Tgruartec>
    {
        public TgruartecMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_gruartec);

            // Properties
            this.Property(t => t.cc_gruartec)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cd_gruart)
                .IsFixedLength()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("TGRUARTEC");
            this.Property(t => t.cc_gruartec).HasColumnName("cc_gruartec");
            this.Property(t => t.cd_gruart).HasColumnName("cd_gruart");
        }
    }
}
