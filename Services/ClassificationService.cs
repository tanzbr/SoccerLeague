using SoccerLeague.Data;
using SoccerLeague.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoccerLeague.Services
{
    public class ClassificationService
    {
        private readonly SoccerDbContext db;

        public ClassificationService(SoccerDbContext context)
        {
            db = context;
        }

        public List<Tabela> GenerateClassification()
        {
            var classification = new List<Tabela>();
            var teams = db.Times.ToList(); // pega todos os times

            foreach (var team in teams)
            {
                // zera contadores antes de come�ar
                int pontos = 0, vit = 0, emp = 0, der = 0, golsPro = 0, golsContra = 0;

                // jogos em casa que j� foram disputados
                var homeMatches = db.Partidas
                                    .Where(p => p.TimeCasaId == team.TimeId && p.Jogado)
                                    .ToList();
                foreach (var m in homeMatches)
                {
                    golsPro += m.GolsTimeCasa;
                    golsContra += m.GolsTimeFora;
                    if (m.GolsTimeCasa > m.GolsTimeFora)
                    {
                        vit++;
                        pontos += 3; // vit�ria = 3 pontos
                    }
                    else if (m.GolsTimeCasa == m.GolsTimeFora)
                    {
                        emp++;
                        pontos += 1; // empate = 1 ponto
                    }
                    else
                    {
                        der++;
                    }
                }

                // jogos fora de casa j� disputados
                var awayMatches = db.Partidas
                                    .Where(p => p.TimeForaId == team.TimeId && p.Jogado)
                                    .ToList();
                foreach (var m in awayMatches)
                {
                    golsPro += m.GolsTimeFora;
                    golsContra += m.GolsTimeCasa;
                    if (m.GolsTimeFora > m.GolsTimeCasa)
                    {
                        vit++;
                        pontos += 3;
                    }
                    else if (m.GolsTimeFora == m.GolsTimeCasa)
                    {
                        emp++;
                        pontos += 1;
                    }
                    else
                    {
                        der++;
                    }
                }

                // adiciona resultado da equipe na classifica��o
                classification.Add(new Tabela
                {
                    TimeId = team.TimeId,
                    Time = team,
                    Pontos = pontos,
                    Vitorias = vit,
                    Empates = emp,
                    Derrotas = der,
                    GolsPro = golsPro,
                    GolsContra = golsContra
                });
            }

            // ordena por pontos, saldo de gols e depois gols pr�
            return classification
                .OrderByDescending(t => t.Pontos)
                .ThenByDescending(t => t.SaldoGols)
                .ThenByDescending(t => t.GolsPro)
                .ToList();
        }
    }
}
