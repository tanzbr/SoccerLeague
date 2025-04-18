using System;
using System.Data.Entity.Migrations;

public partial class UpdateSeed : DbMigration
{
    public override void Up()
    {
        CreateTable(
            "dbo.ComissaoTecnicas",
            c => new
                {
                    ComissaoTecnicaId = c.Int(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 100),
                    Cargo = c.Int(nullable: false),
                    DataNascimento = c.DateTime(nullable: false),
                    TimeId = c.Int(nullable: false),
                })
            .PrimaryKey(t => t.ComissaoTecnicaId)
            .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
            .Index(t => t.TimeId);
        
        CreateTable(
            "dbo.Times",
            c => new
                {
                    TimeId = c.Int(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 100),
                    Cidade = c.String(nullable: false, maxLength: 100),
                    Estado = c.String(nullable: false, maxLength: 2),
                    AnoFundacao = c.Int(nullable: false),
                    Estadio = c.String(nullable: false, maxLength: 100),
                    CapacidadeEstadio = c.Int(nullable: false),
                    CorUniformePrimaria = c.String(nullable: false, maxLength: 50),
                    CorUniformeSecundaria = c.String(maxLength: 50),
                    Status = c.Boolean(nullable: false),
                })
            .PrimaryKey(t => t.TimeId);
        
        CreateTable(
            "dbo.Jogadors",
            c => new
                {
                    JogadorId = c.Int(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 100),
                    DataNascimento = c.DateTime(nullable: false),
                    Nacionalidade = c.String(maxLength: 50),
                    Posicao = c.Int(nullable: false),
                    NumeroCamisa = c.Int(nullable: false),
                    Altura = c.Single(nullable: false),
                    Peso = c.Single(nullable: false),
                    PePreferido = c.Int(nullable: false),
                    TimeId = c.Int(nullable: false),
                })
            .PrimaryKey(t => t.JogadorId)
            .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
            .Index(t => t.TimeId);
        
        CreateTable(
            "dbo.EstatisticaPartidas",
            c => new
                {
                    EstatisticaPartidaId = c.Int(nullable: false, identity: true),
                    GolsJogador = c.Int(nullable: false),
                    JogadorId = c.Int(nullable: false),
                    PartidaId = c.Int(nullable: false),
                })
            .PrimaryKey(t => t.EstatisticaPartidaId)
            .ForeignKey("dbo.Jogadors", t => t.JogadorId, cascadeDelete: true)
            .ForeignKey("dbo.Partidas", t => t.PartidaId, cascadeDelete: true)
            .Index(t => t.JogadorId)
            .Index(t => t.PartidaId);
        
        CreateTable(
            "dbo.Partidas",
            c => new
                {
                    PartidaId = c.Int(nullable: false, identity: true),
                    DataPartida = c.DateTime(nullable: false),
                    GolsTimeCasa = c.Int(nullable: false),
                    GolsTimeFora = c.Int(nullable: false),
                    TimeCasaId = c.Int(nullable: false),
                    TimeForaId = c.Int(nullable: false),
                })
            .PrimaryKey(t => t.PartidaId)
            .ForeignKey("dbo.Times", t => t.TimeCasaId)
            .ForeignKey("dbo.Times", t => t.TimeForaId)
            .Index(t => t.TimeCasaId)
            .Index(t => t.TimeForaId);
        
        CreateTable(
            "dbo.Tabelas",
            c => new
                {
                    TabelaId = c.Int(nullable: false, identity: true),
                    Pontos = c.Int(nullable: false),
                    Vitorias = c.Int(nullable: false),
                    Empates = c.Int(nullable: false),
                    Derrotas = c.Int(nullable: false),
                    GolsPro = c.Int(nullable: false),
                    GolsContra = c.Int(nullable: false),
                    TimeId = c.Int(nullable: false),
                })
            .PrimaryKey(t => t.TabelaId)
            .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
            .Index(t => t.TimeId);
        
    }
    
    public override void Down()
    {
        DropForeignKey("dbo.Tabelas", "TimeId", "dbo.Times");
        DropForeignKey("dbo.EstatisticaPartidas", "PartidaId", "dbo.Partidas");
        DropForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times");
        DropForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times");
        DropForeignKey("dbo.EstatisticaPartidas", "JogadorId", "dbo.Jogadors");
        DropForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times");
        DropForeignKey("dbo.Jogadors", "TimeId", "dbo.Times");
        DropIndex("dbo.Tabelas", new[] { "TimeId" });
        DropIndex("dbo.Partidas", new[] { "TimeForaId" });
        DropIndex("dbo.Partidas", new[] { "TimeCasaId" });
        DropIndex("dbo.EstatisticaPartidas", new[] { "PartidaId" });
        DropIndex("dbo.EstatisticaPartidas", new[] { "JogadorId" });
        DropIndex("dbo.Jogadors", new[] { "TimeId" });
        DropIndex("dbo.ComissaoTecnicas", new[] { "TimeId" });
        DropTable("dbo.Tabelas");
        DropTable("dbo.Partidas");
        DropTable("dbo.EstatisticaPartidas");
        DropTable("dbo.Jogadors");
        DropTable("dbo.Times");
        DropTable("dbo.ComissaoTecnicas");
    }
}
