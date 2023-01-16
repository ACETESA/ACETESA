using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class EstadoCuentaDetalleViewModel
    {
        public IEnumerable<EstadoCuentaDetalleModel> EstadoCuentaDetalleModels { get; set; }
    }
}