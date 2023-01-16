using System.Data.Entity.ModelConfiguration;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping
{
    public class tipo_cambio_diarioMap : EntityTypeConfiguration<tipo_cambio_diario>
    {
        public tipo_cambio_diarioMap()
        {
            // Primary Key
            this.HasKey(t => new { t.d_f_tipo_cambio, t.n_i_valor_compra, t.n_i_val_venta });

            // Properties
            // Table & Column Mappings
            this.ToTable("tipo_cambio_diario");
            this.Property(t => t.d_f_tipo_cambio).HasColumnName("d_f_tipo_cambio");
            this.Property(t => t.n_i_valor_compra).HasColumnName("n_i_valor_compra");
            this.Property(t => t.n_i_val_venta).HasColumnName("n_i_val_venta");
            this.Property(t => t.n_i_paralelo_venta).HasColumnName("n_i_paralelo_venta");
            this.Property(t => t.n_i_paralelo_compra).HasColumnName("n_i_paralelo_compra");
        }
    }
}
