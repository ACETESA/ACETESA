using System;

namespace Acetesa.TomaPedidos.Entity
{
   public class CarteraCliente
    {
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Sector { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public bool Asignado { get; set; }
        //public decimal MontoLimiteCoberturado { get; set; }
        //public decimal MontoLimiteDiscrecional { get; set; }
        //public decimal MontoLimiteInterno { get; set; }
        public decimal MontoTotalLimite { get; set; }
        //public string FechaUltVenta { get; set; }
        public decimal MontoDeuda { get; set; }
        public string Aseguradora { get; set; }

    }
}
