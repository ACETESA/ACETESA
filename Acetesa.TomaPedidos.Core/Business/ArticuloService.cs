using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;

        public ArticuloService(IArticuloRepository articuloRepository)
        {
            _articuloRepository = articuloRepository;
        }

        public IEnumerable<ArticuloModel> GetAllCodDes()
        {
            var query = _articuloRepository.GetAllCodDes().ToList();
            return query;
        }

        public ArticuloModel GetByCodigo(string ccArtic)
        {
            var query = _articuloRepository.GetByCodigo(ccArtic);
            return query;
        }

        //jlazaro
        public IEnumerable<ArticuloModel> GetByNombreOrCodigo(string param)
        {
            //var lista = _articuloRepository
            //    .GetNombreOrCodigo(param)
            //    .ToList();
            //return lista;

            return _articuloRepository.GetArticuloNombreOrCodigo(param);
        }


        public IEnumerable<ArticuloModel> GetByNombreOrCodigoYGrupo(string grupo, string subGrupo, /*string param,*/ string cc_tienda)
        {
            return _articuloRepository.GetArticuloNombreOrCodigoYGrupo(grupo, subGrupo, /*param,*/ cc_tienda);
        }
        public List<ArticuloModel.ArticuloGS> GrupoSubgrupoSegunArtic(string idArticulo)
        {
            return _articuloRepository.GrupoSubgrupoSegunArtic(idArticulo);
        }
        public List<ArticuloModel.Stock> StockSegunArticulo(string idArticulo)
        {
            return _articuloRepository.StockSegunArticulo(idArticulo);
        }
        //public Dictionary<string, string> ValidarProductoPorStockYProduccion(string grupo, string subGrupo, string cc_artic, string cc_tienda, string cn_proforma)
        //{
        //    return _articuloRepository.ValidarProductoPorStockYProduccion(grupo, subGrupo, cc_artic, cc_tienda, cn_proforma);
        //}
        public Dictionary<string, string> ValidaStockArticulo_LCPEDIDOWEB(string cc_artic, string cc_tienda, decimal StockSolicitado, bool EsProforma, string cn_proforma, string cn_pedido)
        {
            return _articuloRepository.ValidaStockArticulo_LCPEDIDOWEB(cc_artic, cc_tienda, StockSolicitado, EsProforma,cn_proforma, cn_pedido);
        }
        public Dictionary<string, string> ValidaProductosCotizacion(string cnProforma)
        {
            return _articuloRepository.ValidaProductosCotizacion(cnProforma);
        }
        public Dictionary<string, string> ValidarStockPedido(string cnPedido, string listaArtic, string listaStockSolicitado, string ccTienda)
        {
            return _articuloRepository.ValidarStockPedido(cnPedido, listaArtic, listaStockSolicitado, ccTienda);
        }
        public List<ArticuloModel.Stock> ObtenerStockTodasTiendasPorArticulo(string IdArticulo)
        {
            return _articuloRepository.ObtenerStockTodasTiendasPorArticulo(IdArticulo);
        }
    }
}
