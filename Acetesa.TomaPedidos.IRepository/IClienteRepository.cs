using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IClienteRepository : IBase<MCLIENTE>
    {
        IEnumerable<ClienteModel> GetAll();
        ClienteModel GetByCodigo(string ccAnalis);
        ClienteModel GetByRazSoc(string cdRazSoc);
        ClienteModel GetEmailByCodigo(int tipoMail, string ccAnalis, string cn_contacto, string cn_suc);
        //void UpdateEmailByCodigo(string ccAnalis, string email);
        IQueryable<ClienteModel> GetByRazSocOrRuc(string param);
        IEnumerable<ContactoListadoModel> GetContactoEntregaDirectaByccAnalis(string ccAnalis);
        List<ClienteModel.VendedorCliente> VendedorAsignadoPorCliente(string ClienteID);
        List<ClienteModel> ClientesActivos();
        Dictionary<int, string> NuevoCliente(MCLIENTE cliente, string emailUsuario);
        List<TSECTOR> ListarSector();
        List<UBIGEO> ListarDepartamento();
        List<UBIGEO> ListarProvincia(string cc_dpto);
        List<UBIGEO> ListarDistrito(string cc_dpto, string cc_prov);
        Dictionary<string, string> ValidarRelacionVendedorCliente(string cc_analis, string emailUsuario);
        Dictionary<string, string> ValidarExistenciaClientePorRUC(string cc_analis);
        void ActualizarMailContacto(int tipoMail, string id, string emailPara);
        List<CarteraCliente> ClientesAsignadosLibres(string correoVendedor);
        List<CarteraCliente> CarteraClientesAsignados(string correoVendedor, string departamentoId, string provinciaId, string distritoId);

        Dictionary<string, string> ActualizarAsignacionClienteVendedor(string rucCliente, string correoVendedor, bool asignar);
        string ValidarClienteEnZonaLiberada(string ruc);


    }
}
