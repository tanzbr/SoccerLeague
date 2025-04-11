using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoccerLeague.Models
{
	public class EstatisticaPartida
	{
        [Key]
        public int EstatisticaPartidaId { get; set; }

        public int GolsJogador { get; set; }

        [ForeignKey("Jogador")]
        public int JogadorId { get; set; }

        [ForeignKey("Partida")]
        public int PartidaId { get; set; }

        // Navegação
        public virtual Jogador Jogador { get; set; }
        public virtual Partida Partida { get; set; }
    }
}