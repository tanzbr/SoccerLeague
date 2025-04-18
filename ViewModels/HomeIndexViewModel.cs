using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoccerLeague.Models;

namespace SoccerLeague.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Tabela> Tabela { get; set; }
        public IEnumerable<Partida> Partidas { get; set; }
        public int CurrentRound { get; set; }
        public int PrevRound { get; set; }

        public int NextRound { get; set; }
        public int MaxRound { get; set; }
    }
}
