using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Dapper;

namespace Acetesa.TomaPedidos.Repository
{
    public class ComandoMapper : ICloneable, IDisposable
    {
        public string CommandName { get; set; }
        public CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public void ReemplazarTexto(string TextoBuscar, string TextoReemplazar)
        {
            this.CommandText = this.CommandText.Replace(TextoBuscar, TextoReemplazar);
        }
        public List<ParametroMapper> Parameters { get; set; }

        public void setParamValue(string sParametro, object oValor)
        {
            Parameters.Find(x => x.ParameterName == "@" + sParametro).ParameterValue = oValor;
        }

        public void AgregarParametroEntrada(string nombre, object valor)
        {
            var oParametro = new ParametroMapper();
            oParametro.ParameterDirection = System.Data.ParameterDirection.Input;
            oParametro.ParameterName = nombre;
            oParametro.ParameterValue = valor;
            oParametro.ParameterType = (DbType)Enum.Parse(typeof(DbType), oParametro.ParameterValue.GetType().Name, true);
            this.Parameters.Add(oParametro);
        }

        public DynamicParameters getParametros()
        {
            var parameters = new DynamicParameters();
            foreach (var oParametro in Parameters)
            {
                parameters.Add("@" + oParametro.ParameterName, ValorParametro(oParametro.ParameterValue), oParametro.ParameterType, oParametro.ParameterDirection, oParametro.ParameterSize);
            }
            return parameters;
        }
        private object ValorParametro(object valor)
        {
            if (valor == null)
            {
                return DBNull.Value;
            }
            else
            {
                return valor;
            }
        }
        public object Clone()
        {
            ComandoMapper oComando = this.MemberwiseClone() as ComandoMapper;
            oComando.Parameters = this.Parameters.Select(item => item.Clone() as ParametroMapper).ToList();
            return oComando;
        }

        public void Dispose()
        {
            this.Parameters = null;
        }
    }

    public class ParametroMapper : ICloneable
    {
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
        public DbType ParameterType { get; set; }
        public int ParameterSize { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone() as ParametroMapper;
        }
    }
    public class Mapper : IDisposable
    {
        const string DATA_SOURCE_DEFAULT = "DefaultConnection";

        private DbConnection _connection = null;
        private string _CadenaConexion = null;
        private static List<ComandoMapper> comandos = null;
        public DbConnection Conexion
        {
            get
            {
                if (_connection == null)
                {
                    this._connection = new SqlConnection(ConfigurationManager.ConnectionStrings[this._CadenaConexion].ConnectionString);
                }
                return this._connection;
            }
        }
        public static ComandoMapper getComando(string sNombre)
        {
            SetearComandos();
            return comandos.Find(x => x.CommandName == sNombre).Clone() as ComandoMapper;
        }

        public Mapper()
        {
            this._CadenaConexion = DATA_SOURCE_DEFAULT;
        }

        public List<T> BuscarTodos<T>(ComandoMapper oComando)
        {
            return Conexion.Query<T>(oComando.CommandText, oComando.getParametros(), commandType: oComando.CommandType).ToList();
        }

        public T BuscarPrimero<T>(ComandoMapper oComando)
        {
            return Conexion.Query<T>(oComando.CommandText, oComando.getParametros(), commandType: oComando.CommandType).FirstOrDefault();
        }

        public void Ejecutar(ComandoMapper oComando)
        {
            Conexion.Execute(oComando.CommandText, oComando.getParametros(), commandType: oComando.CommandType);
        }

        public Mapper(string sCadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(sCadenaConexion))
            {
                this._CadenaConexion = DATA_SOURCE_DEFAULT;
            }
            else
            {
                this._CadenaConexion = sCadenaConexion;
            }
        }

        public void Dispose()
        {
            this._connection = null;
            this._CadenaConexion = null;
        }

        private static void SetearComandos()
        {
            if (comandos != null)
            {
                return;
            }
            comandos = new List<ComandoMapper>();

            string directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase.Replace(@"file:///", ""));
            string _fileMask = "Commands."+ DATA_SOURCE_DEFAULT + ".*.config";
            string[] fileNames = Directory.GetFiles(directoryName, _fileMask);

            if (fileNames.Length == 0)
            {
                throw new ConfigurationErrorsException("No config files found with mask " + directoryName + Path.DirectorySeparatorChar + _fileMask + ".");
            }
            foreach (string fileName in fileNames)
            {
                var xmlComando = XElement.Load(fileName);
                foreach (XElement level1Element in xmlComando.Elements("Command"))
                {
                    var oComando = new ComandoMapper();
                    oComando.CommandText = level1Element.Element("Query").Value;
                    oComando.CommandType = (CommandType)Enum.Parse(typeof(CommandType), level1Element.Attribute("type").Value, true);
                    oComando.CommandName = level1Element.Attribute("name").Value;
                    oComando.Parameters = new List<ParametroMapper>();
                    foreach (XElement level2Element in level1Element.Element("params").Nodes())
                    {
                        var oParametro = new ParametroMapper();
                        oParametro.ParameterName = level2Element.Attribute("name").Value;
                        if (level2Element.Attribute("direction") != null)
                        {
                            oParametro.ParameterDirection = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), level2Element.Attribute("direction").Value);
                        }
                        if (level2Element.Attribute("size") != null)
                        {
                            oParametro.ParameterSize = int.Parse(level2Element.Attribute("size").Value);
                        }
                        if (level2Element.Attribute("type") != null)
                        {
                            oParametro.ParameterType = (DbType)Enum.Parse(typeof(DbType), level2Element.Attribute("type").Value, true);
                        }
                        if (level2Element.Attribute("value") != null)
                        {
                            oParametro.ParameterValue = level2Element.Attribute("value").Value;
                        }

                        oComando.Parameters.Add(oParametro);
                    }
                    comandos.Add(oComando);
                }
            }
        }
    }
}
