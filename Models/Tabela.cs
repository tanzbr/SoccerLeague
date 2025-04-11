using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SoccerLeague.Models
{
  
    public class Tabela
	{
        [Key]
        public int TabelaId { get; set; }

        public int Pontos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }

        [NotMapped]
        public int SaldoGols
        {
            get
            {
                return GolsPro - GolsContra;
            }
        }

        // Chave estrangeira para Time
        [ForeignKey("Time")]
        public int TimeId { get; set; }

        // Navegação
        public virtual Time Time { get; set; }
    }
}