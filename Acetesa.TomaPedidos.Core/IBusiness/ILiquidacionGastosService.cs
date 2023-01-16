using Acetesa.TomaPedidos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ILiquidacionGastosService
    {
        List<LiquidacionGastos> getListaLiquidacionGastos(string correo);

        List<DetalleLiquidacionGastos> getRecuperarDetalleLiquidacionGastos(int id);
        List<LiquidacionGastos> getRecuperarLiquidacionGastos(int id);


    }
}
