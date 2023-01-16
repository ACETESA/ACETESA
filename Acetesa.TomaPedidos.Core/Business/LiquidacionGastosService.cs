using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Core.Business
{
    class LiquidacionGastosService : ILiquidacionGastosService
    {
        private readonly IDbContext _dbContext;
        private readonly ILiquidacionGastosRepository _liquidacionGastosRepository;

        public LiquidacionGastosService(IDbContext dbContext, ILiquidacionGastosRepository liquidacionGastosRepository)
        {
            if (liquidacionGastosRepository == null)
            {
                throw new ArgumentNullException("liquidacionGastosRepository");
            }
            _dbContext = dbContext;
            _liquidacionGastosRepository = liquidacionGastosRepository;
        }

        public List<LiquidacionGastos> getListaLiquidacionGastos(string correo)
        {
            return _liquidacionGastosRepository.getListaLiquidacionGastos(correo);
        }

        public List<DetalleLiquidacionGastos> getRecuperarDetalleLiquidacionGastos(int id)
        {
            return _liquidacionGastosRepository.getRecuperarDetalleLiquidacionGastos(id);
        }

        public List<LiquidacionGastos> getRecuperarLiquidacionGastos(int id)
        {
            return _liquidacionGastosRepository.getRecuperarLiquidacionGastos(id);
        }



    }
}
