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
    public class TimesController : Controller
    {
        private readonly SoccerDbContext db = new SoccerDbContext();

        // GET: Times
        // Lista todos os times cadastrados
        public ActionResult Index()
        {
            var times = db.Times.ToList();
            return View(times);
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Time time = db.Times.Find(id);
            if (time == null)
                return HttpNotFound();

            return View(time);
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Time time)
        {
            if (ModelState.IsValid)
            {
                db.Times.Add(time);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(time);
        }

        // GET: Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Time time = db.Times.Find(id);
            if (time == null)
                return HttpNotFound();

            return View(time);
        }

        // POST: Times/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Time time)
        {
            if (ModelState.IsValid)
            {
                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(time);
        }

        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Time time = db.Times.Find(id);
            if (time == null)
                return HttpNotFound();

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Time time = db.Times.Find(id);
            db.Times.Remove(time);
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