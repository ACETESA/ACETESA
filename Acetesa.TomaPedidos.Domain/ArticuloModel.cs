namespace Acetesa.TomaPedidos.Domain
{
    public class ArticuloModel 
    {
        public string cc_artic { get; set; }
        public string cd_artic { get; set; }
        public string cc_unmed { get; set; }

        public class ArticuloGS
        {
            public string idGrupo { get; set; }
            public string idSubgrupo { get; set; }
        }
        public class Stock {
            public int stock { get; set; }
            public decimal stockActual { get; set; }
            public string tienda { get; set; }
        }
    }
}
