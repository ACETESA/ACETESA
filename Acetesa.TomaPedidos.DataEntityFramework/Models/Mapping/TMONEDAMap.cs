using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TMONEDAMap : EntityTypeConfiguration<TMONEDA>
    {
        public TMONEDAMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_moneda);

            // Properties
            this.Property(t => t.cc_moneda)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_moneda)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.cb_nac)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cd_simbolo)
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.cb_moncambio)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_indmon)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("TMONEDA");
            this.Property(t => t.cc_moneda).HasColumnName("cc_moneda");
            this.Property(t => t.cd_moneda).HasColumnName("cd_moneda");
            this.Property(t => t.cb_nac).HasColumnName("cb_nac");
            this.Property(t => t.cd_simbolo).HasColumnName("cd_simbolo");
            this.Property(t => t.cb_moncambio).HasColumnName("cb_moncambio");
            this.Property(t => t.cb_indmon).HasColumnName("cb_indmon");
        }
    }
}
