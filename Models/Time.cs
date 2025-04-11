using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SoccerLeague.Models
{

    public class Time
	{
        [Key]
        public int TimeId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "O campo Estado deve ter 2 caracteres (UF).")]
        public string Estado { get; set; }

        public int AnoFundacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Estadio { get; set; }

        public int CapacidadeEstadio { get; set; }

        [Required]
        [StringLength(50)]
        public string CorUniformePrimaria { get; set; }

        [StringLength(50)]
        public string CorUniformeSecundaria { get; set; }

        // Indica se o time está apto ou não para participar da liga.
        public bool Status { get; set; }

        // Navegação
        public virtual ICollection<Jogador> Jogadores { get; set; }
        public virtual ICollection<ComissaoTecnica> ComissaoTecnica { get; set; }
    }
}