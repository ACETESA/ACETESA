using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class TCONTACLIE
    {
        public string ct_nombres { get; set; }
        //public string ct_appaterno { get; set; }
        //public string ct_apmaterno { get; set; }
        public string cd_cargo_laboral { get; set; }
        public string cn_telf1 { get; set; }
        public string cn_telf2 { get; set; }
        public string cd_email { get; set; }
        public bool cbContVenta { get; set; }
        public bool cbContCobranza { get; set; }
        public bool cbContAlmacen { get; set; }
        public string cb_envio_docum { get; set; }
        public string cn_contacto { get; set; }
        public string cd_contacto { get; set; }
        public string cn_suc { get; set; }
        public string cc_analis { get; set; }
    }
}
