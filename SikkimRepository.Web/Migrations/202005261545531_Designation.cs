namespace SikkimRepository.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Designation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Designation", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String(maxLength: 11));
            AddColumn("dbo.AspNetUsers", "SchoolID", c => c.Long(nullable: false));
            AddColumn("dbo.AspNetUsers", "RegDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.AspNetUsers", "RegDate");
            DropColumn("dbo.AspNetUsers", "SchoolID");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "Designation");
        }
    }
}
