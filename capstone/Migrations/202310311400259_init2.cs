namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.recensioni", "valutazione", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.recensioni", "valutazione", c => c.Decimal(nullable: false, precision: 3, scale: 1));
        }
    }
}
