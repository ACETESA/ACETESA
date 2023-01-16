using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Transversal
{
    public interface IDbContext
    {

        IQueryable<T> Query<T>() where T : class;
        IQueryable<T> QueryNoTracking<T>() where T : class;
        T FindById<T>(string id) where T : class;
        T Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Commit();
        Task<int> CommitAsync();
        IEnumerable<T> GetExecSpEnumerable<T>(string spName, object[] parameters = null) where T : class, new();
    }
}
