namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.prodotti", "descrizioneshort", c => c.String());
            DropColumn("dbo.prodotti", "descrizioneshot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.prodotti", "descrizioneshot", c => c.String());
            DropColumn("dbo.prodotti", "descrizioneshort");
        }
    }
}
