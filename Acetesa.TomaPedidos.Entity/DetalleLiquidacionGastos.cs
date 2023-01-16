using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class DetalleLiquidacionGastos
    {
        public string fechaEvento { get; set; }
        public string descripcion { get; set; }
        public string ruc { get; set; }
        public string razonSocial { get; set; }
        public string numeroFactura { get; set; }
        public int tipoGastoID { get; set; }
        public decimal montoFactura { get; set; }
        public byte[] documento { get; set; }
    }
}
