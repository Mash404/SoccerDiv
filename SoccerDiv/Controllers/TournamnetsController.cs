using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoccerDiv.Models;

namespace SoccerDiv.Controllers
{
    public class TournamnetsController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Tournamnets
        public ActionResult Index()
        {
            var tournamnets = db.Tournamnets.Include(t => t.Sport);
            return View(tournamnets.ToList());
        }

        // GET: Tournamnets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournamnet tournamnet = db.Tournamnets.Find(id);
            if (tournamnet == null)
            {
                return HttpNotFound();
            }
            return View(tournamnet);
        }

        // GET: Tournamnets/Create
        public ActionResult Create()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            return View();
        }

        // POST: Tournamnets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tournament_ID,Sports_ID,Tournament_Name,Tournament_Description,Tournament_Logo")] Tournamnet tournamnet)
        {
            if (ModelState.IsValid)
            {
                db.Tournamnets.Add(tournamnet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", tournamnet.Sports_ID);
            return View(tournamnet);
        }

        // GET: Tournamnets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournamnet tournamnet = db.Tournamnets.Find(id);
            if (tournamnet == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", tournamnet.Sports_ID);
            return View(tournamnet);
        }

        // POST: Tournamnets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tournamnet trmt)
        {
            string fileName = Path.GetFileNameWithoutExtension(trmt.TournamentImageFile.FileName);
            string extension = Path.GetExtension(trmt.TournamentImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            trmt.Tournament_Logo = "~/TournamentImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TournamentImage/"), fileName);
            trmt.TournamentImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(trmt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tournament");
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", trmt.Sports_ID);
            return View(trmt);
        }

        // GET: Tournamnets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournamnet tournamnet = db.Tournamnets.Find(id);
            if (tournamnet == null)
            {
                return HttpNotFound();
            }
            return View(tournamnet);
        }

        // POST: Tournamnets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournamnet tournamnet = db.Tournamnets.Find(id);
            db.Tournamnets.Remove(tournamnet);
            db.SaveChanges();
            return RedirectToAction("Tournament");
        }


        [HttpGet]
        public ActionResult AddTournament()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            return View();
        }


        [HttpPost]
        public ActionResult AddTournament(Tournamnet tour)
        {
            string fileName = Path.GetFileNameWithoutExtension(tour.TournamentImageFile.FileName);
            string extension = Path.GetExtension(tour.TournamentImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            tour.Tournament_Logo = "~/TournamentImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TournamentImage/"), fileName);
            tour.TournamentImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Tournamnets.Add(tour);
                db.SaveChanges();
                ViewBag.Success = "Successfully added";
            }
            else
            {
                ViewBag.Failed = "Something went wrong! PLease try again";
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", tour.Sports_ID);
            ModelState.Clear();
            return View();
        }




        public ActionResult Tournament()
        {
            var tournamnets = db.Tournamnets.Include(t => t.Sport);
            return View(tournamnets.ToList());
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
