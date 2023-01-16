using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
   public class CotizacionMotivoRechazo
    {
        public int idMotivo { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
    }
}
