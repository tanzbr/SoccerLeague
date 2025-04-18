using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoccerLeague.Services
{
    public class LeagueManager
    {
        private readonly SoccerDbContext _db;

        public LeagueManager(SoccerDbContext context)
        {
            _db = context;
        }

        public List<string> ValidateLeague()
        {
            var errors = new List<string>();

            // Buscar times
            var teams = _db.Times
                           .Include("Jogadores")
                           .Include("ComissaoTecnica")
                           .ToList();

            // Verifica se possui exatamente 20 times
            if (teams.Count != 20)
            {
                errors.Add($"A liga deve ter exatamente 20 times, mas há {teams.Count} cadastrados.");
            }

            foreach (var team in teams)
            {
                // Verificar campos obrigatórios
                if (string.IsNullOrWhiteSpace(team.Nome)
                    || string.IsNullOrWhiteSpace(team.Cidade)
                    || string.IsNullOrWhiteSpace(team.Estado)
                    || team.AnoFundacao <= 0
                    || string.IsNullOrWhiteSpace(team.Estadio)
                    || team.CapacidadeEstadio <= 0
                    || string.IsNullOrWhiteSpace(team.CorUniformePrimaria))
                {
                    errors.Add($"Time {team.Nome}: faltam dados obrigatórios.");
                }

                // Jogadores >= 30
                int jogCount = team.Jogadores?.Count ?? 0;
                if (jogCount < 30)
                    errors.Add($"Time {team.Nome}: somente {jogCount} jogadores (mínimo 30).");

                // Comissão técnica >= 5 e sem cargos duplicados
                int comCount = team.ComissaoTecnica?.Count ?? 0;
                if (comCount < 5)
                {
                    errors.Add($"Time {team.Nome}: comissão técnica com {comCount} membros (mínimo 5).");
                }
                else
                {
                    int distinctRoles = team.ComissaoTecnica
                                            .Select(c => c.Cargo)
                                            .Distinct()
                                            .Count();
                    if (distinctRoles < comCount)
                        errors.Add($"Time {team.Nome}: há cargos repetidos na comissão técnica.");
                }
            }

            return errors;
        }

        public bool IsLeagueReady()
        {
            return !ValidateLeague().Any();
        }
    }
}
