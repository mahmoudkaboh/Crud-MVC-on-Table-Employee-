namespace ConstructionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDataAnnotationPhone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employee", "Phone", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.Employee", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employee", "Image", c => c.String());
            AlterColumn("dbo.Employee", "Phone", c => c.String(maxLength: 15));
        }
    }
}
