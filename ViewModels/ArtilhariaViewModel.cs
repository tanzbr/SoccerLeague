using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoccerLeague.Models;

namespace SoccerLeague.ViewModels
{
    public class ArtilhariaViewModel
    {
        public int Posicao { get; set; }
        public int JogadorId { get; set; }
        public string NomeJogador { get; set; }
        public string NomeTime { get; set; }
        public int TotalGols { get; set; }
    }
}
