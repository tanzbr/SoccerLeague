using System;
using System.Data.Entity.Migrations;

public partial class AddRoundNumberToPartida : DbMigration
{
    public override void Up()
    {
        AddColumn("dbo.Partidas", "RoundNumber", c => c.Int(nullable: false));
    }
    
    public override void Down()
    {
        DropColumn("dbo.Partidas", "RoundNumber");
    }
}
