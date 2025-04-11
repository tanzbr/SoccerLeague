using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoccerLeague.Models

{
    public enum CargoComissao
    {
        Treinador,
        Auxiliar,
        PreparadorFisico,
        Fisiologista,
        TreinadorDeGoleiros,
        Fisioterapeuta
    }

    public class ComissaoTecnica
	{
        [Key]
        public int ComissaoTecnicaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public CargoComissao Cargo { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        // Chave estrangeira
        [ForeignKey("Time")]
        public int TimeId { get; set; }

        // Navegação
        public virtual Time Time { get; set; }
    }
}