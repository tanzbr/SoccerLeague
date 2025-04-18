using SoccerLeague.Models;
using System.Collections.Generic;

namespace SoccerLeague.ViewModels
{
    public class PartidaDetailsViewModel
    {
        public Partida Partida { get; set; }
        public IEnumerable<Jogador> JogadoresCasa { get; set; }
        public IEnumerable<Jogador> JogadoresFora { get; set; }
        public IEnumerable<ComissaoTecnica> ComissaoCasa { get; set; }
        public IEnumerable<ComissaoTecnica> ComissaoFora { get; set; }
        public IEnumerable<EstatisticaPartida> Estatisticas { get; set; }
    }
}
