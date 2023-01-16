using System;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class Usuario
    {
        public string c_c_usuario { get; set; }
        public string c_t_password { get; set; }
        public string c_t_nombre { get; set; }
        public string c_fl_activo { get; set; }
        public string c_t_cliente { get; set; }
        public string C_C_NIV_SECUENCIA { get; set; }
        public string c_c_niv_modifica { get; set; }
        public DateTime? d_f_conexion { get; set; }
        public string c_fl_hextras { get; set; }
        public string cb_activo { get; set; }
        public string c_t_email { get; set; }
        public string c_fl_anticipo { get; set; }
        public string c_fl_notpedvta { get; set; }
        public string c_fl_cdvta { get; set; }
        public string c_fl_limcred { get; set; }
        public string c_fl_verFact { get; set; }
        public string c_fl_verBole { get; set; }
        public string c_fl_verNota { get; set; }
        public string c_fl_HD { get; set; }
        public string c_fl_clievincul { get; set; }
        public string c_fl_pedalm { get; set; }
        public string cb_aut_papsal { get; set; }
        public string cc_usuario_bd { get; set; }
        public string cc_password_bd { get; set; }
        public string c_fl_cat_mat { get; set; }
    }
}
