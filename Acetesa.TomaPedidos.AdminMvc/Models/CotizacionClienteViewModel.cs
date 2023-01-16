using Acetesa.TomaPedidos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class CotizacionClienteViewModel
    {
        public LCPROF_WEB Cotizacion { get; set; }
        public LCPROFADICIONAL_WEB Adicional { get; set; }
    }
}