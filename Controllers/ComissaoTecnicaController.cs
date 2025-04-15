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
    public class ComissaoTecnicaController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: ComissaoTecnica
        public ActionResult Index(string nome, CargoComissao? cargo)
        {
            var comissoes = db.ComissoesTecnicas.Include(c => c.Time).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                comissoes = comissoes.Where(c => c.Nome.Contains(nome));
            }

            if (cargo.HasValue)
            {
                comissoes = comissoes.Where(c => c.Cargo == cargo.Value);
            }

            return View(comissoes.ToList());
        }

        // GET: ComissaoTecnica/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var membro = db.ComissoesTecnicas
                           .Include(c => c.Time)
                           .FirstOrDefault(c => c.ComissaoTecnicaId == id.Value);

            if (membro == null)
                return HttpNotFound();

            return View(membro);
        }

        // GET: ComissaoTecnica/Create
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome");
            return View();
        }

        // POST: ComissaoTecnica/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComissaoTecnica membro)
        {
            if (ModelState.IsValid)
            {
                db.ComissoesTecnicas.Add(membro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", membro.TimeId);
            return View(membro);
        }

        // GET: ComissaoTecnica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var membro = db.ComissoesTecnicas.Find(id);
            if (membro == null)
                return HttpNotFound();

            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", membro.TimeId);
            return View(membro);
        }

        // POST: ComissaoTecnica/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComissaoTecnica membro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", membro.TimeId);
            return View(membro);
        }

        // GET: ComissaoTecnica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var membro = db.ComissoesTecnicas
                           .Include(c => c.Time)
                           .FirstOrDefault(c => c.ComissaoTecnicaId == id.Value);

            if (membro == null)
                return HttpNotFound();

            return View(membro);
        }

        // POST: ComissaoTecnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var membro = db.ComissoesTecnicas.Find(id);
            db.ComissoesTecnicas.Remove(membro);
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