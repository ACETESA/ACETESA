using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ISucursalClienteService
    {
        IEnumerable<SucursalClienteModel> GetByCcAnalis(string ccAnalis);
        IEnumerable<SucursalClienteModel> GetByCcAnalisCnSuc(string ccAnalis, string cnSuc);

        IEnumerable<SucursalClienteModel> GetLugarEntregaByCcAnalis(string ccAnalis);
        IEnumerable<SucursalClienteModel> GetTransporteByCcAnalis(string ccAnalis);
    }
}
