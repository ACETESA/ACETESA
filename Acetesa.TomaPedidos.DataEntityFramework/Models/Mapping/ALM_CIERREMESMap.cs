using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class AlmCierremesMap : EntityTypeConfiguration<AlmCierremes>
    {
        public AlmCierremesMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_anho = t.cc_anho, cc_mes = t.cc_mes });

            // Properties
            this.Property(t => t.cc_anho)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.cc_mes)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("ALM_CIERREMES");
            this.Property(t => t.cc_anho).HasColumnName("cc_anho");
            this.Property(t => t.cc_mes).HasColumnName("cc_mes");
        }
    }
}
