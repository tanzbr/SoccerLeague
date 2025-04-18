using SoccerLeague.Models;
using System.Collections.Generic;
using System;
using System.Linq;

public class FixtureGenerator
{
    // gerar sorteio de partidas
    public List<Partida> GenerateFixture(List<Time> teams, DateTime startDate, int daysInterval)
    {
        int n = teams.Count;
        if (n % 2 != 0) throw new ArgumentException("Número de times deve ser par.");

        var fixture = new List<Partida>();
        var rotating = teams.Skip(1).ToList();
        Time fixedTeam = teams[0];
        int numRounds = n - 1;
        DateTime date = startDate;


        for (int round = 1; round <= numRounds; round++)
        {
            // jogo fixo
            fixture.Add(new Partida
            {
                DataPartida = date,
                TimeCasaId = fixedTeam.TimeId,
                TimeForaId = rotating.Last().TimeId,
                RoundNumber = round
            });
            // demais confrontos
            int pairs = rotating.Count / 2;
            for (int i = 0; i < pairs; i++)
            {
                fixture.Add(new Partida
                {
                    DataPartida = date,
                    TimeCasaId = rotating[i].TimeId,
                    TimeForaId = rotating[rotating.Count - 1 - i].TimeId,
                    RoundNumber = round
                });
            }
            // roda
            var last = rotating.Last();
            rotating.RemoveAt(rotating.Count - 1);
            rotating.Insert(0, last);
            date = date.AddDays(daysInterval);
        }

        // invertendo casa/fora
        DateTime date2 = date;
        for (int round = 1; round <= numRounds; round++)
        {
            var firstHalfMatches = fixture.Where(p => p.RoundNumber == round).ToList();
            foreach (var m in firstHalfMatches)
            {
                fixture.Add(new Partida
                {
                    DataPartida = date2,
                    TimeCasaId = m.TimeForaId,
                    TimeForaId = m.TimeCasaId,
                    RoundNumber = numRounds + round
                });
            }
            date2 = date2.AddDays(daysInterval);
        }

        return fixture;
    }
}
