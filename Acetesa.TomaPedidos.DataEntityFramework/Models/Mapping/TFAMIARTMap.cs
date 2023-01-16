using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TfamiartMap : EntityTypeConfiguration<Tfamiart>
    {
        public TfamiartMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_famart);

            // Properties
            this.Property(t => t.cc_famart)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_famart)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.cb_obsoleto)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_critico)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_partnumber)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_afecto_percepcion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_comision)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_simbolo)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_tipo_exist)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("TFAMIART");
            this.Property(t => t.cc_famart).HasColumnName("cc_famart");
            this.Property(t => t.cd_famart).HasColumnName("cd_famart");
            this.Property(t => t.cb_obsoleto).HasColumnName("cb_obsoleto");
            this.Property(t => t.cb_critico).HasColumnName("cb_critico");
            this.Property(t => t.cb_partnumber).HasColumnName("cb_partnumber");
            this.Property(t => t.c_fl_afecto_percepcion).HasColumnName("c_fl_afecto_percepcion");
            this.Property(t => t.cb_comision).HasColumnName("cb_comision");
            this.Property(t => t.cc_simbolo).HasColumnName("cc_simbolo");
            this.Property(t => t.cc_tipo_exist).HasColumnName("cc_tipo_exist");
        }
    }
}
