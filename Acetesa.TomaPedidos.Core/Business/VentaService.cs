using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class VentaService: IVentaService
    {
        private readonly IVentaRepository _ventaRepository;

        public VentaService(IVentaRepository ventaRepository)
        {
            if (ventaRepository == null)
            {
                throw new ArgumentNullException("ventaRepository");
            }
            _ventaRepository = ventaRepository;
        }
        public Dictionary<string, object> GetVentaStoreProcedure(DateTime fechaInicio, DateTime fechaFinal, string cliente, string vendedor)
        {
            if (string.IsNullOrEmpty(cliente) || string.IsNullOrWhiteSpace(cliente))
            {
                //throw new ArgumentNullException("cliente");
                cliente = "%";
            }
            if (string.IsNullOrEmpty(vendedor))
            {
                throw new ArgumentNullException("vendedor");
            }
            var result = _ventaRepository.GetVentaStoreProcedure(fechaInicio, fechaFinal, cliente, vendedor);
            return result;
        }

        public Dictionary<string, byte[]> RecuperarDocumentosPorComprobante(DocumentosModel documentosModel)
        {
            var result = _ventaRepository.RecuperarDocumentosPorComprobante(documentosModel);
            return result;
        }

    }
}

