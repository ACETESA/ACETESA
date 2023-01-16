﻿using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IVendedorService
    {
        IEnumerable<VendedorModel> GetAll();
        VendedorModel GetByEmail(string ct_email);
        Dictionary<string, string> ValidarVendedorJefe(string correoUsuario);
    }
}
