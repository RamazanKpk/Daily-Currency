﻿namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UsertId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 40),
                        Password = c.String(nullable: false, maxLength: 10),
                        NormalExchangeRatesAuthorization = c.Boolean(nullable: false),
                        CrossExchangeRatesAuthorization = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UsertId);
            
        }
    }
}