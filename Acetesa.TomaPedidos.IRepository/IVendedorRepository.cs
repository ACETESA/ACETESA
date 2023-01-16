using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IVendedorRepository
    {
        IEnumerable<VendedorModel> GetAll();
        VendedorModel GetByEmail(string ct_email);
        Dictionary<string, string> ValidarVendedorJefe(string correoUsuario);
    }
}
