using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TalmacenMap : EntityTypeConfiguration<Talmacen>
    {
        public TalmacenMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_almac = t.cc_almac, cc_tipana = t.cc_tipana, cc_locac = t.cc_locac, cc_analis = t.cc_analis, cc_cta = t.cc_cta, cc_encar1 = t.cc_encar1, cc_encar2 = t.cc_encar2 });

            // Properties
            this.Property(t => t.cc_almac)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cc_tipana)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_locac)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_catalm)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_analis)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cc_cta)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(9);

            this.Property(t => t.cc_encar1)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cd_direc)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.cc_encar2)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cb_princ)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ci_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_prefijo)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_estado)
                .HasMaxLength(1);

            this.Property(t => t.cd_direccion)
                .HasMaxLength(80);

            this.Property(t => t.cc_ubic_generic)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.cc_tienda)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_valorizar)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cod_establecimiento)
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("TALMACEN");
            this.Property(t => t.cc_almac).HasColumnName("cc_almac");
            this.Property(t => t.cc_tipana).HasColumnName("cc_tipana");
            this.Property(t => t.cc_locac).HasColumnName("cc_locac");
            this.Property(t => t.cc_catalm).HasColumnName("cc_catalm");
            this.Property(t => t.cc_analis).HasColumnName("cc_analis");
            this.Property(t => t.cc_cta).HasColumnName("cc_cta");
            this.Property(t => t.cc_encar1).HasColumnName("cc_encar1");
            this.Property(t => t.cd_direc).HasColumnName("cd_direc");
            this.Property(t => t.cc_encar2).HasColumnName("cc_encar2");
            this.Property(t => t.cb_princ).HasColumnName("cb_princ");
            this.Property(t => t.ci_activo).HasColumnName("ci_activo");
            this.Property(t => t.fn_areas).HasColumnName("fn_areas");
            this.Property(t => t.fn_zonas).HasColumnName("fn_zonas");
            this.Property(t => t.fn_anaq).HasColumnName("fn_anaq");
            this.Property(t => t.fn_and).HasColumnName("fn_and");
            this.Property(t => t.fn_casil).HasColumnName("fn_casil");
            this.Property(t => t.fm_presup).HasColumnName("fm_presup");
            this.Property(t => t.fm_por_presup).HasColumnName("fm_por_presup");
            this.Property(t => t.cc_prefijo).HasColumnName("cc_prefijo");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.cd_direccion).HasColumnName("cd_direccion");
            this.Property(t => t.cc_ubic_generic).HasColumnName("cc_ubic_generic");
            this.Property(t => t.cc_tienda).HasColumnName("cc_tienda");
            this.Property(t => t.cb_valorizar).HasColumnName("cb_valorizar");
            this.Property(t => t.cod_establecimiento).HasColumnName("cod_establecimiento");
        }
    }
}
