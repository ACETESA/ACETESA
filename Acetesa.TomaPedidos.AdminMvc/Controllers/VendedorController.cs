using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.Business;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Transversal;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class VendedorController : BaseController
    {
        public IVendedorService VendedorService { get; set; }

        // GET: Vendedor
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActualizarDatos()
        {
            var saved = (bool?)TempData["Guardado"];
            if (saved.HasValue && saved.Value)
            {
                TempData["Success"] = "Datos del correo actualizado.";
            }

            var credentials = VendedorService.ObtenerCredencialesCorreoVendedor(User.Identity.Name);

            string decryptedPassword = "";

            if (credentials.clave.Length > 0 & credentials.llave.Length > 0 & credentials.correo.Length > 0)
            {
                decryptedPassword = AesOperation.DecryptString(credentials.llave, credentials.clave);
            }

            VendedorViewModel model = new VendedorViewModel();
            model.correoElectronico = new VendedorViewModel.CorreoElectronico {
                usuario = credentials.correo,
                clave = decryptedPassword
            };

            return View(model);
        }

        // GET: Vendedor
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarDatos(VendedorViewModel model)
        {
            var key = AesOperation.RandomString(32);
            var encryptedPassword = AesOperation.EncryptString(key, model.correoElectronico.clave);

            var resultados = VendedorService.RegistrarCredencialesCorreoVendedor(model.correoElectronico.usuario, encryptedPassword, key);

            string  mensajeID = resultados["mensajeID"];
            string  mensaje = resultados["mensaje"];
            if (mensajeID == "0")
            {
                ModelState.AddModelError("", mensaje);
                return View();
            }
            else
            {
                TempData["Guardado"] = true;

                VendedorViewModel modelEmpty = new VendedorViewModel();
                return RedirectToAction("ActualizarDatos");
            }
        }
    }
}