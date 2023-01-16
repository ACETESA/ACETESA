using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class LkardexMap : EntityTypeConfiguration<Lkardex>
    {
        public LkardexMap()
        {
            // Primary Key
            this.HasKey(t => new { cc_kardex = t.cc_kardex, cc_artic = t.cc_artic, cc_almac = t.cc_almac, ni_corre = t.ni_corre });

            // Properties
            this.Property(t => t.cc_kardex)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.cc_artic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cc_almac)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.cb_tipo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_notsal)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.cc_noting)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.cb_estado)
                .HasMaxLength(1);

            this.Property(t => t.ni_corre)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.cn_artxpaq)
                .IsFixedLength()
                .HasMaxLength(12);

            this.Property(t => t.cc_movi)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_servicio)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("LKARDEX");
            this.Property(t => t.cc_kardex).HasColumnName("cc_kardex");
            this.Property(t => t.cc_artic).HasColumnName("cc_artic");
            this.Property(t => t.cc_almac).HasColumnName("cc_almac");
            this.Property(t => t.df_kardex).HasColumnName("df_kardex");
            this.Property(t => t.nq_artic).HasColumnName("nq_artic");
            this.Property(t => t.fm_artic).HasColumnName("fm_artic");
            this.Property(t => t.fm_artic_d).HasColumnName("fm_artic_d");
            this.Property(t => t.cb_tipo).HasColumnName("cb_tipo");
            this.Property(t => t.fm_ultpu).HasColumnName("fm_ultpu");
            this.Property(t => t.fm_ultpu_d).HasColumnName("fm_ultpu_d");
            this.Property(t => t.cc_notsal).HasColumnName("cc_notsal");
            this.Property(t => t.cc_noting).HasColumnName("cc_noting");
            this.Property(t => t.cb_estado).HasColumnName("cb_estado");
            this.Property(t => t.ni_corre).HasColumnName("ni_corre");
            this.Property(t => t.fm_costo_prom).HasColumnName("fm_costo_prom");
            this.Property(t => t.fm_costo_prom_d).HasColumnName("fm_costo_prom_d");
            this.Property(t => t.cn_artxpaq).HasColumnName("cn_artxpaq");
            this.Property(t => t.cc_movi).HasColumnName("cc_movi");
            this.Property(t => t.cb_servicio).HasColumnName("cb_servicio");
        }
    }
}
