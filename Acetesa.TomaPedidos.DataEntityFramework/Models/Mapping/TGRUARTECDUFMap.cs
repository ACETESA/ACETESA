using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TgruartecdufMap : EntityTypeConfiguration<Tgruartecduf>
    {
        public TgruartecdufMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_gruartecduf);

            // Properties
            this.Property(t => t.cc_gruartecduf)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.cd_gruartduf)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("TGRUARTECDUF");
            this.Property(t => t.cc_gruartecduf).HasColumnName("cc_gruartecduf");
            this.Property(t => t.cd_gruartduf).HasColumnName("cd_gruartduf");
        }
    }
}
