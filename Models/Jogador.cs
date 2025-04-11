using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SoccerLeague.Models
{

    public enum PosicaoJogador
    {
        Goleiro,
        Zagueiro,
        Volante,
        Meia,
        Atacante
    }

    public enum PePreferido
    {
        Esquerdo,
        Direito,
        Ambidestro
    }

    public class Jogador
	{
		[Key]
        public int JogadorId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [StringLength(50)]
        public string Nacionalidade { get; set; }

        [Required]
        public PosicaoJogador Posicao { get; set; }

        public int NumeroCamisa { get; set; }

        public float Altura { get; set; }
        public float Peso { get; set; }

        [Required]
        public PePreferido PePreferido { get; set; }

        // Chave estrangeira
        [ForeignKey("Time")]
        public int TimeId { get; set; }

        // Navegação
        public virtual Time Time { get; set; }
	}
}