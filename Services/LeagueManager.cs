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
                errors.Add($"A liga deve ter exatamente 20 times, mas h� {teams.Count} cadastrados.");
            }

            foreach (var team in teams)
            {
                // Verificar campos obrigat�rios
                if (string.IsNullOrWhiteSpace(team.Nome)
                    || string.IsNullOrWhiteSpace(team.Cidade)
                    || string.IsNullOrWhiteSpace(team.Estado)
                    || team.AnoFundacao <= 0
                    || string.IsNullOrWhiteSpace(team.Estadio)
                    || team.CapacidadeEstadio <= 0
                    || string.IsNullOrWhiteSpace(team.CorUniformePrimaria))
                {
                    errors.Add($"Time {team.Nome}: faltam dados obrigat�rios.");
                }

                // Jogadores >= 30
                int jogCount = team.Jogadores?.Count ?? 0;
                if (jogCount < 30)
                    errors.Add($"Time {team.Nome}: somente {jogCount} jogadores (m�nimo 30).");

                // Comiss�o t�cnica >= 5 e sem cargos duplicados
                int comCount = team.ComissaoTecnica?.Count ?? 0;
                if (comCount < 5)
                {
                    errors.Add($"Time {team.Nome}: comiss�o t�cnica com {comCount} membros (m�nimo 5).");
                }
                else
                {
                    int distinctRoles = team.ComissaoTecnica
                                            .Select(c => c.Cargo)
                                            .Distinct()
                                            .Count();
                    if (distinctRoles < comCount)
                        errors.Add($"Time {team.Nome}: h� cargos repetidos na comiss�o t�cnica.");
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
