using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class TiendaService: ITiendaService
    {
        private readonly ITiendaRepository _tiendaRepository;

        public TiendaService(
            ITiendaRepository tiendaRepository)
        {
            _tiendaRepository = tiendaRepository;
        }


        //public TTIENDA GetTiendaSegunVendedor(string ct_email)
        //{
        //    if (string.IsNullOrEmpty(ct_email) || string.IsNullOrWhiteSpace(ct_email))
        //    {
        //        throw new ArgumentNullException("ct_email");
        //    }
        //    var result = _tiendaRepository.GetTiendaSegunVendedor(ct_email);
        //    return result;
        //}
    }
}
