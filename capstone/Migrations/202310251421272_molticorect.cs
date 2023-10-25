namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class molticorect : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.carello", name: "idUtenti", newName: "utenti_idUtenti");
            RenameColumn(table: "dbo.vendita", name: "idprodotti", newName: "prodotti_idprodotti");
            RenameIndex(table: "dbo.carello", name: "IX_idUtenti", newName: "IX_utenti_idUtenti");
            RenameIndex(table: "dbo.vendita", name: "IX_idprodotti", newName: "IX_prodotti_idprodotti");
            AddColumn("dbo.carello", "idprodotti", c => c.Int());
            AddColumn("dbo.vendita", "prezzotot", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.vendita", "idUtenti", c => c.Int());
            CreateIndex("dbo.carello", "idprodotti");
            CreateIndex("dbo.vendita", "idUtenti");
            AddForeignKey("dbo.vendita", "idUtenti", "dbo.utenti", "idUtenti");
            AddForeignKey("dbo.carello", "idprodotti", "dbo.prodotti", "idprodotti");
            DropColumn("dbo.carello", "prezzotot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.carello", "prezzotot", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.carello", "idprodotti", "dbo.prodotti");
            DropForeignKey("dbo.vendita", "idUtenti", "dbo.utenti");
            DropIndex("dbo.vendita", new[] { "idUtenti" });
            DropIndex("dbo.carello", new[] { "idprodotti" });
            DropColumn("dbo.vendita", "idUtenti");
            DropColumn("dbo.vendita", "prezzotot");
            DropColumn("dbo.carello", "idprodotti");
            RenameIndex(table: "dbo.vendita", name: "IX_prodotti_idprodotti", newName: "IX_idprodotti");
            RenameIndex(table: "dbo.carello", name: "IX_utenti_idUtenti", newName: "IX_idUtenti");
            RenameColumn(table: "dbo.vendita", name: "prodotti_idprodotti", newName: "idprodotti");
            RenameColumn(table: "dbo.carello", name: "utenti_idUtenti", newName: "idUtenti");
        }
    }
}
