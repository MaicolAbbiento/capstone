namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.aziende",
                c => new
                    {
                        idaziende = c.Int(nullable: false, identity: true),
                        nomeazienda = c.String(nullable: false, maxLength: 60),
                        piva = c.String(nullable: false, maxLength: 11),
                        paese = c.String(nullable: false, maxLength: 100),
                        citta = c.String(nullable: false),
                        provinca = c.String(nullable: false),
                        CAP = c.String(nullable: false, maxLength: 5),
                        RagioneSociale = c.String(nullable: false),
                        telfonoaziendale = c.String(nullable: false, maxLength: 20),
                        inattesa = c.Boolean(nullable: false),
                        dataAprovazione = c.DateTime(),
                        verificata = c.Boolean(nullable: false),
                        idcategoria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idaziende)
                .ForeignKey("dbo.Categorias", t => t.idcategoria, cascadeDelete: true)
                .Index(t => t.idcategoria);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        idcategoria = c.Int(nullable: false, identity: true),
                        categoria = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.idcategoria);
            
            CreateTable(
                "dbo.imprenditori",
                c => new
                    {
                        idimpreditori = c.Int(nullable: false, identity: true),
                        idaziende = c.Int(),
                        idUtenti = c.Int(),
                    })
                .PrimaryKey(t => t.idimpreditori)
                .ForeignKey("dbo.aziende", t => t.idaziende)
                .ForeignKey("dbo.utenti", t => t.idUtenti)
                .Index(t => t.idaziende)
                .Index(t => t.idUtenti);
            
            CreateTable(
                "dbo.utenti",
                c => new
                    {
                        idUtenti = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 100),
                        cognome = c.String(nullable: false, maxLength: 100),
                        password = c.String(nullable: false, maxLength: 40),
                        username = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 50),
                        residenza = c.String(maxLength: 100),
                        citta = c.String(maxLength: 55),
                        accauntVerificato = c.Boolean(),
                        role = c.String(maxLength: 20),
                        numeroditelefono = c.String(nullable: false, maxLength: 20),
                        codicefiscale = c.String(maxLength: 16),
                        numeroCartaDiCredito = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.idUtenti);
            
            CreateTable(
                "dbo.carello",
                c => new
                    {
                        idcarello = c.Int(nullable: false, identity: true),
                        quantita = c.Int(),
                        prezzotot = c.Decimal(precision: 18, scale: 2),
                        idvendita = c.Int(),
                        idUtenti = c.Int(),
                    })
                .PrimaryKey(t => t.idcarello)
                .ForeignKey("dbo.utenti", t => t.idUtenti)
                .ForeignKey("dbo.vendita", t => t.idvendita)
                .Index(t => t.idvendita)
                .Index(t => t.idUtenti);
            
            CreateTable(
                "dbo.vendita",
                c => new
                    {
                        idvendita = c.Int(nullable: false, identity: true),
                        idprodotti = c.Int(),
                        stato = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.idvendita)
                .ForeignKey("dbo.prodotti", t => t.idprodotti)
                .Index(t => t.idprodotti);
            
            CreateTable(
                "dbo.prodotti",
                c => new
                    {
                        idprodotti = c.Int(nullable: false, identity: true),
                        fotoprodotto = c.String(),
                        nomeprodotto = c.String(maxLength: 20),
                        contenutodelprodotto = c.String(),
                        prezzo = c.Decimal(precision: 18, scale: 2),
                        prodottiinmagazzino = c.Int(),
                        datascadenza = c.DateTime(),
                        descrizioneshot = c.String(),
                        descrizione = c.String(),
                        valutazione = c.Int(),
                        invendita = c.Boolean(),
                        idaziende = c.Int(),
                        idcategoria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.idprodotti)
                .ForeignKey("dbo.aziende", t => t.idaziende)
                .ForeignKey("dbo.Categorias", t => t.idcategoria, cascadeDelete: true)
                .Index(t => t.idaziende)
                .Index(t => t.idcategoria);
            
            CreateTable(
                "dbo.recensioni",
                c => new
                    {
                        idrecensioni = c.Int(nullable: false, identity: true),
                        valutazione = c.Decimal(precision: 3, scale: 1),
                        descrizione = c.String(),
                        idUtenti = c.Int(),
                        idprodotti = c.Int(),
                        utilita = c.Int(),
                    })
                .PrimaryKey(t => t.idrecensioni)
                .ForeignKey("dbo.prodotti", t => t.idprodotti)
                .ForeignKey("dbo.utenti", t => t.idUtenti)
                .Index(t => t.idUtenti)
                .Index(t => t.idprodotti);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.imprenditori", "idUtenti", "dbo.utenti");
            DropForeignKey("dbo.vendita", "idprodotti", "dbo.prodotti");
            DropForeignKey("dbo.recensioni", "idUtenti", "dbo.utenti");
            DropForeignKey("dbo.recensioni", "idprodotti", "dbo.prodotti");
            DropForeignKey("dbo.prodotti", "idcategoria", "dbo.Categorias");
            DropForeignKey("dbo.prodotti", "idaziende", "dbo.aziende");
            DropForeignKey("dbo.carello", "idvendita", "dbo.vendita");
            DropForeignKey("dbo.carello", "idUtenti", "dbo.utenti");
            DropForeignKey("dbo.imprenditori", "idaziende", "dbo.aziende");
            DropForeignKey("dbo.aziende", "idcategoria", "dbo.Categorias");
            DropIndex("dbo.recensioni", new[] { "idprodotti" });
            DropIndex("dbo.recensioni", new[] { "idUtenti" });
            DropIndex("dbo.prodotti", new[] { "idcategoria" });
            DropIndex("dbo.prodotti", new[] { "idaziende" });
            DropIndex("dbo.vendita", new[] { "idprodotti" });
            DropIndex("dbo.carello", new[] { "idUtenti" });
            DropIndex("dbo.carello", new[] { "idvendita" });
            DropIndex("dbo.imprenditori", new[] { "idUtenti" });
            DropIndex("dbo.imprenditori", new[] { "idaziende" });
            DropIndex("dbo.aziende", new[] { "idcategoria" });
            DropTable("dbo.recensioni");
            DropTable("dbo.prodotti");
            DropTable("dbo.vendita");
            DropTable("dbo.carello");
            DropTable("dbo.utenti");
            DropTable("dbo.imprenditori");
            DropTable("dbo.Categorias");
            DropTable("dbo.aziende");
        }
    }
}
