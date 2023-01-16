﻿using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IMonedaService
    {
        IEnumerable<MonedaModel> GetAll();
        MonedaModel GetCdMonedaByCcMoneda(string ccMoneda);
    }
}
