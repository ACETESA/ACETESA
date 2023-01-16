using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TtipoarticMap : EntityTypeConfiguration<Ttipoartic>
    {
        public TtipoarticMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_tipoartic);

            // Properties
            this.Property(t => t.cc_tipoartic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cd_tipoartic)
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TTIPOARTIC");
            this.Property(t => t.cc_tipoartic).HasColumnName("cc_tipoartic");
            this.Property(t => t.cd_tipoartic).HasColumnName("cd_tipoartic");
        }
    }
}
