using SoccerLeague.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SoccerLeague.Models;

namespace SoccerLeague.Controllers
{
    public class EstatisticasPartidasController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: EstatisticasPartidas
        public ActionResult Index()
        {
            // traz todas as estatísticas, incluindo jogador e partida
            var estatisticas = db.EstatisticasPartidas
                                 .Include(e => e.Jogador)
                                 .Include(e => e.Partida)
                                 .ToList();
            return View(estatisticas);
        }

        // GET: EstatisticasPartidas/Details/5
        public ActionResult Details(int? id)
        {
            // se não passou o id, retornar BadRequest
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // procura a estatística com o jogador e partida carregados
            var estatistica = db.EstatisticasPartidas
                                .Include(e => e.Jogador)
                                .Include(e => e.Partida)
                                .FirstOrDefault(e => e.EstatisticaPartidaId == id.Value);

            // se não achou, 404
            if (estatistica == null)
                return HttpNotFound();

            return View(estatistica);
        }

        // GET: EstatisticasPartidas/Create
        public ActionResult Create()
        {
            // retorna uma lista de jogadores e partidas pro dropdown
            ViewBag.JogadorId = new SelectList(db.Jogadores, "JogadorId", "Nome");
            ViewBag.PartidaId = new SelectList(db.Partidas, "PartidaId", "PartidaId");
            return View();
        }

        // POST: EstatisticasPartidas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EstatisticaPartida estatistica)
        {
            if (ModelState.IsValid)
            {
                // se tudo OK, adiciona e salva
                db.EstatisticasPartidas.Add(estatistica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // se tiver falha no ModelState, recarrega os dropdowns
            ViewBag.JogadorId = new SelectList(db.Jogadores, "JogadorId", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "PartidaId", "PartidaId", estatistica.PartidaId);
            return View(estatistica);
        }

        // GET: EstatisticasPartidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // busca direto pelo PK
            var estatistica = db.EstatisticasPartidas.Find(id);
            if (estatistica == null)
                return HttpNotFound();

            // repopula dropdowns com o valor atual selecionado
            ViewBag.JogadorId = new SelectList(db.Jogadores, "JogadorId", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "PartidaId", "PartidaId", estatistica.PartidaId);
            return View(estatistica);
        }

        // POST: EstatisticasPartidas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EstatisticaPartida estatistica)
        {
            if (ModelState.IsValid)
            {
                // marca como modificado e manda no SaveChanges
                db.Entry(estatistica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // se falhar, recarrega dropdowns e mostra a view de novo
            ViewBag.JogadorId = new SelectList(db.Jogadores, "JogadorId", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "PartidaId", "PartidaId", estatistica.PartidaId);
            return View(estatistica);
        }

        // GET: EstatisticasPartidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // inclui jogador e partida pra exibir detalhes antes de deletar
            var estatistica = db.EstatisticasPartidas
                                .Include(e => e.Jogador)
                                .Include(e => e.Partida)
                                .FirstOrDefault(e => e.EstatisticaPartidaId == id.Value);

            if (estatistica == null)
                return HttpNotFound();

            return View(estatistica);
        }

        // POST: EstatisticasPartidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var estatistica = db.EstatisticasPartidas.Find(id);
            db.EstatisticasPartidas.Remove(estatistica);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
