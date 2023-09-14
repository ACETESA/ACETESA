using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IArticuloRepository : IBase<ArticuloModel>
    {
        IEnumerable<ArticuloModel> GetAllCodDes();
        ArticuloModel GetByCodigo(string ccArtic);
        IQueryable<ArticuloModel> GetNombreOrCodigo(string param);
        //jlazaro
        IEnumerable<ArticuloModel> GetArticuloNombreOrCodigo(string param);
        IEnumerable<ArticuloModel> GetArticuloNombreOrCodigoYGrupo(string grupo, string subGrupo/*, string param*/, string cc_tienda);
        List<ArticuloModel.ArticuloGS> GrupoSubgrupoSegunArtic(string idArticulo);
        List<ArticuloModel.Stock> StockSegunArticulo(string idArticulo);
        //Dictionary<string, string> ValidarProductoPorStockYProduccion(string grupo, string subGrupo, string cc_artic, string cc_tienda, string cn_proforma);
        Dictionary<string, string> ValidaStockArticulo_LCPEDIDOWEB(string cc_artic, string cc_tienda, decimal StockSolicitado, bool EsProforma, string cn_proforma, string cn_pedido);
        Dictionary<string, string> ValidaProductosCotizacion(string cnProforma);
        Dictionary<string, string> ValidarStockPedido(string cnPedido, string listaArtic, string listaStockSolicitado, string ccTienda);
        List<ArticuloModel.Stock> ObtenerStockTodasTiendasPorArticulo(string IdArticulo);

    }
}
