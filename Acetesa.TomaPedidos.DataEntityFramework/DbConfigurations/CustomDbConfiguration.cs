using System.Data.Entity;
using Acetesa.TomaPedidos.DataEntityFramework.Models;

namespace Acetesa.TomaPedidos.DataEntityFramework.DbConfigurations
{
  internal class CustomDbConfiguration : DbConfiguration
  {
    public CustomDbConfiguration()
    {
      SetDatabaseLogFormatter(
        (context, writeAction) => new ContextSpecifiedLogFormatter(context, writeAction));
    }
  }
}