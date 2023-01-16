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
    public class ClienteService : IClienteService
    {
        private readonly IDbContext _dbContext;
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IDbContext dbContext, IClienteRepository clienteRepository)
        {
            if (clienteRepository == null)
            {
                throw new ArgumentNullException("clienteRepository");
            }
            _dbContext = dbContext;
            _clienteRepository = clienteRepository;
        }

        public IEnumerable<ClienteModel> GetAll()
        {
            var query = _clienteRepository.GetAll().ToList();
            return query;
        }

        public ClienteModel GetByCodigo(string ccAnalis)
        {
            var query = _clienteRepository.GetByCodigo(ccAnalis);
            return query;
        }

        public ClienteModel GetByRazSoc(string cdRazSoc)
        {
            var query = _clienteRepository.GetByRazSoc(cdRazSoc);
            return query;
        }

        public MCLIENTE GuardarBasico(MCLIENTE entity)
        {
            var existeRuc = GetByCodigo(entity.cc_analis);
            if (existeRuc != null)
            {
                throw new ApplicationException(string.Format("Ruc: {0} existe.", entity.cc_analis.Trim()));
            }
            var existeRazonSocial = GetByRazSoc(entity.cd_razsoc);
            if (existeRazonSocial != null)
            {
                throw new ApplicationException(string.Format("Razón Social: {0} existe.", entity.cd_razsoc.Trim()));
            }
            var query = _clienteRepository.Add(entity);
            _dbContext.Commit();
            return query;
        }

        public ClienteModel GetEmailByCodigo(int tipoMail, string ccAnalis, string cn_contacto, string cn_suc)
        {
            var query = _clienteRepository.GetEmailByCodigo(tipoMail, ccAnalis, cn_contacto, cn_suc);
            return query;
        }

        //public void UpdateEmailByCodigo(string ccAnalis, string email)
        //{
        //    _clienteRepository.UpdateEmailByCodigo(ccAnalis,email);
        //    _dbContext.Commit();
        //}

        public void ActualizarMailContacto(int tipoMail, string id, string emailPara)
        {
            _clienteRepository.ActualizarMailContacto(tipoMail, id, emailPara);
        }

        public IEnumerable<ClienteModel> GetByRazonSocialOrRuc(string param)
        {
            var lista = _clienteRepository.GetByRazSocOrRuc(param)
                .ToList();
            return lista;
        }
        public IEnumerable<ContactoListadoModel> GetContactoEntregaDirectaByccAnalis(string ccAnalis)
        {
            var result = _clienteRepository.GetContactoEntregaDirectaByccAnalis(ccAnalis).Select(x => new ContactoListadoModel
            {
                idContacto = (x.idContacto == null) ? " " : x.idContacto,
                nombreContacto = x.nombreContacto
            })
                .ToList();
            return result;
        }
        public List<ClienteModel.VendedorCliente> VendedorAsignadoPorCliente(string ClienteID)
        {
            return _clienteRepository.VendedorAsignadoPorCliente(ClienteID);
        }
        public List<ClienteModel> ClientesActivos()
        {
            return _clienteRepository.ClientesActivos();
        }
        public Dictionary<int, string> NuevoCliente(MCLIENTE cliente, string emailUsuario)
        {
            return _clienteRepository.NuevoCliente(cliente, emailUsuario);
        }
        public List<TSECTOR> ListarSector()
        {
            return _clienteRepository.ListarSector();
        }
        public List<UBIGEO> ListarDepartamento()
        {
            return _clienteRepository.ListarDepartamento();
        }
        public List<UBIGEO> ListarProvincia(string cc_dpto)
        {
            return _clienteRepository.ListarProvincia(cc_dpto);
        }
        public List<UBIGEO> ListarDistrito(string cc_dpto, string cc_prov)
        {
            return _clienteRepository.ListarDistrito(cc_dpto, cc_prov);
        }
        public Dictionary<string, string> ValidarRelacionVendedorCliente(string cc_analis, string emailUsuario)
        {
            return _clienteRepository.ValidarRelacionVendedorCliente(cc_analis, emailUsuario);
        }
        public Dictionary<string, string> ValidarExistenciaClientePorRUC(string cc_analis)
        {
            return _clienteRepository.ValidarExistenciaClientePorRUC(cc_analis);
        }
        public List<CarteraCliente> ClientesAsignadosLibres(string correoVendedor)
        {
            return _clienteRepository.ClientesAsignadosLibres(correoVendedor);
        }

        public List<CarteraCliente> CarteraClientesAsignados(string correoVendedor, string departamentoId, string provinciaId, string distritoId)
        {
            return _clienteRepository.CarteraClientesAsignados(correoVendedor,departamentoId,provinciaId,distritoId);
        }

        public Dictionary<string, string> ActualizarAsignacionClienteVendedor(string rucCliente, string correoVendedor, bool asignar)
        {
            return _clienteRepository.ActualizarAsignacionClienteVendedor(rucCliente, correoVendedor, asignar);
        }

        public string ValidarClienteEnZonaLiberada(string ruc)
        {
            return _clienteRepository.ValidarClienteEnZonaLiberada(ruc);
        }

    }
}
