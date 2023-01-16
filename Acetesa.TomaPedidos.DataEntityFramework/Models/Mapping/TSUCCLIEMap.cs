using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class TSUCCLIEMap : EntityTypeConfiguration<TSUCCLIE>
    {
        public TSUCCLIEMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cc_tipana, t.cc_analis, t.cn_suc });

            // Properties
            this.Property(t => t.cc_tipana)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_analis)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cn_suc)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_dpto)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_suc)
                .IsFixedLength()
                .HasMaxLength(65);

            this.Property(t => t.ct_email)
                .IsFixedLength()
                .HasMaxLength(45);

            this.Property(t => t.cc_prov)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_distrito)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_zona)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_direc)
                .IsFixedLength()
                .HasMaxLength(60);

            this.Property(t => t.cn_telf1)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_telf2)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_telf3)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_fax1)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_fax2)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_fax3)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cb_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("TSUCCLIE");
            this.Property(t => t.cc_tipana).HasColumnName("cc_tipana");
            this.Property(t => t.cc_analis).HasColumnName("cc_analis");
            this.Property(t => t.cn_suc).HasColumnName("cn_suc");
            this.Property(t => t.cc_dpto).HasColumnName("cc_dpto");
            this.Property(t => t.cd_suc).HasColumnName("cd_suc");
            this.Property(t => t.ct_email).HasColumnName("ct_email");
            this.Property(t => t.cc_prov).HasColumnName("cc_prov");
            this.Property(t => t.cc_distrito).HasColumnName("cc_distrito");
            this.Property(t => t.cc_zona).HasColumnName("cc_zona");
            this.Property(t => t.cd_direc).HasColumnName("cd_direc");
            this.Property(t => t.cn_telf1).HasColumnName("cn_telf1");
            this.Property(t => t.cn_telf2).HasColumnName("cn_telf2");
            this.Property(t => t.cn_telf3).HasColumnName("cn_telf3");
            this.Property(t => t.cn_fax1).HasColumnName("cn_fax1");
            this.Property(t => t.cn_fax2).HasColumnName("cn_fax2");
            this.Property(t => t.cn_fax3).HasColumnName("cn_fax3");
            this.Property(t => t.cb_activo).HasColumnName("cb_activo");
        }
    }
}
