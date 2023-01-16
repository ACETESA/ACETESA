using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.Transversal.Extensions
{
    public static class SelectListExtension
    {
        public static IEnumerable<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable, 
            Func<T, string> text,
            Func<T, string> value, 
            string defaultOption = null)
        {
            var items =
                enumerable.Select(f => new SelectListItem {Text = text(f), Value = value(f)}).ToList();
            if (defaultOption != null)
            {
                items.Insert(0, new SelectListItem { Text = defaultOption, Value = "" });
            }
            return items;
        }

    }
}