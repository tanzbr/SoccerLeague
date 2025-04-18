using SoccerLeague.Data;
using SoccerLeague.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SoccerLeague.Controllers
{
    public class TabelasController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // POST: Tabelas/SortearPartidas
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SortearPartidas()
        {
            // se já houver partidas, recusa a operação
            if (db.Partidas.Any())
            {
                TempData["SortearError"] = "As partidas já foram sorteadas e não podem ser geradas novamente.";
                return RedirectToAction("Index");
            }

            // Verifica se a liga está pronta
            var leagueManager = new LeagueManager(db);
            var leagueErrors = leagueManager.ValidateLeague();
            if (leagueErrors.Any())
            {
                TempData["SortearError"] = "Não é possível sortear as partidas: " + string.Join(" • ", leagueErrors);
                return RedirectToAction("Index");
            }

            var teams = db.Times.ToList();
            
            // sortear partidas
            var fixtureGenerator = new FixtureGenerator();
            DateTime startDate = DateTime.Today.AddDays(1);
            int daysInterval = 7;
            var fixture = fixtureGenerator.GenerateFixture(teams, startDate, daysInterval);
            db.Partidas.AddRange(fixture);
            db.SaveChanges();

            TempData["SortearSuccess"] = "Partidas sorteadas com sucesso!";
            return RedirectToAction("Index", "Home");
        }

        // POST: Tabelas/ResetarTabela
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetarTabela()
        {
            try
            {
                db.EstatisticasPartidas.RemoveRange(db.EstatisticasPartidas);
                db.Partidas.RemoveRange(db.Partidas);
                db.SaveChanges();
                TempData["ResetSuccess"] = "Todas as partidas e estatísticas foram removidas com sucesso.";
            }
            catch (Exception ex)
            {
                TempData["ResetError"] = "Erro ao resetar: " + ex.Message;
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
