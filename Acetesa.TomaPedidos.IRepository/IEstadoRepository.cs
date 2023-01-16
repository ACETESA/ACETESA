using System.Linq;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IEstadoRepository : IBase<TESTADO>
    {
        IQueryable<TESTADO> GetAll();
    }
}
