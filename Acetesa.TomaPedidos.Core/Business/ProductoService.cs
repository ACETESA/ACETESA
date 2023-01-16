using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            if (productoRepository == null)
            {
                throw new ArgumentNullException("productoRepository");
            }
            _productoRepository = productoRepository;
        }
        public IEnumerable<Tlistaprec> GetListaPreciosSp()
        {
            var result = _productoRepository.GetListaPreciosSp().ToList();
            return result;
        }
        public IEnumerable<Tgruartec> GetFamiliasSp()
        {
            var result = _productoRepository.GetFamiliasSp().ToList();
            return result;
        }

        public IEnumerable<SubFamiliaModel> GetSubFamiliasByCodFamiliaSp(string codFamilia)
        {
            if (string.IsNullOrEmpty(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            var result = _productoRepository.GetSubFamiliasByCodFamiliaSp(codFamilia).ToList();
            return result;
        }

        public IEnumerable<Ttipoartic> GetTipoProductoSp()
        {
            var result = _productoRepository.GetTiposSp().ToList();
            return result;
        }

        //jlazaro
        public IEnumerable<StockModel> GetStockProductosSp(DateTime fechaDia, string codFamilia, string codSubFamilia, string tipo)
        {
            if (string.IsNullOrEmpty(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }
            //if (string.IsNullOrEmpty(tipo))
            //{
            //    throw new ArgumentNullException("tipo");
            //}
            var result = _productoRepository.GetStockSp(fechaDia, codFamilia, codSubFamilia,
                tipo);
            return result;
        }

        //jlazaro
        public Dictionary<string, object> GetStockProductosStoreProcesure(DateTime fechaDia, string codFamilia, string codSubFamilia, string tipo)
        {
            if (string.IsNullOrEmpty(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }
            var result = _productoRepository.GetStockStoreProcedure(fechaDia, codFamilia, codSubFamilia, tipo);
            return result;
        }

        public IEnumerable<PrecioModel> GetPreciosProductosSp(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks)
        {
            if (string.IsNullOrEmpty(codListaPrecio))
            {
                throw new ArgumentNullException("codListaPrecio");
            }
            if (string.IsNullOrEmpty(codFamilia))
            {
                //throw new ArgumentNullException("codFamilia");
                codFamilia = "%";
                codSubFamilia = null;

            }
            if (string.IsNullOrEmpty(codSubFamilia))
            {
                codSubFamilia = "%";
            }
            var result = _productoRepository
                .GetPreciosSp(codListaPrecio, codFamilia, codSubFamilia, Stocks);
                //.OrderBy(o=>o.cc_artic);
            return result;
        }

        //jlazaro
        public List<object> GetEstadisticaVentas(DateTime sFechaInicio, DateTime sFechaFinal, string sVendedor = "", string sEmpresa = "", string sTienda ="")
        {
            var result = _productoRepository.GetEstadisticaVentas(sFechaInicio, sFechaFinal, sVendedor, sEmpresa, sTienda);
            return result;
        }

        //jpastor
        public IEnumerable<StockPorGrupoModel> GetStockPorGrupoProductoSp(string sEmpresa, string sTienda, string codFamilia,string codSubGrupo, string tipo, DateTime fecha)
        {
            if (string.IsNullOrEmpty(codFamilia))
            {
                codFamilia = "%";
            }
            if (string.IsNullOrEmpty(codSubGrupo) | codSubGrupo == "Ninguno")
            {
                codSubGrupo = "%";
            }
            var result = _productoRepository.GetStockPorGrupoSp(sEmpresa, sTienda, codFamilia, codSubGrupo, tipo, fecha);
            return result;
        }
        public List<Tgruartec> ListarFamiliasArticulosPorEmpresa(string empresa)
        {
            return _productoRepository.ListarFamiliasArticulosPorEmpresa(empresa);
        }

        public List<SubFamiliaModel> ListarSubFamiliasArticulosPorEmpresa(string empresa, string cc_gruart)
        {
            return _productoRepository.ListarSubFamiliasArticulosPorEmpresa(empresa, cc_gruart);
        }

        public List<Ttipoartic> ListarTipoArticulosPorEmpresa(string empresa)
        {
            return _productoRepository.ListarTipoArticulosPorEmpresa(empresa);
        }
        public List<Tlistaprec> ListarPreciosArticulosPorEmpresa(string empresa)
        {
            return _productoRepository.ListarPreciosArticulosPorEmpresa(empresa);
        }

        public List<PrecioModel> ListarPreciosArticulosPorGrupoEmpresa(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks, string empresa)
        {
            if (string.IsNullOrEmpty(codListaPrecio))
            {
                throw new ArgumentNullException("codListaPrecio");
            }
            if (string.IsNullOrEmpty(codFamilia) || codFamilia == "Ninguno")
            {
                //throw new ArgumentNullException("codFamilia");
                codFamilia = "%";
                codSubFamilia = null;

            }
            if (string.IsNullOrEmpty(codSubFamilia) || codSubFamilia == "Ninguno")
            {
                codSubFamilia = "%";
            }

            return _productoRepository.ListarPreciosArticulosPorGrupoEmpresa(codListaPrecio,codFamilia,codSubFamilia,Stocks,empresa);
        }


    }
}
