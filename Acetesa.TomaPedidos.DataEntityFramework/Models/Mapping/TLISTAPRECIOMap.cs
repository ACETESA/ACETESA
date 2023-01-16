using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TLISTAPRECIOMap : EntityTypeConfiguration<TLISTAPRECIO>
    {
        public TLISTAPRECIOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cc_artic, t.cc_lista });

            // Properties
            this.Property(t => t.cc_artic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_lista)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_moneda)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_estado)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_destino)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.c_fl_afecto_percepcion)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("TLISTAPRECIO");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.cc_lista).HasColumnName("cc_lista");
            this.Property(t => t.fm_preciounit).HasColumnName("fm_preciounit");
            this.Property(t => t.cc_moneda).HasColumnName("cc_moneda");
            this.Property(t => t.fm_costounit).HasColumnName("fm_costounit");
            this.Property(t => t.fm_dcto1).HasColumnName("fm_dcto1");
            this.Property(t => t.fm_dcto2).HasColumnName("fm_dcto2");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.cc_destino).HasColumnName("cc_destino");
            this.Property(t => t.fm_preciounit_ant).HasColumnName("fm_preciounit_ant");
            this.Property(t => t.c_fl_afecto_percepcion).HasColumnName("c_fl_afecto_percepcion");
        }
    }
}
