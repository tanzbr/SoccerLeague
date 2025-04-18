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
        public Time()
        {
            Jogadores = new HashSet<Jogador>();
            ComissaoTecnica = new HashSet<ComissaoTecnica>();
        }

        [Key]
        public int TimeId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo Estado deve ter 2 caracteres (UF).")]
        public string Estado { get; set; }

        [Range(1800, 2100, ErrorMessage = "O ano de fundação deve ser válido.")]
        public int AnoFundacao { get; set; }

        [Required(ErrorMessage = "O campo Estádio é obrigatório.")]
        [StringLength(100)]
        public string Estadio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A capacidade do estádio deve ser um valor positivo.")]
        public int CapacidadeEstadio { get; set; }

        [Required(ErrorMessage = "A cor do uniforme primário é obrigatória.")]
        [StringLength(50)]
        public string CorUniformePrimaria { get; set; }

        [StringLength(50)]
        public string CorUniformeSecundaria { get; set; }

        public bool Status { get; set; }

        // Navegação
        public virtual ICollection<Jogador> Jogadores { get; set; }
        public virtual ICollection<ComissaoTecnica> ComissaoTecnica { get; set; }
    }
}
