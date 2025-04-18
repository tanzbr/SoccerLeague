using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

internal sealed class Configuration : DbMigrationsConfiguration<SoccerDbContext>
{
    public Configuration()
    {
        AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(SoccerDbContext context)
    {
        // Só popular se o banco já existir
        if (!context.Database.Exists())
            return;

        // Só popular se não houver migrações pendentes
        var migratorConfig = new Configuration();
        var dbMigrator = new DbMigrator(migratorConfig);
        var pending = dbMigrator.GetPendingMigrations();
        if (pending.Any())
            return;

        // Se já houver times, abortar
        if (context.Times.Any())
            return;

        // Popular o banco

        var estados = new[] { "SP", "RJ", "MG", "RS", "PR", "SC", "BA", "CE", "PE", "GO", "AM", "PA", "ES", "DF", "MT", "MS", "PB", "PI", "SE", "AL" };
        var cargosComissao = Enum.GetValues(typeof(CargoComissao)).Cast<CargoComissao>().ToList();
        var posicoes = Enum.GetValues(typeof(PosicaoJogador)).Cast<PosicaoJogador>().ToList();
        var pes = Enum.GetValues(typeof(PePreferido)).Cast<PePreferido>().ToList();
        var rand = new Random();

        for (int t = 1; t <= 20; t++)
        {
            var time = new Time
            {
                Nome = $"Time {t:00}",
                Cidade = $"Cidade {t:00}",
                Estado = estados[rand.Next(estados.Length)],
                AnoFundacao = rand.Next(1900, 2020),
                Estadio = $"Estádio {t:00}",
                CapacidadeEstadio = rand.Next(20000, 80000),
                CorUniformePrimaria = $"Cor{rand.Next(1, 100)}",
                CorUniformeSecundaria = $"Cor{rand.Next(101, 200)}",
                Status = true
            };
            context.Times.Add(time);
            context.SaveChanges();

            var usedShirtNumbers = new HashSet<int>();
            for (int j = 1; j <= 30; j++)
            {
                int numero;
                do { numero = rand.Next(1, 100); } while (!usedShirtNumbers.Add(numero));

                context.Jogadores.Add(new Jogador
                {
                    Nome = $"Jogador {t:00}-{j:00}",
                    DataNascimento = DateTime.Today.AddYears(-rand.Next(18, 40)).AddDays(rand.Next(365)),
                    Nacionalidade = "Brasil",
                    Posicao = posicoes[rand.Next(posicoes.Count)],
                    NumeroCamisa = numero,
                    Altura = (float) Math.Round(rand.NextDouble() * 0.3 + 1.6, 2),
                    Peso = (float) Math.Round(rand.NextDouble() * 30 + 65, 1),
                    PePreferido = pes[rand.Next(pes.Count)],
                    TimeId = time.TimeId
                });
            }

            var cargosDisponiveis = new List<CargoComissao>(cargosComissao);
            for (int c = 0; c < 5; c++)
            {
                var cargo = cargosDisponiveis[rand.Next(cargosDisponiveis.Count)];
                cargosDisponiveis.Remove(cargo);

                context.ComissoesTecnicas.Add(new ComissaoTecnica
                {
                    Nome = $"{cargo} {t:00}-{c + 1:00}",
                    DataNascimento = DateTime.Today.AddYears(-rand.Next(30, 60)).AddDays(rand.Next(365)),
                    Cargo = cargo,
                    TimeId = time.TimeId
                });
            }

            context.SaveChanges();
        }

        foreach (var time in context.Times)
        {
            context.Tabelas.Add(new Tabela
            {
                TimeId = time.TimeId,
                Pontos = 0,
                Vitorias = 0,
                Empates = 0,
                Derrotas = 0,
                GolsPro = 0,
                GolsContra = 0
            });
        }
        context.SaveChanges();
    }
}
