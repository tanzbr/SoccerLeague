using System;
using System.Data.Entity.Migrations;

public partial class AddJogadoToPartida : DbMigration
{
    public override void Up()
    {
        AddColumn("dbo.Partidas", "Jogado", c => c.Boolean(nullable: false));
    }
    
    public override void Down()
    {
        DropColumn("dbo.Partidas", "Jogado");
    }
}
