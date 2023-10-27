namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nomeprodotti : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.prodotti", "nomeprodotto", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.prodotti", "nomeprodotto", c => c.String(maxLength: 20));
        }
    }
}
