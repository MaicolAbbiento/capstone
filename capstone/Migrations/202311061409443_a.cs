namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.utenti", "cvv", c => c.String());
            AddColumn("dbo.utenti", "datascandenza", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.utenti", "datascandenza");
            DropColumn("dbo.utenti", "cvv");
        }
    }
}
