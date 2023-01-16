using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Domain
{
    public class EstadoCuentaResumenModel
    {
        public string Ruc { get; set; }
        public string Razon_Social { get; set; }
        public string Telefonos { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public decimal? fm_deuda_dol { get; set; }
        public decimal? fm_deuda_sol { get; set; }
        public string cd_mon_cred { get; set; }
        public decimal? fm_limite_cred { get; set; }
        public decimal? fm_saldo_cred { get; set; }
        public decimal? fm_venc_dol { get; set; }
        public decimal? fm_venc_sol { get; set; }
        public decimal? fm_por_venc_dol { get; set; }
        public decimal? fm_por_venc_sol { get; set; }
        public EstadoCuentaDetalleModel EstadoCuentaDetalleModel { get; set; }
    }
}
