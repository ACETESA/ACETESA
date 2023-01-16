using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.DataEntityFramework
{
    public class DbContextAdapter : IDbContext, IDisposable
    {
        private DbContext _dbContext;


        public DbContextAdapter(DbContext context)
        {
            _dbContext = context;
            
        }

        public DbContext Context { get { return _dbContext; } }

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> QueryNoTracking<T>() where T : class
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public T FindById<T>(string id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Add<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                _dbContext.Set<T>().Add(entity);
            }
            return entity;
        }

        public void Update<T>(T entity) where T : class
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(entity);

            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Commit()
        {
            
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entidad de Tipo \"{0}\" en estado \"{1}\" tiene los siguientes errores de validación:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    var errores = eve.ValidationErrors;
                    foreach (var item in errores)
                    {
                        sb.AppendLine(string.Format("- Propiedad: \"{0}\", Error: \"{1}\"", item.PropertyName, item.ErrorMessage));
                    }
                }
                throw;
            }
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetExecSpEnumerable<T>(string spName, object[] parameters = null) where T : class, new()

        {
            DbRawSqlQuery<T> result;
            if (parameters != null && parameters.Any())
            {
                var sb = new StringBuilder();
                sb.Append("EXEC ");
                sb.Append(spName + " ");
                foreach (var parameter in parameters)
                {
                    sb.Append(parameter + ",");
                }
                var commandString = sb.ToString();
                var lastIndex = commandString.LastIndexOf(",", StringComparison.Ordinal);
                if (lastIndex > -1)
                {
                    commandString = commandString.Substring(0, lastIndex);
                }
                result = _dbContext.Database
                   .SqlQuery<T>(commandString, parameters);
            }
            else
            {
                result = _dbContext.Database
                    .SqlQuery<T>(spName);
            }
            return result.ToList();

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
            _disposed = true;
        }

        ~DbContextAdapter()
        {
            Dispose(true);
        }
    }
}
