using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TCONDVTAMap : EntityTypeConfiguration<TCONDVTA>
    {
        public TCONDVTAMap()
        {
            // Primary Key
            this.HasKey(t => t.cc_vta);

            // Properties
            this.Property(t => t.cc_vta)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.cd_vta)
                .IsFixedLength()
                .HasMaxLength(60);

            this.Property(t => t.cb_gasto)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_condvta)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_no_fact)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cb_todos)
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TCONDVTA");
            this.Property(t => t.cc_vta).HasColumnName("cc_vta");
            this.Property(t => t.cd_vta).HasColumnName("cd_vta");
            this.Property(t => t.fq_pond).HasColumnName("fq_pond");
            this.Property(t => t.fq_diasplazo).HasColumnName("fq_diasplazo");
            this.Property(t => t.fm_porcmin).HasColumnName("fm_porcmin");
            this.Property(t => t.fm_porcmax).HasColumnName("fm_porcmax");
            this.Property(t => t.cb_gasto).HasColumnName("cb_gasto");
            this.Property(t => t.fq_dias).HasColumnName("fq_dias");
            this.Property(t => t.fq_letras).HasColumnName("fq_letras");
            this.Property(t => t.cb_condvta).HasColumnName("cb_condvta");
            this.Property(t => t.cb_no_fact).HasColumnName("cb_no_fact");
            this.Property(t => t.cb_todos).HasColumnName("cb_todos");
        }
    }
}
