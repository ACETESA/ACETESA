using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TsubgruartecdufMap : EntityTypeConfiguration<Tsubgruartecduf>
    {
        public TsubgruartecdufMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_gruartecduf = t.cc_gruartecduf, cc_subgruartduf = t.cc_subgruartduf });

            // Properties
            this.Property(t => t.cc_gruartecduf)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.cc_subgruartduf)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.cd_subgruartduf)
                .HasMaxLength(35);

            // Table & Column Mappings
            this.ToTable("TSUBGRUARTECDUF");
            this.Property(t => t.cc_gruartecduf).HasColumnName("cc_gruartecduf");
            this.Property(t => t.cc_subgruartduf).HasColumnName("cc_subgruartduf");
            this.Property(t => t.cd_subgruartduf).HasColumnName("cd_subgruartduf");
        }
    }
}
