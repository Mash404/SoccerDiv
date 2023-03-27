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
    public class SportsController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Sports
        public ActionResult Index()
        {
            return View(db.Sports.ToList());
        }

        // GET: Sports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // GET: Sports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sports_ID,Sports_Name,Sports_Description,Sports_Image")] Sport sport)
        {
            if (ModelState.IsValid)
            {
                db.Sports.Add(sport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sport);
        }

        // GET: Sports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sport sprt)
        {
            string fileName = Path.GetFileNameWithoutExtension(sprt.SportsImageFile.FileName);
            string extension = Path.GetExtension(sprt.SportsImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            sprt.Sports_Image = "~/SportsImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/SportsImage/"), fileName);
            sprt.SportsImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(sprt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SportsView");
            }
            return View(sprt);
        }

        // GET: Sports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sport sport = db.Sports.Find(id);
            if (sport == null)
            {
                return HttpNotFound();
            }
            return View(sport);
        }

        // POST: Sports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sport sport = db.Sports.Find(id);
            db.Sports.Remove(sport);
            db.SaveChanges();
            return RedirectToAction("SportsView");
        }




        [HttpGet]
        public ActionResult AddSports()
        {
            return View();
        }



        [HttpPost]
        public ActionResult AddSports(Sport withimage)
        {
            string fileName = Path.GetFileNameWithoutExtension(withimage.SportsImageFile.FileName);
            string extension = Path.GetExtension(withimage.SportsImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            withimage.Sports_Image = "~/SportsImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/SportsImage/"), fileName);
            withimage.SportsImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Sports.Add(withimage);
                db.SaveChanges();
                ViewBag.Success = "Successfully added";
            }
            else
            {
                ViewBag.Failed = "Something went wrong! PLease try again";
            }
            ModelState.Clear();
            return View();
        }




        public ActionResult SportsView()
        {
            return View(db.Sports.ToList());
        }





        public ActionResult OtherSports()
        {
            return View(db.Sports.ToList());
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
