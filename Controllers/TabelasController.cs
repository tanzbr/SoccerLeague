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
    public class TabelasController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: Tabelas
        // Exemplo de pesquisa por nome do time
        public ActionResult Index(string nomeTime)
        {
            var tabelas = db.Tabelas
                            .Include(t => t.Time)
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomeTime))
            {
                tabelas = tabelas.Where(t => t.Time.Nome.Contains(nomeTime));
            }

            // Possível ordenação por pontos, saldo de gols etc.
            tabelas = tabelas.OrderByDescending(t => t.Pontos)
                             .ThenByDescending(t => t.SaldoGols);

            return View(tabelas.ToList());
        }

        // GET: Tabelas/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tabela = db.Tabelas
                           .Include(t => t.Time)
                           .FirstOrDefault(t => t.TabelaId == id.Value);

            if (tabela == null)
                return HttpNotFound();

            return View(tabela);
        }

        // GET: Tabelas/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome");
            return View();
        }

        // POST: Tabelas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tabela tabela)
        {
            if (ModelState.IsValid)
            {
                db.Tabelas.Add(tabela);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", tabela.TimeId);
            return View(tabela);
        }

        // GET: Tabelas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tabela = db.Tabelas.Find(id);
            if (tabela == null)
                return HttpNotFound();

            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", tabela.TimeId);
            return View(tabela);
        }

        // POST: Tabelas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tabela tabela)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabela).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", tabela.TimeId);
            return View(tabela);
        }

        // GET: Tabelas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tabela = db.Tabelas
                           .Include(t => t.Time)
                           .FirstOrDefault(t => t.TabelaId == id.Value);

            if (tabela == null)
                return HttpNotFound();

            return View(tabela);
        }

        // POST: Tabelas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tabela = db.Tabelas.Find(id);
            db.Tabelas.Remove(tabela);
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