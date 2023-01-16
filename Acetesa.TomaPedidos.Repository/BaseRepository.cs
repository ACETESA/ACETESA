using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class BaseRepository<T> : IBase<T> where T : class,  new()
    {
        private readonly IDbContext _dbContext;
        private readonly IQueryable<T> _query;

        protected BaseRepository(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            _dbContext = dbContext;
            _query = _dbContext.Query<T>();
        }

        public IDbContext DbContext { get { return _dbContext; } }

        protected IQueryable<T> Query
        {
            get { return _query; }
        }

        public T Add(T entity)
        {
            _dbContext.Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public T GetById(string id)
        {
            return _dbContext.FindById<T>(id);
        }

        public IEnumerable<T> ExecSpQueryable(string spName, object[] sqlParameters)
        {
            return _dbContext.GetExecSpEnumerable<T>(spName, sqlParameters);
        }

    }
}
