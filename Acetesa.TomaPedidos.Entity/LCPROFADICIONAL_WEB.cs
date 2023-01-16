using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class LCPROFADICIONAL_WEB
    {
        public string cc_tienda { get; set; }
        public int igv_bo { get; set; }
        public string cn_suc { get; set; }
        public string cn_contacto { get; set; }
        public string nombreContacto { get; set; }
        public string emailContacto { get; set; }
        public string telefonoContacto { get; set; }
        public int imprimirPrecioTN { get; set; }
        public string observacion { get; set; }
        public int zonaLiberada { get; set; }
    }
}