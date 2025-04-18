using SoccerLeague.Data;
using SoccerLeague.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SoccerLeague.Controllers
{
    public class ComissaoTecnicaController : Controller
    {
        // nosso DbContext para acessar o banco
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: ComissaoTecnica
        public ActionResult Index(string nome, CargoComissao? cargo)
        {
            // traz todos os membros com o time já incluído
            var comissoes = db.ComissoesTecnicas
                               .Include(c => c.Time)
                               .AsQueryable();

            // filtro por nome (se rolou algo no input)
            if (!string.IsNullOrWhiteSpace(nome))
            {
                comissoes = comissoes.Where(c => c.Nome.Contains(nome));
            }

            // filtro por cargo (se passou cargo)
            if (cargo.HasValue)
            {
                comissoes = comissoes.Where(c => c.Cargo == cargo.Value);
            }

            // retorna a lista já materializada
            return View(comissoes.ToList());
        }

        // GET: ComissaoTecnica/Details/5
        public ActionResult Details(int? id)
        {
            // se não veio id, dá bad request
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // busca o membro com o time junto
            var membro = db.ComissoesTecnicas
                           .Include(c => c.Time)
                           .FirstOrDefault(c => c.ComissaoTecnicaId == id.Value);

            // se não achou, 404
            if (membro == null)
                return HttpNotFound();

            return View(membro);
        }

        // GET: ComissaoTecnica/Create
        public ActionResult Create()
        {
            // popula dropdown de times
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
                // checa duplicidade de cargo no mesmo time
                bool cargoRepetido = db.ComissoesTecnicas.Any(c =>
                    c.TimeId == membro.TimeId &&
                    (int)c.Cargo == (int)membro.Cargo);

                if (cargoRepetido)
                {
                    ModelState.AddModelError("Cargo", "Já existe um membro com esse cargo para esse time.");
                    ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", membro.TimeId);
                    return View(membro);
                }

                // tá tudo certo, salva
                db.ComissoesTecnicas.Add(membro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // se chegou aqui, rolou algo de errado no ModelState
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
                // verifica duplicidade, porém ignora o próprio registro
                bool cargoRepetido = db.ComissoesTecnicas.Any(c =>
                    c.TimeId == membro.TimeId &&
                    (int)c.Cargo == (int)membro.Cargo &&
                    c.ComissaoTecnicaId != membro.ComissaoTecnicaId);

                if (cargoRepetido)
                {
                    ModelState.AddModelError("Cargo", "Já existe um membro com esse cargo para esse time.");
                    ViewBag.TimeId = new SelectList(db.Times, "TimeId", "Nome", membro.TimeId);
                    return View(membro);
                }

                // persiste alterações
                db.Entry(membro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // se ModelState falhou, repopula dropdown e mostra view
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
            // aqui só remove e salva de novo
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
