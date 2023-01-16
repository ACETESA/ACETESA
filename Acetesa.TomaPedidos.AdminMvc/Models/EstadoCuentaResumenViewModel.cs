using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class EstadoCuentaResumenViewModel
    {
        public EstadoCuentaResumenModel EstadoCuentaResumenModel { get; set; }
        public EstadoCuentaDetalleViewModel EstadoCuentaDetalleViewModel { get; set; }
    }
}