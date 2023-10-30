namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class e : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.aziende", "RagioneSociale");
        }
        
        public override void Down()
        {
            AddColumn("dbo.aziende", "RagioneSociale", c => c.String(nullable: false));
        }
    }
}
