using SoccerLeague.Data;
using SoccerLeague.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace SoccerLeague.Controllers
{
    public class ArtilhariaController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: /Artilharia/
        public ActionResult Index()
        {
            // agrupa por jogador e soma gols
            var golsPorJogador = db.EstatisticasPartidas
                .GroupBy(e => e.JogadorId)
                .Select(g => new {
                    JogadorId = g.Key,
                    TotalGols = g.Sum(e => e.GolsJogador)
                })
                .OrderByDescending(x => x.TotalGols)
                .ToList();

            // projeta para o ViewModel, carregando nome e time
            var lista = golsPorJogador
                .Select((x, idx) => {
                    var jogador = db.Jogadores
                                    .Include(j => j.Time)
                                    .FirstOrDefault(j => j.JogadorId == x.JogadorId);
                    return new ArtilhariaViewModel
                    {
                        Posicao = idx + 1,
                        JogadorId = x.JogadorId,
                        NomeJogador = jogador?.Nome,
                        NomeTime = jogador?.Time.Nome,
                        TotalGols = x.TotalGols
                    };
                })
                .ToList();

            return View(lista);
        }
    }
}
