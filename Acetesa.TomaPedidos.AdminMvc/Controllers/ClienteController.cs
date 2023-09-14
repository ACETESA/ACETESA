using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.Core.Business;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class ClienteController : BaseController
    {
        public IClienteService ClienteService { get; set; }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listado()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Nuevo()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo(string model)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult VendedorAsignadoPorCliente() {
            return View();
        }

        public ActionResult ListaVendedorCliente(string ClienteID)
        {
            List<ClienteModel.VendedorCliente> lista = new List<ClienteModel.VendedorCliente>();
            lista = ClienteService.VendedorAsignadoPorCliente(ClienteID);
            return JsonSuccess(lista);
        }

        public ActionResult ClientesActivos()
        {
            List<ClienteModel> lista = new List<ClienteModel>();
            lista = ClienteService.ClientesActivos();
            return JsonSuccess(lista);
        }

        public ActionResult ValidarExistenciaClientePorRUC(string cc_analis)
        {
            var diccionario = ClienteService.ValidarExistenciaClientePorRUC(cc_analis);
            return JsonSuccess(diccionario);
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CarteraCliente()
        {
            //var listaClientes = ClienteService.ClientesAsignadosLibres(User.Identity.Name);
            return View();
        }

        public ActionResult ClientesAsignadosLibres(string EsAsignado)
        {
            var listaClientes = ClienteService.ClientesAsignadosLibres(User.Identity.Name, EsAsignado);

            //var listaFiltrada = listaClientes;

            //if (rucCliente == "")
            //{
            //    listaFiltrada = listaClientes;
            //}
            //else
            //{
            //    listaFiltrada = listaClientes
            //        .Where(m => m.Ruc == rucCliente)
            //        .ToList();
            //}

            return JsonSuccess(listaClientes);
        }

        public ActionResult ClientesAsignadosLibresSelect()
        {
            var listaClientes = ClienteService.ClientesAsignadosLibres(User.Identity.Name,"%");

            var selectClientes = (from model in listaClientes
                                 select new { model.Ruc, model.RazonSocial}).ToList();

            return JsonSuccess(selectClientes);
        }

        public ActionResult SelectClientesSegunCarteraVendedor()
        {
            var listaClientes = ClienteService.SelectClientesSegunCarteraVendedor(User.Identity.Name);

            var selectClientes = (from model in listaClientes
                                  select new { model.cc_analis, model.cd_razsoc}).ToList();

            return JsonSuccess(selectClientes);
        }

        public ActionResult SelectClientesSegunCarteraVendedorYLibres()
        {
            var listaClientes = ClienteService.SelectClientesSegunCarteraVendedorYLibres(User.Identity.Name);

            var selectClientes = (from model in listaClientes
                                  select new { model.cc_analis, model.cd_razsoc }).ToList();

            return JsonSuccess(selectClientes);
        }

        public ActionResult ClientesAsignadosSelect(string departamentoId, string provinciaId, string distritoId)
        {
            var listaClientes = ClienteService.CarteraClientesAsignados(User.Identity.Name, departamentoId, provinciaId, distritoId);

            var selectClientes = (from model in listaClientes
                                  select new { model.Ruc, model.RazonSocial }).ToList();

            return JsonSuccess(selectClientes);
        }

        public ActionResult ActualizarAsignacionClienteVendedor(string rucCliente, bool asignar)
        {
          var diccionario = ClienteService.ActualizarAsignacionClienteVendedor(rucCliente, User.Identity.Name, asignar);

            return JsonSuccess(diccionario);
        }

        public ActionResult SelectTipoDocumentoIdentidad()
        {
            var listaTipoDocumentos = ClienteService.SelectTipoDocumentoIdentidad();

            return JsonSuccess(listaTipoDocumentos);
        }


        public ActionResult SelectProcedencias()
        {
            List<Dictionary<string, string>> Procedencias = new List<Dictionary<string, string>>();
            Procedencias.Add(new Dictionary<string, string> { { "idProcedencia", "N" }, { "procedencia", "Nacional" } });
            Procedencias.Add(new Dictionary<string, string> { { "idProcedencia", "R" }, { "procedencia", "Regional" } });
            Procedencias.Add(new Dictionary<string, string> { { "idProcedencia", "E" }, { "procedencia", "Extranjera" } });

            return JsonSuccess(Procedencias);
        }

        public ActionResult SelectDistrito(string departamentoID, string provinciaID)
        {
            var listaDistrito = ClienteService.ListarDistrito(departamentoID,provinciaID);

            return JsonSuccess(listaDistrito);
        }

        public ActionResult SelectProvincia(string departamentoID)
        {
            var listaProvincia = ClienteService.ListarProvincia(departamentoID);

            return JsonSuccess(listaProvincia);
        }

        public ActionResult SelectDepartamento()
        {
            var listaDepartamento = ClienteService.ListarDepartamento();

            return JsonSuccess(listaDepartamento);
        }

        public ActionResult SelectPais()
        {
            var listaPaises = ClienteService.ListarPaises();

            return JsonSuccess(listaPaises);
        }

        public ActionResult SelectZonas(string cc_distrito, string cc_dpto, string cc_prov)
        {
            var listaZonas = ClienteService.ListarZonas(cc_distrito, cc_dpto, cc_prov);

            return JsonSuccess(listaZonas);
        }

        public ActionResult SelectSector()
        {
            var listaSector = ClienteService.ListarSector();

            return JsonSuccess(listaSector);
        }
        public ActionResult SelectCategorias()
        {
            var listaCategoria = ClienteService.ListarCategorias();
            return JsonSuccess(listaCategoria);
        }

        public ActionResult SelectEstadosCliente()
        {
            var listaEstado = ClienteService.ListarEstadosCliente();
            return JsonSuccess(listaEstado);
        }

        public ActionResult SelectMonedaFacturacion()
        {
            List<Dictionary<string, string>> MonedaFacturacion = new List<Dictionary<string, string>>();
            MonedaFacturacion.Add(new Dictionary<string, string> { { "cb_monfac", "N" }, { "cd_monfac", "Nacional" } });
            MonedaFacturacion.Add(new Dictionary<string, string> { { "cb_monfac", "E" }, { "cd_monfac", "Extranjera" } });
            MonedaFacturacion.Add(new Dictionary<string, string> { { "cb_monfac", "A" }, { "cd_monfac", "Ambas" } });

            return JsonSuccess(MonedaFacturacion);
        }

        public ActionResult RegistrarCliente(MCLIENTE cliente)
        {
            var listaEstado = ClienteService.RegistrarCliente(cliente);
            return JsonSuccess(listaEstado);
        }


    }
}