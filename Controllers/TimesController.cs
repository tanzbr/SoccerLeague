using SoccerLeague.Data;
using SoccerLeague.Models;
using SoccerLeague.ViewModels;
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
            // carrega os times e inclui jogadores e comissão técnica para contagem
            var times = db.Times
                          .Include(t => t.Jogadores)
                          .Include(t => t.ComissaoTecnica)
                          .ToList();

            var timesViewModel = new List<TimeStatusViewModel>();

            foreach (var t in times)
            {
                var pendencias = new List<string>();

                // verifica se o time tem todos os dados preenchidos 
                if (string.IsNullOrWhiteSpace(t.Nome) ||
                    string.IsNullOrWhiteSpace(t.Cidade) ||
                    string.IsNullOrWhiteSpace(t.Estado) ||
                    t.AnoFundacao <= 0 ||
                    string.IsNullOrWhiteSpace(t.Estadio) ||
                    t.CapacidadeEstadio <= 0 ||
                    string.IsNullOrWhiteSpace(t.CorUniformePrimaria))
                {
                    pendencias.Add("Nem todos os dados obrigatórios do time foram preenchidos.");
                }

                // cada time deve ter no mínimo 30 jogadores
                if (t.Jogadores == null || t.Jogadores.Count < 30)
                {
                    pendencias.Add($"Precisa de pelo menos 30 jogadores, mas tem {t.Jogadores?.Count ?? 0}.");
                }

                // comissão técnica deve ter no mínimo 5 profissionais e não pode haver sobreposição de cargos
                if (t.ComissaoTecnica == null || t.ComissaoTecnica.Count < 5)
                {
                    pendencias.Add($"Precisa de pelo menos 5 membros na comissão técnica, mas tem {t.ComissaoTecnica?.Count ?? 0}.");
                }
                else
                {
                    // verifica se não há cargos repetidos
                    var distinctCargos = t.ComissaoTecnica.Select(c => c.Cargo).Distinct().Count();
                    if (distinctCargos < t.ComissaoTecnica.Count)
                    {
                        pendencias.Add("Há sobreposição de cargos na comissão técnica (cargos repetidos).");
                    }
                }

                // se não houver pendências, considera apto
                bool apto = (pendencias.Count == 0);

                timesViewModel.Add(new TimeStatusViewModel
                {
                    Time = t,
                    Apto = apto,
                    Pendencias = pendencias
                });
            }

            // verifica se a liga pode iniciar
            if (times.Count != 20)
            {
                ViewBag.LigaStatus = "A liga não pode iniciar pois não há exatamente 20 times cadastrados.";
            }
            else
            {
                int totalAptos = timesViewModel.Count(vm => vm.Apto);
                if (totalAptos == 20)
                    ViewBag.LigaStatus = "A liga está apta a iniciar! (20 times aptos).";
                else
                    ViewBag.LigaStatus = $"A liga não está apta a iniciar (apenas {totalAptos} de 20 times estão aptos).";
            }

            return View(timesViewModel);
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var time = db.Times
                         .Include(t => t.Jogadores)
                         .Include(t => t.ComissaoTecnica)
                         .FirstOrDefault(t => t.TimeId == id.Value);

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

                TempData["MensagemSucesso"] = "Time cadastrado com sucesso!";
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

                TempData["MensagemSucesso"] = "Time atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(time);
        }


        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Time time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
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