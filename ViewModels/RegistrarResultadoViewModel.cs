using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoccerLeague.Models;

namespace SoccerLeague.ViewModels
{
    public class RegistrarResultadoViewModel
    {
        public Partida Partida { get; set; }
        public List<Jogador> JogadoresCasa { get; set; }
        public List<Jogador> JogadoresFora { get; set; }
        public List<EstatisticaPartida> Estatisticas { get; set; }
    }

}
