namespace Acetesa.TomaPedidos.Entity
{
    public partial class LDPEDIDO_WEB
    {
        public string cn_pedido { get; set; }
        public string cn_item { get; set; }
        public string cc_artic { get; set; }
        public decimal fq_cantidad { get; set; }
        public decimal fq_peso { get; set; }
        public decimal? fq_stock { get; set; }
        public string cc_lista { get; set; }
        public decimal fm_precio { get; set; }
        public decimal fm_precio2 { get; set; }
        public decimal fm_precio_fin { get; set; }
        public decimal fm_total { get; set; }
        public virtual LCPEDIDO_WEB LCPEDIDO_WEB { get; set; }
        public virtual MARTICUL MARTICUL { get; set; }
        public virtual TLISTAPRECIO TLISTAPRECIO { get; set; }
    }
}
