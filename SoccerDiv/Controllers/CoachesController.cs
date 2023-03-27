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
    public class CoachesController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Coaches
        public ActionResult Index()
        {
            var coaches = db.Coaches.Include(c => c.Sport).Include(c => c.Team);
            return View(coaches.ToList());
        }

        // GET: Coaches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // GET: Coaches/Create
        public ActionResult Create()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }

        // POST: Coaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Coach_ID,Sports_ID,Team_ID,Coach_Name,Coach_Age,Coach_Nationality,Coach_Image")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                db.Coaches.Add(coach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", coach.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", coach.Team_ID);
            return View(coach);
        }

        // GET: Coaches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", coach.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", coach.Team_ID);
            return View(coach);
        }

        // POST: Coaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Coach_ID,Sports_ID,Team_ID,Coach_Name,Coach_Age,Coach_Nationality,Coach_Image")] Coach coach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", coach.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", coach.Team_ID);
            return View(coach);
        }

        // GET: Coaches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coaches.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coach coach = db.Coaches.Find(id);
            db.Coaches.Remove(coach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult AddCoach()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }


        [HttpPost]
        public ActionResult AddVenue(Coach ch)
        {
            string fileName = Path.GetFileNameWithoutExtension(ch.CoachImageFile.FileName);
            string extension = Path.GetExtension(ch.CoachImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            ch.Coach_Image = "~/CoachImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/CoachImage/"), fileName);
            ch.CoachImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Coaches.Add(ch);
                db.SaveChanges();
                ViewBag.Success = "successfully added";
            }
            else
            {
                ViewBag.Failed = "Failed! Something went wrong! Please try again";
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", ch.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", ch.Team_ID);
            ModelState.Clear();
            return View();
        }


        public ActionResult CoachesList()
        {
            var coaches = db.Coaches.Include(c => c.Sport).Include(c => c.Team);
            return View(coaches.ToList());
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
