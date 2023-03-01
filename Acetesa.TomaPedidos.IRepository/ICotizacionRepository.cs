using System;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface ICotizacionRepository : IBase<LCPROF_WEB>
    {
        IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string estado);
        IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string vendedor, string estado);
        LCPROF_WEB GetLastCotizacion();
        void Delete(LCPROF_WEB entity);
        void GuardarAdicional(LCPROF_WEB entityMaster, string email, string Tienda, int IncluyeIGV, string cn_suc, string cn_contacto, bool imprimirPrecioTN, string observacion, int zonaLiberada);
        LCPROFADICIONAL_WEB GetAdicionalById(string cnCotizacion);
        string AnularRestablecerProforma(string cn_proforma);
        List<Entity.Consultas.DetalleCotizacion> DatosDetalleProformaParaPedido(string cn_proforma);
        List<CotizacionMotivoRechazo> ListaMotivosRechazoCotizacion();
        Dictionary<string, string> RegistrarRechazoCotizacion(string cn_proforma, int idMotivo, string mensajeRechazo);
        Dictionary<string, string> RegistrarCierreCotizacionParcial(string cn_proforma, int idMotivo, string mensajeRechazo);
        List<Tuple<string, string>> ValidarTransformacionCotizacionAPedido(string CotizacionID);

    }
}