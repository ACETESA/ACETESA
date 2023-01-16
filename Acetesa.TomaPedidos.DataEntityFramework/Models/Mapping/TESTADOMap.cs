using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TESTADOMap : EntityTypeConfiguration<TESTADO>
    {
        public TESTADOMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_estado);

            // Properties
            this.Property(t => t.cc_estado)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_estado)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.cb_estado)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_usrcreacion)
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cc_usrmodifica)
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cb_fijo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cn_orden)
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("TESTADO");
            this.Property(t => t.cc_estado).HasColumnName("cc_estado");
            this.Property(t => t.cd_estado).HasColumnName("cd_estado");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.cc_usrcreacion).HasColumnName("cc_usrcreacion");
            this.Property(t => t.df_creacion).HasColumnName("df_creacion");
            this.Property(t => t.cc_usrmodifica).HasColumnName("cc_usrmodifica");
            this.Property(t => t.df_modifica).HasColumnName("df_modifica");
            this.Property(t => t.cb_fijo).HasColumnName("cb_fijo");
            this.Property(t => t.cn_orden).HasColumnName("cn_orden");
        }
    }
}
