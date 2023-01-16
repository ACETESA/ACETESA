using System;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IProductoRepository : IBase<MARTICUL>
    {
        IEnumerable<Tlistaprec> GetListaPreciosSp();
        IEnumerable<Tgruartec> GetFamiliasSp();
        IEnumerable<SubFamiliaModel> GetSubFamiliasByCodFamiliaSp(string codFamilia);
        IEnumerable<Ttipoartic> GetTiposSp();
        IEnumerable<StockModel> GetStockSp(DateTime fechaDelDia, string codFamilia, string codSubFamilia, string codigoTipoArticulo);
        IEnumerable<PrecioModel> GetPreciosSp(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks);

        //Jpastor
        IEnumerable<StockPorGrupoModel> GetStockPorGrupoSp(string sEmpresa, string sTienda, string codFamilia, string codSubGrupo, string codigoTipoArticulo, DateTime fecha);

        //jlazaro
        Dictionary<string, object> GetStockStoreProcedure(DateTime fechaDelDia, string codFamilia, string codSubFamilia, string codigoTipoArticulo);
        List<object> GetEstadisticaVentas(DateTime sFechaInicio, DateTime sFechaFinal, string sVendedor = "", string sEmpresa = "", string sTienda ="");
        List<Tgruartec> ListarFamiliasArticulosPorEmpresa(string empresa);
        List<SubFamiliaModel> ListarSubFamiliasArticulosPorEmpresa(string empresa, string cc_gruart);
        List<Ttipoartic> ListarTipoArticulosPorEmpresa(string empresa);

        List<Tlistaprec> ListarPreciosArticulosPorEmpresa(string empresa);
        List<PrecioModel> ListarPreciosArticulosPorGrupoEmpresa(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks, string empresa);


    }
}
