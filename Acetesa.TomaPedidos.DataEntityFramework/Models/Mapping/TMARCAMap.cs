using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TmarcaMap : EntityTypeConfiguration<Tmarca>
    {
        public TmarcaMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_marca);

            // Properties
            this.Property(t => t.cc_marca)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.cd_marca)
                .IsFixedLength()
                .HasMaxLength(35);

            this.Property(t => t.cb_estado)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_tipo)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("TMARCA");
            this.Property(t => t.cc_marca).HasColumnName("cc_marca");
            this.Property(t => t.cd_marca).HasColumnName("cd_marca");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.cb_tipo).HasColumnName("cb_tipo");
        }
    }
}
