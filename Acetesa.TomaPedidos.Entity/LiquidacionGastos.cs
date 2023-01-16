using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class LiquidacionGastos
    {
        public int LiquidacionGastosID { get; set; }
        public string AlternoID { get; set; }
        public string Asunto { get; set; }
        public string Destino { get; set; }
        public string FechaInicioViaje { get; set; }
        public string FechaFinViaje { get; set; }
        public decimal GastoTotal { get; set; }
        public string Observaciones { get; set; }
        public decimal Viaticos { get; set; }

    }
}
