using System.Linq;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IMonedaRepository : IBase<MonedaModel>
    {
        IQueryable<MonedaModel> GetAll();
        MonedaModel GetCdMonedaByCcMoneda(string ccMoneda);
    }
}
