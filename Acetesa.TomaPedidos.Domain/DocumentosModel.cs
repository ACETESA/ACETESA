using System;


namespace Acetesa.TomaPedidos.Domain
{
    public class DocumentosModel
    {
        public int idDocumento { get; set; }
        public string empresaEmisora { get; set; }
        public int idTipoDocumento { get; set; }
        public string tipoDocRelacionado { get; set; }
        public string serieDocRelacionado { get; set; }
        public string correlativoDocRelacionado { get; set; }
        public string documento { get; set; }
        public string observacion { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string usuarioRegistro { get; set; }

        public class DocumentacionSunat
        {
            public byte[] documentoPDF { get; set; }
            public byte[] documentoXML { get; set; }
            public byte[] documentoCDR { get; set; }
        }
    }
}
