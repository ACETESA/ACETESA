using System.Linq;
using Acetesa.TomaPedidos.Entity;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface ISucursalClienteRepository : IBase<TSUCCLIE>
    {
        IQueryable<TSUCCLIE> GetByCcAnalis(string ccAnalis);
        IQueryable<TSUCCLIE> GetByCcAnalisCnSuc(string ccAnalis, string cnSuc);

        IEnumerable<TLUGCLIE> GetLugarEntregaByCcAnalis(string ccAnalis);
        IEnumerable<TTRASPORTE> GetTransporteByCcAnalis(string ccAnalis);
        List<SucursalClienteModel> RecuperarSucursalPorClienteID(string cc_analis);

    }
}
