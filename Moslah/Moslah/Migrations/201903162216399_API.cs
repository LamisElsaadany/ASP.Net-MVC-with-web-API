namespace Moslah.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class API : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusLocations",
                c => new
                    {
                        BusNumber = c.Int(nullable: false),
                        Source = c.String(nullable: false, maxLength: 128),
                        PriceBus = c.Int(nullable: false),
                        Destination = c.String(),
                        stationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusNumber, t.Source })
                .ForeignKey("dbo.Stations", t => t.stationId, cascadeDelete: true)
                .Index(t => t.stationId);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        cityID = c.Int(nullable: false),
                        Name = c.String(),
                        zone = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.cityID, cascadeDelete: true)
                .Index(t => t.cityID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityCode = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                    })
                .PrimaryKey(t => t.CityCode);
            
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        Destination = c.String(),
                        StatusRoad = c.String(),
                        Date = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        cityID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Cities", t => t.cityID, cascadeDelete: true)
                .Index(t => t.cityID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Age = c.Int(),
                        Phone = c.String(),
                        City = c.String(maxLength: 20),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        Destination = c.String(),
                        StatusRoad = c.String(),
                        Date = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        cityID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Cities", t => t.cityID, cascadeDelete: true)
                .Index(t => t.cityID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.MetroLocations",
                c => new
                    {
                        MetroNumber = c.Int(nullable: false),
                        Source = c.String(nullable: false, maxLength: 128),
                        Destination = c.String(),
                        stationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MetroNumber, t.Source })
                .ForeignKey("dbo.Stations", t => t.stationId, cascadeDelete: true)
                .Index(t => t.stationId);
            
            CreateTable(
                "dbo.MicroBus",
                c => new
                    {
                        MicroID = c.Int(nullable: false),
                        Source = c.String(nullable: false, maxLength: 128),
                        Destination = c.String(),
                        PriceMicro = c.Int(nullable: false),
                        stationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MicroID, t.Source })
                .ForeignKey("dbo.Stations", t => t.stationId, cascadeDelete: true)
                .Index(t => t.stationId);
            
            CreateTable(
                "dbo.QuickSearches",
                c => new
                    {
                        Source = c.String(nullable: false, maxLength: 128),
                        Destination = c.String(nullable: false, maxLength: 128),
                        RoadDesc = c.String(nullable: false, maxLength: 128),
                        VechialType = c.String(),
                        City_CityCode = c.Int(),
                    })
                .PrimaryKey(t => new { t.Source, t.Destination, t.RoadDesc })
                .ForeignKey("dbo.Cities", t => t.City_CityCode)
                .Index(t => t.City_CityCode);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuickSearches", "City_CityCode", "dbo.Cities");
            DropForeignKey("dbo.MicroBus", "stationId", "dbo.Stations");
            DropForeignKey("dbo.MetroLocations", "stationId", "dbo.Stations");
            DropForeignKey("dbo.BusLocations", "stationId", "dbo.Stations");
            DropForeignKey("dbo.Stations", "cityID", "dbo.Cities");
            DropForeignKey("dbo.Descriptions", "cityID", "dbo.Cities");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "cityID", "dbo.Cities");
            DropForeignKey("dbo.News", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Descriptions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.QuickSearches", new[] { "City_CityCode" });
            DropIndex("dbo.MicroBus", new[] { "stationId" });
            DropIndex("dbo.MetroLocations", new[] { "stationId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.News", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.News", new[] { "cityID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Descriptions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Descriptions", new[] { "cityID" });
            DropIndex("dbo.Stations", new[] { "cityID" });
            DropIndex("dbo.BusLocations", new[] { "stationId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.QuickSearches");
            DropTable("dbo.MicroBus");
            DropTable("dbo.MetroLocations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.News");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Descriptions");
            DropTable("dbo.Cities");
            DropTable("dbo.Stations");
            DropTable("dbo.BusLocations");
        }
    }
}
