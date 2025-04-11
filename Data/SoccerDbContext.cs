using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SoccerLeague.Data
{
    public class SoccerDbContext : DbContext
    {
        public SoccerDbContext()
            : base("SoccerContext")
        {
            
        }

        public DbSet<Time> Times { get; set; }
        public DbSet<Jogador> Jogadores { get; set; }
        public DbSet<ComissaoTecnica> ComissoesTecnicas { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<EstatisticaPartida> EstatisticasPartidas { get; set; }
        public DbSet<Tabela> Tabelas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Desativar Cascade Delete entre Partida -> TimeCasa
            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .WillCascadeOnDelete(false);

            // Desativar Cascade Delete entre Partida -> TimeFora
            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.TimeFora)
                .WithMany()
                .HasForeignKey(p => p.TimeForaId)
                .WillCascadeOnDelete(false);
        }
    }

    }