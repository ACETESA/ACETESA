using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoRepository _estadoRepository;

        public EstadoService(IEstadoRepository estadoRepository)
        {
            if (estadoRepository == null)
            {
                throw new ArgumentNullException("estadoRepository");
            }
            _estadoRepository = estadoRepository;
        }

        public IEnumerable<EstadoModel> GetAll()
        {
            var query = _estadoRepository.GetAll().Select(x => new EstadoModel
            {
                cc_estado = x.cc_estado,
                cd_estado = x.cd_estado
            }).ToList();
            return query;
        }
    }
}
