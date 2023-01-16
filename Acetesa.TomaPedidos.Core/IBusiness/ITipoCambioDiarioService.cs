using System;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ITipoCambioDiarioService
    {
        double GetByFechaTipoCambio(DateTime fechaTipoCambio);
    }
}
