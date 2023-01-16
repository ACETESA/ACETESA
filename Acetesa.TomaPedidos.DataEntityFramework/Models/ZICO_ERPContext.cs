using System.Data.Entity;
using Acetesa.TomaPedidos.DataEntityFramework.Models.Mapping;
using Acetesa.TomaPedidos.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using InteractivePreGeneratedViews;
using System.Data.Entity.Core.Metadata.Edm;

namespace Acetesa.TomaPedidos.DataEntityFramework.Models
{
    //[DbConfigurationType("Acetesa.TomaPedidos.DataEntityFramework.DbConfigurations.CustomDbConfiguration, Acetesa.TomaPedidos.DataEntityFramework")]
    public partial class ZicoErpContext : DbContext
    {
        //static ZicoErpContext()
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<ZicoErpContext, Configuration>());
        //}

        public ZicoErpContext()
            : base("Name=DefaultConnection")
        {
            Database.CommandTimeout = 3*60;
            Configuration.LazyLoadingEnabled = true;
            //Configuration.AutoDetectChangesEnabled = false;
            //Database.Log = Console.Write;

        }

        public DbSet<AlmCierremes> AlmCierremes { get; set; }
        public DbSet<Lkardex> LkardeXes { get; set; }
        public DbSet<MARTICUL> MarticuLs { get; set; }
        public DbSet<Sarticxalmacen> SarticxalmaceNs { get; set; }
        public DbSet<Talmacen> TalmaceNs { get; set; }
        public DbSet<Tfamiart> TfamiarTs { get; set; }
        public DbSet<Tgruartec> TgruarteCs { get; set; }
        public DbSet<Tgruartecduf> TgruartecduFs { get; set; }
        public DbSet<Tmarca> TmarcAs { get; set; }
        public DbSet<Tmodelo> TmodelOes { get; set; }
        public DbSet<Tsubgruartec> TsubgruarteCs { get; set; }
        public DbSet<Tsubgruartecduf> TsubgruartecduFs { get; set; }
        public DbSet<Ttipoartic> TtipoartiCs { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<LCPROF_WEB> LcprofWebs { get; set; }
        public DbSet<LDPROF_WEB> LdprofWebs { get; set; }
        public DbSet<MCLIENTE> Mclientes { get; set; }
        public DbSet<TCONDVTA> Tcondvtas { get; set; }
        public DbSet<TLISTAPRECIO> Tlistaprecios { get; set; }
        public DbSet<TMONEDA> Tmonedas { get; set; }
        public DbSet<TESTADO> Testados { get; set; }
        public DbSet<tipo_cambio_diario> TipoCambioDiarios { get; set; }

        public DbSet<LCPEDIDO_WEB> LCPEDIDO_WEB { get; set; }
        public DbSet<LDPEDIDO_WEB> LDPEDIDO_WEB { get; set; }
        public DbSet<TSUCCLIE> TSUCCLIEs { get; set; }
        public object InteractiveViewsHelper { get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AlmCierremesMap());
            modelBuilder.Configurations.Add(new LkardexMap());
            modelBuilder.Configurations.Add(new MARTICULMap());
            modelBuilder.Configurations.Add(new SarticxalmacenMap());
            modelBuilder.Configurations.Add(new TalmacenMap());
            modelBuilder.Configurations.Add(new TfamiartMap());
            modelBuilder.Configurations.Add(new TgruartecMap());
            modelBuilder.Configurations.Add(new TgruartecdufMap());
            modelBuilder.Configurations.Add(new TmarcaMap());
            modelBuilder.Configurations.Add(new TmodeloMap());
            modelBuilder.Configurations.Add(new TsubgruartecMap());
            modelBuilder.Configurations.Add(new TsubgruartecdufMap());
            modelBuilder.Configurations.Add(new TtipoarticMap());
            modelBuilder.Configurations.Add(new UsuarioMap());

            modelBuilder.Configurations.Add(new LCPROF_WEBMap());
            modelBuilder.Configurations.Add(new LDPROF_WEBMap());
            modelBuilder.Configurations.Add(new MCLIENTEMap());
            modelBuilder.Configurations.Add(new TCONDVTAMap());
            modelBuilder.Configurations.Add(new TLISTAPRECIOMap());
            modelBuilder.Configurations.Add(new TMONEDAMap());
            modelBuilder.Configurations.Add(new TESTADOMap());
            
            modelBuilder.Configurations.Add(new tipo_cambio_diarioMap());

            modelBuilder.Configurations.Add(new LCPEDIDO_WEBMap());
            modelBuilder.Configurations.Add(new LDPEDIDO_WEBMap());
            modelBuilder.Configurations.Add(new TSUCCLIEMap());
        }

     
    }
}
