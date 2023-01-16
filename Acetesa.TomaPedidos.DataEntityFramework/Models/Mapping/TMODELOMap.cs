using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TmodeloMap : EntityTypeConfiguration<Tmodelo>
    {
        public TmodeloMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_marca = t.cc_marca, cc_modelo = t.cc_modelo });

            // Properties
            this.Property(t => t.cc_marca)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.cc_modelo)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.cd_modelo)
                .IsFixedLength()
                .HasMaxLength(35);

            this.Property(t => t.ct_graf)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TMODELO");
            this.Property(t => t.cc_marca).HasColumnName("cc_marca");
            this.Property(t => t.cc_modelo).HasColumnName("cc_modelo");
            this.Property(t => t.cd_modelo).HasColumnName("cd_modelo");
            this.Property(t => t.ct_graf).HasColumnName("ct_graf");
        }
    }
}
