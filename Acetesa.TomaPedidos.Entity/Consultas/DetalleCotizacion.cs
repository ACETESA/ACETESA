using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity.Consultas
{
    public class DetalleCotizacion
    {
        public string cc_artic { get; set; }
        public string cd_artic { get; set; }
        public string cc_unmed { get; set; }
        public string cc_lista { get; set; }
        public decimal fq_cantidad { get; set; }
        public double fq_peso_teorico { get; set; }
        public decimal fm_precio { get; set; }
        public decimal fm_precio2 { get; set; }
        public decimal fm_precio_fin { get; set; }
        public decimal fm_total { get; set; }
    }
}
