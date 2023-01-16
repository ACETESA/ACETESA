using Acetesa.TomaPedidos.AdminMvc.Models;

namespace Acetesa.TomaPedidos.AdminMvc.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}
