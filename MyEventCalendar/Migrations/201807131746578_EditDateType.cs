namespace MyEventCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDateType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventModels", "Date", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventModels", "Date", c => c.DateTime());
        }
    }
}
