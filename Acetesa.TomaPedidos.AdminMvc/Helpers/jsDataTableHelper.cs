using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Resources;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Helpers
{
    public class jsDataTableHelper
    {
        public class ValorCeldaImagen : ValorCelda
        {
            public ValorCeldaImagen(string sSrc, bool bSrcEsAbsoluto, string sAlt = null, string sTitle = null, string sClase = null)
                : base("img", sClase)
            {
                this["src"] = bSrcEsAbsoluto ? sSrc : VirtualPathUtility.ToAbsolute("~/" + sSrc);
                this["alt"] = sAlt;
                this["title"] = sTitle;
            }
        }

        public class ValorCeldaLink : ValorCelda
        {
            public ValorCeldaLink(string sTexto, string sClase = null)
                : base("a", sClase)
            {
                this["href"] = "#";
                this["innerText"] = sTexto;
            }
        }

        public class ValorCeldaRadioButton : ValorCeldaInput
        {
            public ValorCeldaRadioButton(string sName, string sValor, bool bChequeado = false, string sClase = null)
                : base("radio", sValor, sName, sClase)
            {
                if (bChequeado)
                    this["checked"] = "";
            }
        }

        public class ValorCeldaCheckBox : ValorCeldaInput
        {
            public ValorCeldaCheckBox(string sName, string sValor, bool bChequeado = false, string sClase = null)
                : base("checkbox", sValor, sName, sClase)
            {
                if (bChequeado)
                    this["checked"] = "";
            }
        }

        public class ValorCeldaBoton : ValorCeldaInput
        {
            public ValorCeldaBoton(string sName, string sValor, string sClase = null)
                : base("button", sValor, sName, sClase)
            {

            }
        }

        public class ValorCeldaSpan : ValorCelda
        {
            public ValorCeldaSpan(string sClase, string tooltip = null, string sTexto = "")
                : base("span", sClase)
            {
                this["title"] = tooltip;
                this["innerText"] = sTexto;
            }
        }

        public abstract class ValorCeldaInput : ValorCelda
        {
            public ValorCeldaInput(string sType, string sValor, string sName, string sClase)
                : base("input", sClase)
            {
                this["value"] = sValor;
                this["name"] = sName;
                this["type"] = sType;
            }
        }

        public abstract class ValorCelda : Dictionary<string, string>
        {
            public ValorCelda(string sTag, string sClase)
            {
                this["tag"] = sTag;
                this["class"] = sClase;
            }
        }

        public class ConfigColumna : Dictionary<string, object>
        {
            private const string cTITULO = "title";
            private const string cDATA = "data";
            private const string cSEARCHABLE = "searchable";
            private const string cRENDERIZABLE = "renderizable";
            private const string cORDENABLE = "orderable";
            private const string cVISIBLE = "visible";
            private const string cCLASE = "className";
            private const string cANCHO = "width";
            private const string cEXPORTABLE_EXCEL = "exportableAExcel";

            public ConfigColumna(ResourceManager oRm, string sEtiquetaTitulo, string sDato, bool bOrdenable = true, bool bVisible = true, bool bRenderizable = false,
                bool bPuedeBuscarse = true, string sClase = null, bool bExportableAExcel = true)
                : this(sEtiquetaTitulo, sDato, bOrdenable, bVisible, bRenderizable, bPuedeBuscarse, sClase, bExportableAExcel)
            {
            }

            public ConfigColumna(string sDato, bool bOrdenable = true, bool bVisible = true, bool bRenderizable = false, bool bPuedeBuscarse = true, string sClase = null,
                bool bExportableAExcel = true)
                : this(String.Empty, sDato, bOrdenable, bVisible, bRenderizable, bPuedeBuscarse, sClase, bExportableAExcel)
            {
            }

            public ConfigColumna(string sTitulo, string sDato, bool bOrdenable = true, bool bVisible = true, bool bRenderizable = false, bool bPuedeBuscarse = true, string sClase = null,
                bool bExportableAExcel = true)
            {
                this[cTITULO] = sTitulo;
                this[cDATA] = sDato;
                this[cEXPORTABLE_EXCEL] = bExportableAExcel;
                this.Clase = sClase;
                this[cSEARCHABLE] = bPuedeBuscarse;
                if (bRenderizable)
                    this[cRENDERIZABLE] = true;
                if (!bOrdenable)
                    this[cORDENABLE] = false;
                if (!bVisible)
                    this[cVISIBLE] = false;
            }

            private bool ChequearPropiedadBooleana(string sNombrePropiedad)
            {
                return this.ContainsKey(sNombrePropiedad) && Convert.ToBoolean(this[sNombrePropiedad]);
            }

            public string Titulo
            {
                get { return this[cTITULO].ToString(); }
            }

            public string Data
            {
                get { return this[cDATA].ToString(); }
            }

            public bool BusquedaHabilitada
            {
                get { return this.ChequearPropiedadBooleana(cSEARCHABLE); }
            }

            public bool Renderizable
            {
                get { return this.ChequearPropiedadBooleana(cRENDERIZABLE); }
            }

            public bool Visible
            {
                get { return this.ChequearPropiedadBooleana(cVISIBLE); }
            }

            public bool Ordenable
            {
                get { return this.ChequearPropiedadBooleana(cORDENABLE); }
            }

            public string Clase
            {
                get
                {
                    string sClase = null;
                    if (this.ContainsKey(cCLASE))
                        sClase = this[cCLASE].ToString();
                    return sClase;
                }
                set
                {
                    this[cCLASE] = String.Format("{0} {1}", value, (Convert.ToBoolean(this[cEXPORTABLE_EXCEL]) ? " exportable-excel" : "")).Trim();
                }
            }
            public string Ancho
            {
                get
                {
                    string sWidth = null;
                    if (this.ContainsKey(cANCHO))
                        sWidth = this[cANCHO].ToString();
                    return sWidth;
                }
                set
                {
                    this[cANCHO] = String.Format("{0}px", value).Trim();
                }
            }

            public bool ExportableAExcel
            {
                get { return this.ChequearPropiedadBooleana(cEXPORTABLE_EXCEL); }
            }
        }

        public static Dictionary<string, object> GetIdiomasParaDataTable(ResourceManager oRm)
        {
            var dIdiomas = new Dictionary<string, object>();

            dIdiomas.Add("sProcessing", "Procesando...");
            dIdiomas.Add("sLengthMenu", "Mostrar _MENU_ registros");
            dIdiomas.Add("sZeroRecords", "No hay items para mostrar");
            dIdiomas.Add("sEmptyTable", "Ningún dato disponible en esta tabla");
            dIdiomas.Add("sInfo", "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros");
            dIdiomas.Add("sInfoEmpty", "Mostrando registros del 0 al 0 de un total de 0 registros");
            dIdiomas.Add("sInfoFiltered", "(filtrado de un total de _MAX_ registros)");
            dIdiomas.Add("sInfoPostFix", String.Empty);
            dIdiomas.Add("sSearch", GetTextoConSimbolos("Buscar", ":"));
            dIdiomas.Add("sUrl", String.Empty);
            dIdiomas.Add("sInfoThousands", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
            dIdiomas.Add("sLoadingRecords", GetTextoConSimbolos("Cargando", "..."));
            dIdiomas.Add("oPaginate", new Dictionary<string, string>(){
            {"sFirst", "Primero"},
            {"sLast", "Último"},
            {"sNext", " Siguiente"},
            {"sPrevious", " Anterior"}
        });
            dIdiomas.Add("oAria", new Dictionary<string, string>() {
            {"sSortAscending", "Activar para ordenar la columna de manera ascendente"},
            {"sSortDescending", "Activar para ordenar la columna de manera descendente"}
        });
            dIdiomas.Add("ExportarExcel", "Exportar a excel");
            dIdiomas.Add("Aceptar", "Aceptar");
            dIdiomas.Add("Cancelar", "Cancelar");
            return dIdiomas;
        }

        private static string GetTextoConSimbolos(string sEtiqueta, string sSimbolosFinal)
        {
            string sTexto = sEtiqueta;
            if (!sTexto.TrimEnd().EndsWith(sSimbolosFinal))
                sTexto += sSimbolosFinal;
            return sTexto;
        }

        public static dynamic GetConfigColumna<T, X, Y>(jsDataTableHelper.ConfigColumna oConfigCliente, Func<Y, T> fGetValor, Func<T, X> aPreprocesar = null)
        {
            dynamic oConfig = GetConfigColumna<T, X, Y>(oConfigCliente, aPreprocesar);
            oConfig.GetValor = fGetValor;
            return oConfig;
        }

        private static dynamic GetConfigColumna<T, X, Y>(jsDataTableHelper.ConfigColumna oConfigCliente, Func<T, X> aPreprocesar = null)
        {
            dynamic oConfig = new ExpandoObject();
            oConfig.configCliente = oConfigCliente;
            oConfig.Procesar = (Func<Y, X>)((Y oObjeto) =>
            {
                if (aPreprocesar != null)
                    return aPreprocesar(oConfig.GetValor(oObjeto));
                else
                    return Convert.ChangeType(oConfig.GetValor(oObjeto), typeof(X));
            });
            oConfig.CumpleConFiltro = (Func<Y, string, bool>)((Y oObjeto, string sFiltro) =>
            {
                string sValor = Convert.ToString(oConfig.GetValor(oObjeto));
                if (String.IsNullOrEmpty(sValor))
                    return false;
                else
                    return sValor.IndexOf(sFiltro, StringComparison.InvariantCultureIgnoreCase) > -1;
            });
            oConfig.CompararValores = (Func<Y, Y, int>)((Y oObjeto1, Y oObjeto2) =>
            {
                var oPropiedadDeX = oConfig.GetValor(oObjeto1);
                var oPropiedadDeY = oConfig.GetValor(oObjeto2);
                if (oPropiedadDeX == null && oPropiedadDeY == null)
                    return 0;
                else if (oPropiedadDeX == null)
                    return -1;
                else if (oPropiedadDeY == null)
                    return 1;
                else
                    return oPropiedadDeX.CompareTo(oPropiedadDeY);
            });
            return oConfig;
        }

        public static Dictionary<string, object> GetObjetoRespuestaPaginacion<T>(HttpRequest oPeticion, T[] aObjetos, int iTotalObjetos, Dictionary<string, dynamic> dConfigColumnas)
        {
            int iPrimerItem = Convert.ToInt32(oPeticion["start"]),
                iTamanioPagina = Convert.ToInt32(oPeticion["length"]);
            Dictionary<string, object> oObjetoSerializable;
            T oObjeto;
            var lObjetos = new List<Dictionary<string, object>>();
            for (int i = iPrimerItem; i < iTamanioPagina + iPrimerItem && i < aObjetos.Length; i++)
            {
                oObjeto = aObjetos[i];
                oObjetoSerializable = new Dictionary<string, object>();
                foreach (var oPar in dConfigColumnas)
                {
                    oObjetoSerializable[oPar.Key] = oPar.Value.Procesar(oObjeto);
                }
                lObjetos.Add(oObjetoSerializable);
            }
            var oRespuesta = new Dictionary<string, object>();
            oRespuesta["draw"] = Convert.ToInt32(oPeticion["draw"]);
            oRespuesta["recordsTotal"] = iTotalObjetos;
            oRespuesta["recordsFiltered"] = aObjetos.Length;
            oRespuesta["data"] = lObjetos;
            return oRespuesta;
        }

        public static T[] OrdenarYFiltrar<T>(HttpRequest oPeticion, T[] aObjetos, Dictionary<string, dynamic> dConfigColumnas)
        {
            var aObjetosFiltrados = jsDataTableHelper.AplicarFiltro(oPeticion["search[value]"], aObjetos, dConfigColumnas);
            if (!String.IsNullOrEmpty(oPeticion["order[0][column]"]))
            {
                var oConfigColumna = dConfigColumnas[oPeticion["columns[" + oPeticion["order[0][column]"] + "][data]"]];
                Array.Sort(aObjetosFiltrados, (x, y) => oConfigColumna.CompararValores(x, y));
                if (oPeticion["order[0][dir]"].Equals("desc"))
                    Array.Reverse(aObjetosFiltrados);
            }
            return aObjetosFiltrados;
        }

        public static T[] AplicarFiltro<T>(string sFiltro, T[] aObjetos, Dictionary<string, dynamic> dConfigColumnas)
        {
            if (String.IsNullOrEmpty(sFiltro))
            {
                return aObjetos;
            }
            else
            {
                return aObjetos.Where(x =>
                {
                    bool bCumple = false;
                    foreach (var oConfig in dConfigColumnas)
                    {
                        if (oConfig.Value.configCliente.BusquedaHabilitada == true)
                            bCumple = oConfig.Value.CumpleConFiltro(x, sFiltro);
                        if (bCumple)
                            break;
                    }
                    return bCumple;
                }).ToArray();
            }
        }

        public static Dictionary<string, object> GetConfigGrilla(bool bExportableAExcel, ResourceManager oRm)
        {
            int iPagina = 100;
            var dConfig = new Dictionary<string, object>();
            dConfig["exportableAExcel"] = bExportableAExcel;
            dConfig["idioma"] = jsDataTableHelper.GetIdiomasParaDataTable(oRm);
            dConfig["tamanioPagina"] = iPagina;
            return dConfig;
        }
    }
}