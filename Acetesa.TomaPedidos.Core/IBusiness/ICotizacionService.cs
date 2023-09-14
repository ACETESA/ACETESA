using System;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ICotizacionService
    {
        IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string estado);
        IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string vendedor, string estado);
        LCPROF_WEB GetById(string cnProforma);
        LCPROF_WEB GetLastCnProforma();
        void Guardar(LCPROF_WEB entity, int igv_bo, string empresa, int zonaLiberada);
        void GuardarAdicional(LCPROF_WEB entity, string email, string Tienda, int IncluyeIGV, string cn_suc, string cn_contacto, bool imprimirPrecioTN, string observacion, int zonaLiberada);
        IEnumerable<LDPROF_WEB> CotizacionDetalleServices { get; set; }
        void UpdateEstado(string cnProforma, string estado);
        LCPROFADICIONAL_WEB GetAdicionalById(string cnCotizacion);
        string AnularRestablecerProforma(string cn_proforma);
        List<Entity.Consultas.DetalleCotizacion> DatosDetalleProformaParaPedido(string cn_proforma);
        List<CotizacionMotivoRechazo> ListaMotivosRechazoCotizacion();
        Dictionary<string, string> RegistrarRechazoCotizacion(string cn_proforma, int idMotivo, string mensajeRechazo);
        Dictionary<string, string> RegistrarCierreCotizacionParcial(string cn_proforma, int idMotivo, string mensajeRechazo);
        List<Tuple<string, string>> ValidarTransformacionCotizacionAPedido(string CotizacionID);
        LCPROF_WEB RecuperarDatosProformaByID(string ProformaID);
        void EliminarProformaByID(string ProformaID);
        void GuardarCabeceraProforma(LCPROF_WEB Proforma);
        void GuardarDetalleProforma(LDPROF_WEB DetalleProforma);

        void RegistrarDocumentoProforma(string ProformaID, byte[] Documento);

    }
}
