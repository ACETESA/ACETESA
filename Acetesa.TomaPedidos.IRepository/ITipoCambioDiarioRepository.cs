using System;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface ITipoCambioDiarioRepository : IBase<tipo_cambio_diario>
    {
        tipo_cambio_diario GetByFechaTipoCambio(DateTime fechaTipoCambio);
    }
}
