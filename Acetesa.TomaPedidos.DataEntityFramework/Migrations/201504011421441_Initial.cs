namespace Acetesa.TomaPedidos.DataEntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ALM_CIERREMES",
                c => new
                    {
                        cc_anho = c.String(nullable: false, maxLength: 4),
                        cc_mes = c.String(nullable: false, maxLength: 2),
                    })
                .PrimaryKey(t => new { t.cc_anho, t.cc_mes });
            
            CreateTable(
                "dbo.LKARDEX",
                c => new
                    {
                        cc_kardex = c.String(nullable: false, maxLength: 8, fixedLength: true),
                        cc_artic = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        cc_almac = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        ni_corre = c.Int(nullable: false, identity: true),
                        df_kardex = c.DateTime(),
                        nq_artic = c.Decimal(precision: 18, scale: 2),
                        fm_artic = c.Decimal(precision: 18, scale: 2),
                        fm_artic_d = c.Decimal(precision: 18, scale: 2),
                        cb_tipo = c.String(maxLength: 1, fixedLength: true),
                        fm_ultpu = c.Decimal(precision: 18, scale: 2),
                        fm_ultpu_d = c.Decimal(precision: 18, scale: 2),
                        cc_notsal = c.String(maxLength: 12, fixedLength: true),
                        cc_noting = c.String(maxLength: 12, fixedLength: true),
                        cb_estado = c.String(maxLength: 1),
                        fm_costo_prom = c.Decimal(precision: 18, scale: 2),
                        fm_costo_prom_d = c.Decimal(precision: 18, scale: 2),
                        cn_artxpaq = c.String(maxLength: 12, fixedLength: true),
                        cc_movi = c.String(maxLength: 2, fixedLength: true),
                        cb_servicio = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => new { t.cc_kardex, t.cc_artic, t.cc_almac, t.ni_corre });
            
            CreateTable(
                "dbo.MARTICUL",
                c => new
                    {
                        cc_artic = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        cb_eval_precio = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        cc_gruart = c.String(maxLength: 5, fixedLength: true),
                        cc_famiart = c.String(maxLength: 2, fixedLength: true),
                        cc_marca = c.String(maxLength: 6, fixedLength: true),
                        cc_gruartec = c.String(maxLength: 3, fixedLength: true),
                        cc_subgruart = c.String(maxLength: 1, fixedLength: true),
                        cc_modelo = c.String(maxLength: 20, fixedLength: true),
                        cd_artic = c.String(maxLength: 55, fixedLength: true),
                        cn_parara = c.String(maxLength: 15, fixedLength: true),
                        cd_artictc = c.String(maxLength: 60, fixedLength: true),
                        cc_unmed = c.String(maxLength: 2, fixedLength: true),
                        df_ultcom = c.DateTime(),
                        fm_ultcom = c.Double(),
                        df_ultven = c.DateTime(),
                        fm_ultven = c.Double(),
                        cb_undalt = c.String(maxLength: 1, fixedLength: true),
                        cb_activo = c.String(maxLength: 1, fixedLength: true),
                        cb_stocks = c.String(maxLength: 1, fixedLength: true),
                        fq_smin = c.Double(),
                        fq_smax = c.Double(),
                        fq_nivrepos = c.Double(),
                        cb_standard = c.String(maxLength: 1, fixedLength: true),
                        ci_consig = c.String(maxLength: 1, fixedLength: true),
                        cb_nacional = c.String(maxLength: 1, fixedLength: true),
                        cb_critico = c.String(maxLength: 1, fixedLength: true),
                        cb_obsleto = c.String(maxLength: 1, fixedLength: true),
                        cc_barras = c.String(maxLength: 100, fixedLength: true),
                        fm_precprom = c.Double(),
                        ct_graf = c.String(maxLength: 100, fixedLength: true),
                        fm_ulco_d = c.Double(),
                        fm_ulve_d = c.Double(),
                        fm_precprom_d = c.Double(),
                        ci_abc = c.String(maxLength: 1, fixedLength: true),
                        cb_catart = c.String(maxLength: 1, fixedLength: true),
                        fm_ultpu = c.Double(),
                        fm_ultpu_d = c.Double(),
                        fd_stock_cero = c.DateTime(),
                        fm_consumo = c.Double(),
                        fq_sinicial = c.Double(),
                        cb_estado = c.String(maxLength: 1, fixedLength: true),
                        cb_rotacion = c.String(maxLength: 1, fixedLength: true),
                        cn_partnumber = c.String(maxLength: 15, fixedLength: true),
                        cb_uso = c.String(maxLength: 1, fixedLength: true),
                        cn_item = c.String(maxLength: 3, fixedLength: true),
                        fq_embalaje = c.Double(),
                        cc_catalogo = c.String(maxLength: 25),
                        cc_tipoartic = c.String(maxLength: 1, fixedLength: true),
                        fq_espesor = c.Double(),
                        fq_ancho = c.Double(),
                        fq_largo = c.Double(),
                        cc_costeo = c.String(maxLength: 2, fixedLength: true),
                        cc_tipArt = c.String(maxLength: 1, fixedLength: true),
                        cc_costo_kardexpaqbobi = c.String(maxLength: 1, fixedLength: true),
                        df_creacion = c.DateTime(),
                        fq_sku = c.Double(),
                        cc_articant = c.String(maxLength: 10, fixedLength: true),
                        cc_color = c.String(maxLength: 2, fixedLength: true),
                        cb_peso_pt = c.String(maxLength: 1, fixedLength: true),
                        cb_mprima = c.String(maxLength: 1, fixedLength: true),
                        cc_simbolo = c.String(maxLength: 10, fixedLength: true),
                        cc_costeo_pocl = c.String(maxLength: 2, fixedLength: true),
                        cc_codpeso = c.String(maxLength: 2, fixedLength: true),
                        cc_gruartecduf = c.String(maxLength: 3),
                        cc_subgruartduf = c.String(maxLength: 1),
                        c_fl_afecto_percepcion = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => new { t.cc_artic, t.cb_eval_precio });
            
            CreateTable(
                "dbo.SARTICXALMACEN",
                c => new
                    {
                        cc_anho = c.String(nullable: false, maxLength: 4, fixedLength: true),
                        cc_mes = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        cc_artic = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        cc_locac = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        cc_almac = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        fq_saldo_inicial = c.Decimal(precision: 18, scale: 2),
                        fq_ingresos = c.Decimal(precision: 18, scale: 2),
                        fq_egresos = c.Decimal(precision: 18, scale: 2),
                        fq_saldo_final = c.Decimal(precision: 18, scale: 2),
                        fm_monto_ing_nac = c.Decimal(precision: 18, scale: 2),
                        fm_monto_sal_nac = c.Decimal(precision: 18, scale: 2),
                        fm_monto_ing_ext = c.Decimal(precision: 18, scale: 2),
                        fm_monto_sal_ext = c.Decimal(precision: 18, scale: 2),
                        fm_costo_prom_nac = c.Decimal(precision: 18, scale: 2),
                        fm_costo_prom_ext = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_nac = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_ext = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_final_nac = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_final_ext = c.Decimal(precision: 18, scale: 2),
                        fm_costo_nac = c.Decimal(precision: 18, scale: 2),
                        fm_costo_ext = c.Decimal(precision: 18, scale: 2),
                        fm_monto_ajus_nac = c.Decimal(precision: 18, scale: 2),
                        fm_monto_ajus_ext = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_ant_ajus_nac = c.Decimal(precision: 18, scale: 2),
                        fm_saldo_ant_ajus_ext = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.cc_anho, t.cc_mes, t.cc_artic, t.cc_locac, t.cc_almac });
            
            CreateTable(
                "dbo.TALMACEN",
                c => new
                    {
                        cc_almac = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        cc_tipana = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        cc_locac = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        cc_analis = c.String(nullable: false, maxLength: 11, fixedLength: true),
                        cc_cta = c.String(nullable: false, maxLength: 9, fixedLength: true),
                        cc_encar1 = c.String(nullable: false, maxLength: 11, fixedLength: true),
                        cc_encar2 = c.String(nullable: false, maxLength: 11, fixedLength: true),
                        cc_catalm = c.String(maxLength: 2, fixedLength: true),
                        cd_direc = c.String(maxLength: 40, fixedLength: true),
                        cb_princ = c.String(maxLength: 1, fixedLength: true),
                        ci_activo = c.String(maxLength: 1, fixedLength: true),
                        fn_areas = c.Int(),
                        fn_zonas = c.Int(),
                        fn_anaq = c.Int(),
                        fn_and = c.Int(),
                        fn_casil = c.Int(),
                        fm_presup = c.Double(),
                        fm_por_presup = c.Double(),
                        cc_prefijo = c.String(maxLength: 2, fixedLength: true),
                        cb_estado = c.String(maxLength: 1),
                        cd_direccion = c.String(maxLength: 80),
                        cc_ubic_generic = c.String(maxLength: 20, fixedLength: true),
                        cc_tienda = c.String(maxLength: 2, fixedLength: true),
                        cb_valorizar = c.String(maxLength: 1, fixedLength: true),
                        cod_establecimiento = c.String(maxLength: 4, fixedLength: true),
                    })
                .PrimaryKey(t => new { t.cc_almac, t.cc_tipana, t.cc_locac, t.cc_analis, t.cc_cta, t.cc_encar1, t.cc_encar2 });
            
            CreateTable(
                "dbo.TFAMIART",
                c => new
                    {
                        cc_famart = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        cd_famart = c.String(maxLength: 40, fixedLength: true),
                        cb_obsoleto = c.String(maxLength: 1, fixedLength: true),
                        cb_critico = c.String(maxLength: 1, fixedLength: true),
                        cb_partnumber = c.String(maxLength: 1, fixedLength: true),
                        c_fl_afecto_percepcion = c.String(maxLength: 1, fixedLength: true),
                        cb_comision = c.String(maxLength: 1, fixedLength: true),
                        cc_simbolo = c.String(maxLength: 2, fixedLength: true),
                        cc_tipo_exist = c.String(maxLength: 2, fixedLength: true),
                    })
                .PrimaryKey(t => t.cc_famart);
            
            CreateTable(
                "dbo.TGRUARTECDUF",
                c => new
                    {
                        cc_gruartecduf = c.String(nullable: false, maxLength: 3),
                        cd_gruartduf = c.String(maxLength: 45),
                    })
                .PrimaryKey(t => t.cc_gruartecduf);
            
            CreateTable(
                "dbo.TGRUARTEC",
                c => new
                    {
                        cc_gruartec = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        cd_gruart = c.String(maxLength: 45, fixedLength: true),
                    })
                .PrimaryKey(t => t.cc_gruartec);
            
            CreateTable(
                "dbo.TMARCA",
                c => new
                    {
                        cc_marca = c.String(nullable: false, maxLength: 6, fixedLength: true),
                        cd_marca = c.String(maxLength: 35, fixedLength: true),
                        cb_estado = c.String(maxLength: 1, fixedLength: true),
                        cb_tipo = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.cc_marca);
            
            CreateTable(
                "dbo.TMODELO",
                c => new
                    {
                        cc_marca = c.String(nullable: false, maxLength: 6, fixedLength: true),
                        cc_modelo = c.String(nullable: false, maxLength: 20, fixedLength: true),
                        cd_modelo = c.String(maxLength: 35, fixedLength: true),
                        ct_graf = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.cc_marca, t.cc_modelo });
            
            CreateTable(
                "dbo.TSUBGRUARTECDUF",
                c => new
                    {
                        cc_gruartecduf = c.String(nullable: false, maxLength: 3),
                        cc_subgruartduf = c.String(nullable: false, maxLength: 1),
                        cd_subgruartduf = c.String(maxLength: 35),
                    })
                .PrimaryKey(t => new { t.cc_gruartecduf, t.cc_subgruartduf });
            
            CreateTable(
                "dbo.TSUBGRUARTEC",
                c => new
                    {
                        cc_gruartec = c.String(nullable: false, maxLength: 3, fixedLength: true),
                        cc_subgruart = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        cd_subgruart = c.String(maxLength: 35, fixedLength: true),
                    })
                .PrimaryKey(t => new { t.cc_gruartec, t.cc_subgruart });
            
            CreateTable(
                "dbo.TTIPOARTIC",
                c => new
                    {
                        cc_tipoartic = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        cd_tipoartic = c.String(maxLength: 50, fixedLength: true),
                    })
                .PrimaryKey(t => t.cc_tipoartic);
            
            CreateTable(
                "dbo.USUARIOS",
                c => new
                    {
                        c_c_usuario = c.String(nullable: false, maxLength: 8, fixedLength: true),
                        c_t_password = c.String(maxLength: 8, fixedLength: true),
                        c_t_nombre = c.String(maxLength: 40, fixedLength: true),
                        c_fl_activo = c.String(maxLength: 1, fixedLength: true),
                        c_t_cliente = c.String(maxLength: 35),
                        C_C_NIV_SECUENCIA = c.String(maxLength: 1, fixedLength: true),
                        c_c_niv_modifica = c.String(maxLength: 1, fixedLength: true),
                        d_f_conexion = c.DateTime(),
                        c_fl_hextras = c.String(maxLength: 1, fixedLength: true),
                        cb_activo = c.String(maxLength: 1, fixedLength: true),
                        c_t_email = c.String(maxLength: 40, fixedLength: true),
                        c_fl_anticipo = c.String(maxLength: 1, fixedLength: true),
                        c_fl_notpedvta = c.String(maxLength: 1, fixedLength: true),
                        c_fl_cdvta = c.String(maxLength: 1, fixedLength: true),
                        c_fl_limcred = c.String(maxLength: 1, fixedLength: true),
                        c_fl_verFact = c.String(maxLength: 1, fixedLength: true),
                        c_fl_verBole = c.String(maxLength: 1, fixedLength: true),
                        c_fl_verNota = c.String(maxLength: 1, fixedLength: true),
                        c_fl_HD = c.String(maxLength: 1, fixedLength: true),
                        c_fl_clievincul = c.String(maxLength: 1),
                        c_fl_pedalm = c.String(maxLength: 1),
                        cb_aut_papsal = c.String(maxLength: 1, fixedLength: true),
                        cc_usuario_bd = c.String(maxLength: 20, fixedLength: true),
                        cc_password_bd = c.String(maxLength: 20, fixedLength: true),
                        c_fl_cat_mat = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.c_c_usuario);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.USUARIOS");
            DropTable("dbo.TTIPOARTIC");
            DropTable("dbo.TSUBGRUARTEC");
            DropTable("dbo.TSUBGRUARTECDUF");
            DropTable("dbo.TMODELO");
            DropTable("dbo.TMARCA");
            DropTable("dbo.TGRUARTEC");
            DropTable("dbo.TGRUARTECDUF");
            DropTable("dbo.TFAMIART");
            DropTable("dbo.TALMACEN");
            DropTable("dbo.SARTICXALMACEN");
            DropTable("dbo.MARTICUL");
            DropTable("dbo.LKARDEX");
            DropTable("dbo.ALM_CIERREMES");
        }
    }
}
