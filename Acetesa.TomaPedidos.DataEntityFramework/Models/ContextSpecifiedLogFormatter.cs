using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models
{
  public class ContextSpecifiedLogFormatter : DatabaseLogFormatter
  {

      public ContextSpecifiedLogFormatter(DbContext context, Action<string> writeAction)
      : base(context, writeAction)
    {
        
    }

      public override void LogCommand<TResult>(DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext)
    {
      if (Context == null)
      {
        Write(string.Format(
          "Unknown context is executing command '{0}'{1}'",
          command.CommandText,
          Environment.NewLine));
      }
      else
      {
        Write(string.Format(
          "Context '{0}' is executing command '{1}'{2}",
          Context.GetType().Name,
          command.CommandText,
          Environment.NewLine));
      }
    }
  }
}