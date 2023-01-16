using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;


namespace Acetesa.TomaPedidos.IRepository
{
    public interface IVentaRepository : IBase<LCPEDIDO_WEB>
    {
        Dictionary<string, object> GetVentaStoreProcedure(DateTime fechaInicio, DateTime fechaFinal, string cliente, string vendedor);
        Dictionary<string, byte[]> RecuperarDocumentosPorComprobante(DocumentosModel documentosModel);
    }
}
