using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IBase<T> where T:class
    {
        T Add(T entity);
        void Update(T entity);
        T GetById(string id);

        IEnumerable<T> ExecSpQueryable(string spName, object[] sqlParameters);
    }
}
