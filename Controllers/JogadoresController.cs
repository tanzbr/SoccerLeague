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
    public class JogadoresController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: Jogadores
        // Exemplo de pesquisa por nome, posição e pé preferido.
        public ActionResult Index(string nome, PosicaoJogador? posicao, PePreferido? pe)
        {
            var jogadores = db.Jogadores.Include(j => j.Time).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                jogadores = jogadores.Where(j => j.Nome.Contains(nome));
            }

            if (posicao.HasValue)
            {
                jogadores = jogadores.Where(j => j.Posicao == posicao.Value);
            }

            if (pe.HasValue)
            {
                jogadores = jogadores.Where(j => j.PePreferido == pe.Value);
            }

            return View(jogadores.ToList());
        }

        // GET: Jogadores/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var jogador = db.Jogadores
                            .Include(j => j.Time)
                            .FirstOrDefault(j => j.JogadorId == id.Value);

            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        // GET: Jogadores/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome");
            return View();
        }

        // POST: Jogadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Jogadores.Add(jogador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // GET: Jogadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var jogador = db.Jogadores.Find(id);
            if (jogador == null)
                return HttpNotFound();

            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // POST: Jogadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", jogador.TimeId);
            return View(jogador);
        }

        // GET: Jogadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var jogador = db.Jogadores
                            .Include(j => j.Time)
                            .FirstOrDefault(j => j.JogadorId == id.Value);

            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        // POST: Jogadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var jogador = db.Jogadores.Find(id);
            db.Jogadores.Remove(jogador);
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