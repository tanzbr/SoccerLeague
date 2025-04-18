using SoccerLeague.Data;
using SoccerLeague.Services;
using SoccerLeague.ViewModels;
using SoccerLeague.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SoccerLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();
        private readonly ClassificationService classificationService;

        public HomeController()
        {
            classificationService = new ClassificationService(db);
        }

        // GET: Home
        public ActionResult Index(int? round)
        {
            // gera a tabela de classificação usando o service
            List<Tabela> classification = classificationService.GenerateClassification();

            // pega todas as rodadas que já existem no banco
            var rounds = db.Partidas
                           .Select(p => p.RoundNumber)
                           .Distinct()
                           .OrderBy(r => r)
                           .ToList();

            // define limites e valores padrão para navegação de rodadas
            int maxRound = rounds.Any() ? rounds.Max() : 0;
            int current = round ?? (rounds.Any() ? rounds.Min() : 0);
            int prev = rounds.Where(r => r < current).DefaultIfEmpty().Max();
            int next = rounds.Where(r => r > current).DefaultIfEmpty().Min();

            // carrega partidas da rodada atual, se houver alguma
            List<Partida> partidas = new List<Partida>();
            if (current > 0)
            {
                partidas = db.Partidas
                             .Include(p => p.TimeCasa)
                             .Include(p => p.TimeFora)
                             .Where(p => p.RoundNumber == current)
                             .OrderBy(p => p.DataPartida)
                             .ToList();
            }

            // monta o ViewModel com tudo que a view precisa
            var model = new HomeIndexViewModel
            {
                Tabela = classification,
                Partidas = partidas,
                CurrentRound = current,
                PrevRound = prev,
                NextRound = next,
                MaxRound = maxRound
            };

            // roda uma validação extra na liga e vê se já tem partidas
            var leagueManager = new LeagueManager(db);
            var leagueErrors = leagueManager.ValidateLeague();
            bool isLeagueReady = !leagueErrors.Any();
            bool hasMatches = db.Partidas.Any();

            // passa avisos e flags pra view, inclusive mensagens do TempData
            ViewBag.LeagueErrors = leagueErrors;
            ViewBag.IsLeagueReady = isLeagueReady;
            ViewBag.HasMatches = hasMatches;
            ViewBag.SortearError = TempData["SortearError"];
            ViewBag.SortearSuccess = TempData["SortearSuccess"];
            ViewBag.ResetError = TempData["ResetError"];
            ViewBag.ResetSuccess = TempData["ResetSuccess"];

            return View(model);
        }
    }
}
