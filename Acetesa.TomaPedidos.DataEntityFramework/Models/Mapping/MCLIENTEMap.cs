using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class MCLIENTEMap : EntityTypeConfiguration<MCLIENTE>
    {
        public MCLIENTEMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cc_tipana, t.cc_analis });

            // Properties
            this.Property(t => t.cc_tipana)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_analis)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cc_pais)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_dpto)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_prov)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_sector)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_catclie)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cc_distrito)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cd_razsoc)
                .IsFixedLength()
                .HasMaxLength(65);

            this.Property(t => t.cc_zona)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.cb_proced)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cd_direc)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.cn_regind)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_sanit)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.cn_regmerc)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.ct_giro)
                .IsFixedLength()
                .HasMaxLength(85);

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

            this.Property(t => t.cb_monfac)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_limcred)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_cheque)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_sucursal)
                .IsFixedLength()
                .HasMaxLength(18);

            this.Property(t => t.cb_sector)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ct_legal)
                .IsFixedLength()
                .HasMaxLength(45);

            this.Property(t => t.cn_rucleg)
                .IsFixedLength()
                .HasMaxLength(11);

            this.Property(t => t.cd_direcleg)
                .IsFixedLength()
                .HasMaxLength(45);

            this.Property(t => t.cn_telfleg)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cn_faxleg)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.cb_cheqdif)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_flete)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_embalaje)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_moncred)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cd_nomcom)
                .IsFixedLength()
                .HasMaxLength(45);

            this.Property(t => t.cd_appaterno)
                .HasMaxLength(30);

            this.Property(t => t.cd_apmaterno)
                .HasMaxLength(30);

            this.Property(t => t.cd_nombre1)
                .HasMaxLength(30);

            this.Property(t => t.cd_nombre2)
                .HasMaxLength(30);

            this.Property(t => t.c_fl_agente_percepcion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_proyecto)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_cod_documento_identidad)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_vinculacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ct_email)
                .IsFixedLength()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("MCLIENTE");
            this.Property(t => t.cc_tipana).HasColumnName("cc_tipana");
            this.Property(t => t.cc_analis).HasColumnName("cc_analis");
            this.Property(t => t.cc_pais).HasColumnName("cc_pais");
            this.Property(t => t.cc_dpto).HasColumnName("cc_dpto");
            this.Property(t => t.cc_prov).HasColumnName("cc_prov");
            this.Property(t => t.cc_sector).HasColumnName("cc_sector");
            this.Property(t => t.cc_catclie).HasColumnName("cc_catclie");
            this.Property(t => t.cc_distrito).HasColumnName("cc_distrito");
            this.Property(t => t.cd_razsoc).HasColumnName("cd_razsoc");
            this.Property(t => t.cc_zona).HasColumnName("cc_zona");
            this.Property(t => t.cb_proced).HasColumnName("cb_proced");
            this.Property(t => t.cd_direc).HasColumnName("cd_direc");
            this.Property(t => t.cn_regind).HasColumnName("cn_regind");
            this.Property(t => t.cn_sanit).HasColumnName("cn_sanit");
            this.Property(t => t.cn_regmerc).HasColumnName("cn_regmerc");
            this.Property(t => t.ct_giro).HasColumnName("ct_giro");
            this.Property(t => t.dt_constit).HasColumnName("dt_constit");
            this.Property(t => t.dt_registro).HasColumnName("dt_registro");
            this.Property(t => t.cn_telf1).HasColumnName("cn_telf1");
            this.Property(t => t.cn_telf2).HasColumnName("cn_telf2");
            this.Property(t => t.cn_telf3).HasColumnName("cn_telf3");
            this.Property(t => t.cn_fax1).HasColumnName("cn_fax1");
            this.Property(t => t.cn_fax2).HasColumnName("cn_fax2");
            this.Property(t => t.cn_fax3).HasColumnName("cn_fax3");
            this.Property(t => t.cb_monfac).HasColumnName("cb_monfac");
            this.Property(t => t.cb_limcred).HasColumnName("cb_limcred");
            this.Property(t => t.cb_cheque).HasColumnName("cb_cheque");
            this.Property(t => t.cb_sucursal).HasColumnName("cb_sucursal");
            this.Property(t => t.cb_sector).HasColumnName("cb_sector");
            this.Property(t => t.cb_activo).HasColumnName("cb_activo");
            this.Property(t => t.dt_ultcomp).HasColumnName("dt_ultcomp");
            this.Property(t => t.ct_legal).HasColumnName("ct_legal");
            this.Property(t => t.cn_rucleg).HasColumnName("cn_rucleg");
            this.Property(t => t.dt_ultdeuda).HasColumnName("dt_ultdeuda");
            this.Property(t => t.fm_compmn).HasColumnName("fm_compmn");
            this.Property(t => t.cd_direcleg).HasColumnName("cd_direcleg");
            this.Property(t => t.fm_compme).HasColumnName("fm_compme");
            this.Property(t => t.cn_telfleg).HasColumnName("cn_telfleg");
            this.Property(t => t.fm_acummn).HasColumnName("fm_acummn");
            this.Property(t => t.cn_faxleg).HasColumnName("cn_faxleg");
            this.Property(t => t.fm_acumme).HasColumnName("fm_acumme");
            this.Property(t => t.fm_saldomn).HasColumnName("fm_saldomn");
            this.Property(t => t.fm_saldome).HasColumnName("fm_saldome");
            this.Property(t => t.fq_descto).HasColumnName("fq_descto");
            this.Property(t => t.fm_cantporc).HasColumnName("fm_cantporc");
            this.Property(t => t.fm_desctomn).HasColumnName("fm_desctomn");
            this.Property(t => t.fm_montporcmn).HasColumnName("fm_montporcmn");
            this.Property(t => t.fm_desctome).HasColumnName("fm_desctome");
            this.Property(t => t.fm_montporcme).HasColumnName("fm_montporcme");
            this.Property(t => t.cb_cheqdif).HasColumnName("cb_cheqdif");
            this.Property(t => t.fm_diferidomn).HasColumnName("fm_diferidomn");
            this.Property(t => t.fm_diferidome).HasColumnName("fm_diferidome");
            this.Property(t => t.cb_flete).HasColumnName("cb_flete");
            this.Property(t => t.cb_embalaje).HasColumnName("cb_embalaje");
            this.Property(t => t.cb_moncred).HasColumnName("cb_moncred");
            this.Property(t => t.fm_limcred).HasColumnName("fm_limcred");
            this.Property(t => t.cd_nomcom).HasColumnName("cd_nomcom");
            this.Property(t => t.cd_appaterno).HasColumnName("cd_appaterno");
            this.Property(t => t.cd_apmaterno).HasColumnName("cd_apmaterno");
            this.Property(t => t.cd_nombre1).HasColumnName("cd_nombre1");
            this.Property(t => t.cd_nombre2).HasColumnName("cd_nombre2");
            this.Property(t => t.c_fl_agente_percepcion).HasColumnName("c_fl_agente_percepcion");
            this.Property(t => t.cc_proyecto).HasColumnName("cc_proyecto");
            this.Property(t => t.c_cod_documento_identidad).HasColumnName("c_cod_documento_identidad");
            this.Property(t => t.c_fl_vinculacion).HasColumnName("c_fl_vinculacion");
            this.Property(t => t.ct_email).HasColumnName("ct_email");
        }
    }
}
