using System;

namespace Acetesa.TomaPedidos.Entity
{
    public partial class tipo_cambio_diario
    {
        public DateTime d_f_tipo_cambio { get; set; }
        public double? n_i_valor_compra { get; set; }
        public double? n_i_val_venta { get; set; }
        public double? n_i_paralelo_venta { get; set; }
        public double? n_i_paralelo_compra { get; set; }
    }
}
