using System;
using System.Threading;

namespace Acetesa.TomaPedidos.Transversal.Extensions
{
    public static class StringExtension
    {
        public static string GetNameFileAsDateTime()
        {
            var fecha = DateTime.Now;
            return fecha.ToString("dd_MM_yy_hh_mm_ss");
        }

        public static string ConvertToTitleCase(this string source)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(source);
        }
    }
}
