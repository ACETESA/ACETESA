using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.c_c_usuario);

            // Properties
            this.Property(t => t.c_c_usuario)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.c_t_password)
                .IsFixedLength()
                .HasMaxLength(8);

            this.Property(t => t.c_t_nombre)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.c_fl_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_t_cliente)
                .HasMaxLength(35);

            this.Property(t => t.C_C_NIV_SECUENCIA)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_c_niv_modifica)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_hextras)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cb_activo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_t_email)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.c_fl_anticipo)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_notpedvta)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_cdvta)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_limcred)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_verFact)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_verBole)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_verNota)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_HD)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.c_fl_clievincul)
                .HasMaxLength(1);

            this.Property(t => t.c_fl_pedalm)
                .HasMaxLength(1);

            this.Property(t => t.cb_aut_papsal)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.cc_usuario_bd)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.cc_password_bd)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.c_fl_cat_mat)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("USUARIOS");
            this.Property(t => t.c_c_usuario).HasColumnName("c_c_usuario");
            this.Property(t => t.c_t_password).HasColumnName("c_t_password");
            this.Property(t => t.c_t_nombre).HasColumnName("c_t_nombre");
            this.Property(t => t.c_fl_activo).HasColumnName("c_fl_activo");
            this.Property(t => t.c_t_cliente).HasColumnName("c_t_cliente");
            this.Property(t => t.C_C_NIV_SECUENCIA).HasColumnName("C_C_NIV_SECUENCIA");
            this.Property(t => t.c_c_niv_modifica).HasColumnName("c_c_niv_modifica");
            this.Property(t => t.d_f_conexion).HasColumnName("d_f_conexion");
            this.Property(t => t.c_fl_hextras).HasColumnName("c_fl_hextras");
            this.Property(t => t.cb_activo).HasColumnName("cb_activo");
            this.Property(t => t.c_t_email).HasColumnName("c_t_email");
            this.Property(t => t.c_fl_anticipo).HasColumnName("c_fl_anticipo");
            this.Property(t => t.c_fl_notpedvta).HasColumnName("c_fl_notpedvta");
            this.Property(t => t.c_fl_cdvta).HasColumnName("c_fl_cdvta");
            this.Property(t => t.c_fl_limcred).HasColumnName("c_fl_limcred");
            this.Property(t => t.c_fl_verFact).HasColumnName("c_fl_verFact");
            this.Property(t => t.c_fl_verBole).HasColumnName("c_fl_verBole");
            this.Property(t => t.c_fl_verNota).HasColumnName("c_fl_verNota");
            this.Property(t => t.c_fl_HD).HasColumnName("c_fl_HD");
            this.Property(t => t.c_fl_clievincul).HasColumnName("c_fl_clievincul");
            this.Property(t => t.c_fl_pedalm).HasColumnName("c_fl_pedalm");
            this.Property(t => t.cb_aut_papsal).HasColumnName("cb_aut_papsal");
            this.Property(t => t.cc_usuario_bd).HasColumnName("cc_usuario_bd");
            this.Property(t => t.cc_password_bd).HasColumnName("cc_password_bd");
            this.Property(t => t.c_fl_cat_mat).HasColumnName("c_fl_cat_mat");
        }
    }


}
