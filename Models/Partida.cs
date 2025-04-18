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
        public int PartidaId { get; set; }
        public DateTime DataPartida { get; set; }

        public int TimeCasaId { get; set; }
        public virtual Time TimeCasa { get; set; }

        public int TimeForaId { get; set; }
        public virtual Time TimeFora { get; set; }

        public int RoundNumber { get; set; }
        public bool Jogado { get; set; }

        public int GolsTimeCasa { get; set; }
        public int GolsTimeFora { get; set; }

        public virtual ICollection<EstatisticaPartida> Estatisticas { get; set; }

        public Partida()
        {
            Estatisticas = new HashSet<EstatisticaPartida>();
        }
    }


}