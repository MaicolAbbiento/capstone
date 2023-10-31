namespace capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuovocampotitolocalsserecensioni : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.recensioni", "titolo", c => c.String());
            AlterColumn("dbo.recensioni", "valutazione", c => c.Decimal(nullable: false, precision: 3, scale: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.recensioni", "valutazione", c => c.Decimal(precision: 3, scale: 1));
            DropColumn("dbo.recensioni", "titolo");
        }
    }
}
