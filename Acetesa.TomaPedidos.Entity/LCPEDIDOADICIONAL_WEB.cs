using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class LCPEDIDOADICIONAL_WEB
    {
        public string Cn_lug { get; set; }
        public string IdContactoEntregaDirecta { get; set; }
        public string cc_tienda { get; set; }
        public int igv_bo { get; set; }
        public string cd_direc { get; set; }

        public string CC_transp { get; set; }
        public string cd_transp { get; set; }
        public string ct_direccion { get; set; }
        public string cd_email { get; set; }
        public string cn_telefono1 { get; set; }
        public string ct_contacto { get; set; }

        public string Vt_observacion { get; set; }
        public string Vt_observacionGuia { get; set; }
        public string ContactoTransporte { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoTelefono1 { get; set; }
        public string ContactoTelefono2 { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string cn_ocompra { get; set; }
        public string cd_tienda { get; set; }
        public int zonaLiberada { get; set; }
    }
}
