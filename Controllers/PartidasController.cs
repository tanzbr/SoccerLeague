using SoccerLeague.Data;
using SoccerLeague.Models;
using SoccerLeague.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SoccerLeague.Controllers
{
    public class PartidasController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: Partidas
        public ActionResult Index(int? round)
        {
            var partidas = db.Partidas
                             .Include(p => p.TimeCasa)
                             .Include(p => p.TimeFora);

            var rounds = partidas
                         .Select(p => p.RoundNumber)
                         .Distinct()
                         .OrderBy(r => r)
                         .ToList();
            ViewBag.Rounds = new SelectList(rounds, round);

            if (round.HasValue)
                partidas = partidas.Where(p => p.RoundNumber == round.Value);

            ViewBag.SelectedRound = round;
            return View(partidas
                        .OrderBy(p => p.RoundNumber)
                        .ThenBy(p => p.DataPartida)
                        .ToList());
        }

        // GET: Partidas/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partida = db.Partidas
                            .Include(p => p.TimeCasa)
                            .Include(p => p.TimeFora)
                            .FirstOrDefault(p => p.PartidaId == id.Value);
            if (partida == null)
                return HttpNotFound();

            var jogadoresCasa = db.Jogadores
                                  .Where(j => j.TimeId == partida.TimeCasaId)
                                  .ToList();
            var jogadoresFora = db.Jogadores
                                  .Where(j => j.TimeId == partida.TimeForaId)
                                  .ToList();

            var comissaoCasa = db.ComissoesTecnicas
                                 .Where(c => c.TimeId == partida.TimeCasaId)
                                 .ToList();
            var comissaoFora = db.ComissoesTecnicas
                                 .Where(c => c.TimeId == partida.TimeForaId)
                                 .ToList();

            var estatisticas = db.EstatisticasPartidas
                                 .Include(e => e.Jogador)
                                 .Where(e => e.PartidaId == partida.PartidaId)
                                 .ToList();

            var vm = new PartidaDetailsViewModel
            {
                Partida = partida,
                JogadoresCasa = jogadoresCasa,
                JogadoresFora = jogadoresFora,
                ComissaoCasa = comissaoCasa,
                ComissaoFora = comissaoFora,
                Estatisticas = estatisticas
            };

            return View(vm);
        }

        // GET: Partidas/RegistrarResultado/5
        public ActionResult RegistrarResultado(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partida = db.Partidas
                            .Include(p => p.TimeCasa)
                            .Include(p => p.TimeFora)
                            .FirstOrDefault(p => p.PartidaId == id.Value);
            if (partida == null)
                return HttpNotFound();

            var jogadoresCasa = db.Jogadores
                                  .Where(j => j.TimeId == partida.TimeCasaId)
                                  .ToList();
            var jogadoresFora = db.Jogadores
                                  .Where(j => j.TimeId == partida.TimeForaId)
                                  .ToList();

            var viewModel = new RegistrarResultadoViewModel
            {
                Partida = partida,
                JogadoresCasa = jogadoresCasa,
                JogadoresFora = jogadoresFora,
                Estatisticas = new List<EstatisticaPartida>()
            };

            return View(viewModel);
        }

        // POST: Partidas/RegistrarResultado
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RegistrarResultado(RegistrarResultadoViewModel model)
        {
            // inicializa se nulo
            if (model.Estatisticas == null)
                model.Estatisticas = new List<EstatisticaPartida>();

            if (!ModelState.IsValid)
            {
                // recarrega jogadores e retorna a view
                model.JogadoresCasa = db.Jogadores.Where(j => j.TimeId == model.Partida.TimeCasaId).ToList();
                model.JogadoresFora = db.Jogadores.Where(j => j.TimeId == model.Partida.TimeForaId).ToList();
                return View(model);
            }

            var partida = db.Partidas.Find(model.Partida.PartidaId);
            if (partida == null) return HttpNotFound();

            partida.GolsTimeCasa = model.Partida.GolsTimeCasa;
            partida.GolsTimeFora = model.Partida.GolsTimeFora;
            partida.Jogado = true;
            db.Entry(partida).State = EntityState.Modified;
            db.SaveChanges();

            // remove antigos e adiciona novos
            var old = db.EstatisticasPartidas.Where(e => e.PartidaId == partida.PartidaId);
            db.EstatisticasPartidas.RemoveRange(old);
            db.SaveChanges();

            foreach (var e in model.Estatisticas)
            {
                e.PartidaId = partida.PartidaId;
                db.EstatisticasPartidas.Add(e);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
