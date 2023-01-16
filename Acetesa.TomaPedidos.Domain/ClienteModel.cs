namespace Acetesa.TomaPedidos.Domain
{
    public class ClienteModel
    {
        public string cc_tipana { get; set; }
        public string cc_analis { get; set; }
        public string cd_razsoc { get; set; }
        public string cd_direc { get; set; }
        public string ct_email { get; set; }
        public string cn_telf1 { get; set; }
        public class VendedorCliente {
            public string vendedor { get; set; }
            public string fechaAsignacion { get; set; }
        }
    }
}
