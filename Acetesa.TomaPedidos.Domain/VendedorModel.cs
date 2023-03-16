using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Domain
{
    public class VendedorModel
    {
        public string cc_tipanavend { get; set; }
        public string cc_analisvend { get; set; }
        public string ct_nombres { get; set; }
        public string ct_appaterno { get; set; }
        public string cn_telf { get; set; }
        public string cn_telfref { get; set; }
        public string ct_email { get; set; }

        public string ct_nombreCompleto
        {
            get
            {
                return ct_nombres.Trim() + " " + ct_appaterno.Trim();
            }
        }

        public string cn_telefonos
        {
            get
            {
                return cn_telf.Trim() + " / " + cn_telfref.Trim();
            }
        }

        public class CorreoVendedor
        {
            public string correo { get; set; }
            public string clave { get; set; }
            public string llave { get; set; }
        }

        public CorreoVendedor correoVendedor = new CorreoVendedor();
    }
}
