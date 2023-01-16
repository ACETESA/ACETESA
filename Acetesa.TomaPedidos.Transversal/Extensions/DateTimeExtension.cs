using System;
using System.Globalization;

namespace Acetesa.TomaPedidos.Transversal.Extensions
{
    public static class DateTimeExtension
    {
        public static bool IsDateTime(this string value)
        {
            DateTime fecha;
            return DateTime.TryParse(value, new CultureInfo("es-PE"), DateTimeStyles.AssumeLocal, out fecha);
        }

        public static DateTime ConvertDateTime(this string value)
        {
            DateTime fecha;
            DateTime.TryParse(value, new CultureInfo("es-PE"), DateTimeStyles.AssumeLocal, out fecha);
            return fecha;
        }

        public static TimeSpan ConvertTimeSpan(this string value)
        {
            var sTime = value.Replace(".", "");
            var tHora = DateTime.ParseExact(sTime, "hh:mm:ss", CultureInfo.InvariantCulture).TimeOfDay;
            return tHora;
        }

        public static TimeSpan ConvertTimeSpanMeridional(this string value)
        {
            var sTime = value.Replace(".", "");
            var tHora = DateTime.ParseExact(sTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            return tHora;
        }
        public static DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
        public static int GetAge(DateTime birthday)
        {
            var now = DateTime.Today;
            var age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;
            return age;
        }

        public static string GetNoCache()
        {
            var milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds.ToString(CultureInfo.InvariantCulture);
        }

        public static string FormatearFechaInicio(DateTime fecha)
        {
            return string.Format("{0}-{1}-{2} 00:00:00", fecha.Year, fecha.Month, fecha.Day);
        }

        public static string FormatearFechaFinal(DateTime fecha)
        {
            return string.Format("{0}-{1}-{2} 23:59:59", fecha.Year, fecha.Month, fecha.Day);
        }
    }
}
