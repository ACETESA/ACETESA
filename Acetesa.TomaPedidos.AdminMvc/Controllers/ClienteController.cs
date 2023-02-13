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

        public ActionResult ClientesAsignadosLibres(string rucCliente)
        {
            var listaClientes = ClienteService.ClientesAsignadosLibres(User.Identity.Name);

            var listaFiltrada = listaClientes;

            if (rucCliente == "")
            {
                listaFiltrada = listaClientes;
            }
            else
            {
                listaFiltrada = listaClientes
                    .Where(m => m.Ruc == rucCliente)
                    .ToList();
            }

            return JsonSuccess(listaFiltrada);
        }

        public ActionResult ClientesAsignadosLibresSelect()
        {
            var listaClientes = ClienteService.ClientesAsignadosLibres(User.Identity.Name);

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

    }
}