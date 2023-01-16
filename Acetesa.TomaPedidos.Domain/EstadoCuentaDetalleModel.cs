using System;

namespace Acetesa.TomaPedidos.Domain
{
    public class EstadoCuentaDetalleModel
    {
        public string Ruc { get; set; }
        public string Razon_Social { get; set; }
        public string TipoDocumento { get; set; }
        public string Nro_Documento { get; set; }
        public DateTime Fecha_Documento { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public decimal? Dias_Vcto { get; set; }
        public string Mon_Docum { get; set; }
        public decimal? Tot_Docum { get; set; }
        public decimal? Acta_Docum { get; set; }
        public decimal? Sal_Docum { get; set; }

    }
}
