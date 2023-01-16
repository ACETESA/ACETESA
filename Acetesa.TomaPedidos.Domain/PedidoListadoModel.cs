using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Domain
{
    public class PedidoListadoModel
    {
        public string cn_pedido { get; set; }
        public string cn_proforma { get; set; }
        public string cc_analis { get; set; }
        public string cd_razsoc { get; set; }
        public DateTime df_emision { get; set; }
        public decimal fm_totvta { get; set; }
        public string cb_estado { get; set; }
        public string archivoOC { get; set; }
    }
}
