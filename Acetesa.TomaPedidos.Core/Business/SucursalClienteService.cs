using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class SucursalClienteService : ISucursalClienteService
    {
        private readonly ISucursalClienteRepository _sucursalClienteRepository;

        public SucursalClienteService(
            ISucursalClienteRepository sucursalClienteRepository)
        {
            _sucursalClienteRepository = sucursalClienteRepository;
        }

        public IEnumerable<SucursalClienteModel> GetByCcAnalis(string ccAnalis)
        {
            var query = _sucursalClienteRepository
                .GetByCcAnalis(ccAnalis)
                .Select(x => new SucursalClienteModel
                {
                    cc_analis = ccAnalis,
                    cd_direc = (x.cd_direc == null) ? " ": x.cd_direc,
                    cn_suc = x.cn_suc
                })
                .ToList();
            return query;
        }

        public IEnumerable<SucursalClienteModel> GetByCcAnalisCnSuc(string ccAnalis, string cnSuc)
        {
            var query = _sucursalClienteRepository
                .GetByCcAnalisCnSuc(ccAnalis,cnSuc)
                .Select(x => new SucursalClienteModel
                    {
                        cc_analis = x.cc_analis,
                        cd_direc = x.cd_direc,
                        cn_suc = x.cn_suc
                    })
                .ToList();
            return query;
        }

        public IEnumerable<SucursalClienteModel> GetLugarEntregaByCcAnalis(string ccAnalis)
        {
            var result = _sucursalClienteRepository.GetLugarEntregaByCcAnalis(ccAnalis).Select(x => new SucursalClienteModel
                    {
                        cd_direc = (x.cd_direc == null) ? " " : x.cd_direc,
                cn_suc = x.cn_lug
                    })
                .ToList();
            return result;
        }

        public IEnumerable<SucursalClienteModel> GetTransporteByCcAnalis(string ccAnalis)
        {
            var result = _sucursalClienteRepository.GetTransporteByCcAnalis(ccAnalis).Select(x => new SucursalClienteModel
            {
                cd_direc = x.cd_transp,
                cn_suc = x.cc_transp
            })
                .ToList();
            return result;
        }
    }
}
