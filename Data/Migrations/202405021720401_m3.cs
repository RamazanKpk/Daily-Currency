namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "NormalExchangeRatesAuthorization", c => c.Boolean());
            AlterColumn("dbo.Users", "CrossExchangeRatesAuthorization", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "CrossExchangeRatesAuthorization", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "NormalExchangeRatesAuthorization", c => c.Boolean(nullable: false));
        }
    }
}
