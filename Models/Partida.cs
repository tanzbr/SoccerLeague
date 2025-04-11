using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SoccerLeague.Models
{

    public class Partida
	{
        [Key]
        public int PartidaId { get; set; }

        [Required(ErrorMessage = "A data/hora da partida é obrigatória.")]
        public DateTime DataPartida { get; set; }

        public int GolsTimeCasa { get; set; }
        public int GolsTimeFora { get; set; }

        // Time Casa
        [ForeignKey("TimeCasa")]
        public int TimeCasaId { get; set; }

        // Time Fora
        [ForeignKey("TimeFora")]
        public int TimeForaId { get; set; }

        // Navegação
        public virtual Time TimeCasa { get; set; }
        public virtual Time TimeFora { get; set; }

        // Estatísticas dos jogadores nesta partida
        public virtual ICollection<EstatisticaPartida> Estatisticas { get; set; }
    }

}