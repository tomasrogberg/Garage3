namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        RegNr = c.String(),
                        Brand = c.String(),
                        ProdName = c.String(),
                        Color = c.String(),
                        Wheels = c.Int(nullable: false),
                        CheckInTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
