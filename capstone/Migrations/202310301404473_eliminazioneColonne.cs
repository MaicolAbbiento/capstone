namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eliminazioneColonne : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.prodotti", "nomeprodotto", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.prodotti", "prezzo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.prodotti", "prodottiinmagazzino", c => c.Int(nullable: false));
            DropColumn("dbo.prodotti", "contenutodelprodotto");
            DropColumn("dbo.prodotti", "datascadenza");
            DropColumn("dbo.prodotti", "descrizioneshort");
        }
        
        public override void Down()
        {
            AddColumn("dbo.prodotti", "descrizioneshort", c => c.String());
            AddColumn("dbo.prodotti", "datascadenza", c => c.DateTime());
            AddColumn("dbo.prodotti", "contenutodelprodotto", c => c.String());
            AlterColumn("dbo.prodotti", "prodottiinmagazzino", c => c.Int());
            AlterColumn("dbo.prodotti", "prezzo", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.prodotti", "nomeprodotto", c => c.String(maxLength: 500));
        }
    }
}
