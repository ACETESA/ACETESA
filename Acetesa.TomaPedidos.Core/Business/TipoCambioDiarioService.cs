using System;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class TipoCambioDiarioService : ITipoCambioDiarioService
    {
        private readonly ITipoCambioDiarioRepository _tipoCambioDiarioRepository;

        public TipoCambioDiarioService(ITipoCambioDiarioRepository tipoCambioDiarioRepository)
        {
            _tipoCambioDiarioRepository = tipoCambioDiarioRepository;
        }

        public double GetByFechaTipoCambio(DateTime fechaTipoCambio)
        {
            var query = _tipoCambioDiarioRepository.GetByFechaTipoCambio(fechaTipoCambio);
            if (query == null) return 0.00;
            var precioVenta = query.n_i_val_venta;
            return precioVenta ?? 0.00;
        }
    }
}
