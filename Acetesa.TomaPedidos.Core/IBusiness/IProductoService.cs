using System;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IProductoService
    {
        IEnumerable<Tlistaprec> GetListaPreciosSp();
        IEnumerable<Tgruartec> GetFamiliasSp();
        IEnumerable<SubFamiliaModel> GetSubFamiliasByCodFamiliaSp(string codFamilia);
        IEnumerable<Ttipoartic> GetTipoProductoSp();
        IEnumerable<StockModel> GetStockProductosSp(DateTime fechaDia,string codFamilia, string codSubFamilia, string tipo);
        IEnumerable<PrecioModel> GetPreciosProductosSp(string codListaPrecio,string codFamilia, string codSubFamilia, int Stocks);
        //jpastor
        IEnumerable<StockPorGrupoModel> GetStockPorGrupoProductoSp(string sEmpresa, string sTienda, string codFamilia, string codSubGrupo, string tipo, DateTime fecha);

        //jlazaro
        Dictionary<string, object> GetStockProductosStoreProcesure(DateTime fechaDia, string codFamilia, string codSubFamilia, string tipo);
        List<object> GetEstadisticaVentas(DateTime sFechaInicio, DateTime sFechaFinal, string sVendedor = "", string sEmpresa = "", string sTienda ="");
        List<Tgruartec> ListarFamiliasArticulosPorEmpresa(string empresa);
        List<SubFamiliaModel> ListarSubFamiliasArticulosPorEmpresa(string empresa, string cc_gruart);
        List<Ttipoartic> ListarTipoArticulosPorEmpresa(string empresa);
        List<Tlistaprec> ListarPreciosArticulosPorEmpresa(string empresa);
        List<PrecioModel> ListarPreciosArticulosPorGrupoEmpresa(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks, string empresa);


    }
}
