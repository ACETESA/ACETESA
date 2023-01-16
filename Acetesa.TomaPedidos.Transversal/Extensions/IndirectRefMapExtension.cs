
using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;

namespace Acetesa.TomaPedidos.Transversal.Extensions
{
    public static class IndirectRefMapExtension
    {
        public static string GetIndirectRef(this string directRef)
        {
            //var map = (Dictionary<string,string>)HttpContext.Current.Session["Map"];
            //return map == null ? AddDirectRef(directRef) : map[directRef];
            return AddDirectRef(directRef);
        }

        public static string GetDirectRef(this string indirectRef)
        {
            var map = HttpContext.Current.Session["Map"];
            if (map == null) throw new ApplicationException("Mapa no encontrado");
            return ((Dictionary<string, string>)map)[indirectRef];
        }

        private static string AddDirectRef(string directRef)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[32];

            rng.GetBytes(buff);

            var indirectRef = HttpServerUtility.UrlTokenEncode(buff);

            IDictionary<string, string> map = (Dictionary<string, string>)HttpContext.Current.Session["Map"];
            if (map != null)
            {
                if (!map.ContainsKey(directRef))
                {
                    map.Add(directRef, indirectRef);
                    map.Add(indirectRef, directRef);
                }
                else
                {
                    return ((Dictionary<string, string>)map)[directRef];
                }
            }
            else
            {
                map = new Dictionary<string, string> { { directRef, indirectRef }, { indirectRef, directRef } };
            }

            HttpContext.Current.Session["Map"] = map;
            return indirectRef;
        }
    }
}