using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class VendedorService: IVendedorService
    {
        private readonly IDbContext _dbContext;
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IDbContext dbContext, IVendedorRepository vendedorRepository)
        {
            if (vendedorRepository == null)
            {
                throw new ArgumentNullException("vendedorRepository");
            }
            _dbContext = dbContext;
            _vendedorRepository = vendedorRepository;
        }
        public IEnumerable<VendedorModel> GetAll()
        {
            var query = _vendedorRepository.GetAll().ToList();
            return query;
        }

        public VendedorModel GetByEmail(string ct_email)
        {
            var query = _vendedorRepository.GetByEmail(ct_email);
            return query;
        }
        public Dictionary<string, string> ValidarVendedorJefe(string correoUsuario)
        {
            return _vendedorRepository.ValidarVendedorJefe(correoUsuario);
        }
    }
}
