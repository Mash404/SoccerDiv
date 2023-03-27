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
    public class TeamsController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Teams
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Sport);
            return View(teams.ToList());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Team_ID,Sports_ID,Team_Name,Team_Description,Team_Rating,Team_Ranking,Team_Logo,Team_Country")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", team.Sports_ID);
            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", team.Sports_ID);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Team tm)
        {
            string fileName = Path.GetFileNameWithoutExtension(tm.TeamImageFile.FileName);
            string extension = Path.GetExtension(tm.TeamImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            tm.Team_Logo = "~/TeamImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TeamImage/"), fileName);
            tm.TeamImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Entry(tm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TeamView");
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", tm.Sports_ID);
            return View(tm);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("TeamView");
        }




        [HttpGet]
        public ActionResult AddTeam()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddTeam(Team withteamimage)
        {
            string fileName = Path.GetFileNameWithoutExtension(withteamimage.TeamImageFile.FileName);
            string extension = Path.GetExtension(withteamimage.TeamImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            withteamimage.Team_Logo = "~/TeamImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TeamImage/"), fileName);
            withteamimage.TeamImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Teams.Add(withteamimage);
                db.SaveChanges();
                ViewBag.Success = "Successfully added";
            }
            else
            {
                ViewBag.Failed = "Something went wrong! PLease try again";
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", withteamimage.Sports_ID);
            ModelState.Clear();
            return View();
        }



        //Football Team//
        [Authorize]
        public ViewResult Football()
        {
            //var football = db.Teams.SqlQuery("select * from Team where Sports_ID=1").ToList();
            var football = db.Teams.Where(u => u.Sports_ID == 1).ToList();
            if (football != null)
            {
                //Session["teamname"] = "football";
                return View(football);
            }
            else
            {
                ViewBag.Football = "There is no team in this sports";
                return View();
            }
        }

        //Cricket Team
        [Authorize]
        public ViewResult Cricket()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var cricket = db.Teams.Where(u => u.Sports_ID == 2).ToList();
            if (cricket != null)
            {
                //Session["teamname"] = "cricket";
                return View(cricket);
            }
            else
            {
                ViewBag.Cricket = "There is no team in this sports";
                return View();
            }
        }


        //Tennis
        [Authorize]
        public ViewResult Tennis()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var tennis = db.Teams.Where(u => u.Sports_ID == 3).ToList();
            if (tennis != null)
            {
                return View(tennis);
            }
            else
            {
                ViewBag.Tennis = "There is no team in this sports";
                return View();
            }
        }


        //Basketball
        [Authorize]
        public ViewResult Basketball()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var basketball = db.Teams.Where(u => u.Sports_ID == 4).ToList();
            if (basketball != null)
            {
                return View(basketball);
            }
            else
            {
                ViewBag.Basketball = "There is no team in this sports";
                return View();
            }
        }


        //Volleyball
        [Authorize]
        public ViewResult Volleyball()
        {
            //var volleyball = db.Teams.SqlQuery("select * from Team where Sports_ID=3").ToList();
            var volleyball = db.Teams.Where(u => u.Sports_ID == 5).ToList();

            if (volleyball != null)
            {
                return View(volleyball);
            }
            else
            {
                ViewBag.Volleyball = "There is no team in this sports";
                return View();
            }
        }

        //Hockey
        [Authorize]
        public ViewResult Hockey()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var hockey = db.Teams.Where(u => u.Sports_ID == 6).ToList();
            if (hockey != null)
            {
                return View(hockey);
            }
            else
            {
                ViewBag.Hockey = "There is no team in this sports";
                return View();
            }
        }

        [Authorize]
        public ViewResult TableTennis()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var tabletennis = db.Teams.Where(u => u.Sports_ID == 7).ToList();
            if (tabletennis != null)
            {
                return View(tabletennis);
            }
            else
            {
                ViewBag.TableTennis = "There is no team in this sports";
                return View();
            }
        }


        //Badminton
        [Authorize]
        public ViewResult Badminton()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var badminton = db.Teams.Where(u => u.Sports_ID == 8).ToList();
            if (badminton != null)
            {
                return View(badminton);
            }
            else
            {
                ViewBag.Badminton = "There is no team in this sports";
                return View();
            }
        }


        [Authorize]
        public ViewResult Baseball()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var baseball = db.Teams.Where(u => u.Sports_ID == 9).ToList();
            if (baseball != null)
            {
                return View(baseball);
            }
            else
            {
                ViewBag.Baseball = "There is no team in this sports";
                return View();
            }
        }

        [Authorize]
        public ViewResult Rugby()
        {
            //var cricket = db.Teams.SqlQuery("select * from Team where Sports_ID=2").ToList();

            var rugby = db.Teams.Where(u => u.Sports_ID == 10).ToList();
            if (rugby != null)
            {
                return View(rugby);
            }
            else
            {
                ViewBag.Rugby = "There is no team in this sports";
                return View();
            }
        }


        public ActionResult TeamView()
        {
            var teams = db.Teams.Include(t => t.Sport);
            return View(teams.ToList());
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
