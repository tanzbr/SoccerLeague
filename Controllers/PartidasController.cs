using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoccerLeague.Controllers
{
    public class PartidasController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: Partidas
        // Poderíamos filtrar por Estádio, Data, Rodada etc., mas aqui está simples.
        public ActionResult Index()
        {
            var partidas = db.Partidas
                             .Include(p => p.TimeCasa)
                             .Include(p => p.TimeFora)
                             .ToList();
            return View(partidas);
        }

        // GET: Partidas/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partida = db.Partidas
                            .Include(p => p.TimeCasa)
                            .Include(p => p.TimeFora)
                            .Include(p => p.Estatisticas.Select(e => e.Jogador))
                            .FirstOrDefault(p => p.PartidaId == id.Value);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // GET: Partidas/Create
        public ActionResult Create()
        {
            ViewBag.TimeCasaId = new SelectList(db.Times, "TimeId", "Nome");
            ViewBag.TimeForaId = new SelectList(db.Times, "TimeId", "Nome");
            return View();
        }

        // POST: Partidas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Partidas.Add(partida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeCasaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partida = db.Partidas.Find(id);
            if (partida == null)
                return HttpNotFound();

            ViewBag.TimeCasaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // POST: Partidas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeCasaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "TimeId", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partida = db.Partidas
                            .Include(p => p.TimeCasa)
                            .Include(p => p.TimeFora)
                            .FirstOrDefault(p => p.PartidaId == id.Value);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // POST: Partidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var partida = db.Partidas.Find(id);
            db.Partidas.Remove(partida);
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