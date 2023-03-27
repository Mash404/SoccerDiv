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
    public class PlayersController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Players
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Sport).Include(p => p.Team);
            return View(players.ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Player_ID,Sports_ID,Team_ID,Player_Name,Player_Age,Player_Nationality,Player_Position,Player_Rating,Player_Ranking,Player_Image")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", player.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", player.Team_ID);
            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", player.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", player.Team_ID);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player plyr)
        {
            string fileName = Path.GetFileNameWithoutExtension(plyr.PlayerImageFile.FileName);
            string extension = Path.GetExtension(plyr.PlayerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            plyr.Player_Image = "~/TeamImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TeamImage/"), fileName);
            plyr.PlayerImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.Entry(plyr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", plyr.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", plyr.Team_ID);
            return View(plyr);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("PlayerList");
        }



        [HttpGet]
        public ActionResult AddPlayer()
        {
            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name");
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }



        [HttpPost]
        public ActionResult AddPlayer(Player plr)
        {
            string fileName = Path.GetFileNameWithoutExtension(plr.PlayerImageFile.FileName);
            string extension = Path.GetExtension(plr.PlayerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            plr.Player_Image = "~/TeamImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/TeamImage/"), fileName);
            plr.PlayerImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Players.Add(plr);
                db.SaveChanges();
                ViewBag.Success = "Successfully added";
            }
            else
            {
                ViewBag.Failed = "Something went wrong! PLease try again";
            }

            ViewBag.Sports_ID = new SelectList(db.Sports, "Sports_ID", "Sports_Name", plr.Sports_ID);
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", plr.Team_ID);
            ModelState.Clear();
            return View();

        }


        //for admin
        public ActionResult PlayerList()
        {
            var players = db.Players.Include(p => p.Sport).Include(p => p.Team);
            return View(players.ToList());
        }


        //for customer
        public ActionResult NewZealand()
        {
            var newzealand = db.Players.Where(u => u.Team.Team_ID == 1).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (newzealand != null)
            {
                return View(newzealand);
            }
            else
            {
                ViewBag.Newzealand = "There is no player in this team";
                return View();
            }
        }


        public ActionResult India()
        {
            var india = db.Players.Where(u => u.Team.Team_ID == 2).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (india != null)
            {
                return View(india);
            }
            else
            {
                ViewBag.India = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Australia()
        {
            var australia = db.Players.Where(u => u.Team.Team_ID == 3).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (australia != null)
            {
                return View(australia);
            }
            else
            {
                ViewBag.Australia = "There is no player in this team";
                return View();
            }
        }

        public ActionResult England()
        {
            var england = db.Players.Where(u => u.Team.Team_ID == 4 && u.Sport.Sports_ID==2).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (england != null)
            {
                return View(england);
            }
            else
            {
                ViewBag.England = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Pakistan()
        {
            var pakistan = db.Players.Where(u => u.Team.Team_ID == 5).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (pakistan != null)
            {
                return View(pakistan);
            }
            else
            {
                ViewBag.Pakistan = "There is no player in this team";
                return View();
            }
        }

        public ActionResult SouthAfrica()
        {
            var southafrica = db.Players.Where(u => u.Team.Team_ID == 6).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (southafrica != null)
            {
                return View(southafrica);
            }
            else
            {
                ViewBag.SouthAfrica = "There is no player in this team";
                return View();
            }
        }

        public ActionResult WestIndies()
        {
            var westindies = db.Players.Where(u => u.Team.Team_ID == 7).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (westindies != null)
            {
                return View(westindies);
            }
            else
            {
                ViewBag.WestIndies = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Srilanka()
        {
            var srilanka = db.Players.Where(u => u.Team.Team_ID == 8).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (srilanka != null)
            {
                return View(srilanka);
            }
            else
            {
                ViewBag.Srilanka = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Bangladesh()
        {
            var bangladesh = db.Players.Where(u => u.Team.Team_ID == 9).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (bangladesh != null)
            {
                return View(bangladesh);
            }
            else
            {
                ViewBag.Bangladesh = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Zimbabwe()
        {
            var zimbabwe = db.Players.Where(u => u.Team.Team_ID == 10).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (zimbabwe != null)
            {
                return View(zimbabwe);
            }
            else
            {
                ViewBag.Zimbabwe = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Afghanistan()
        {
            var afghanistan = db.Players.Where(u => u.Team.Team_ID == 11).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (afghanistan != null)
            {
                return View(afghanistan);
            }
            else
            {
                ViewBag.Afghanistan = "There is no player in this team";
                return View();
            }
        }




        public ActionResult Ireland()
        {
            var ireland = db.Players.Where(u => u.Team.Team_ID == 12).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (ireland != null)
            {
                return View(ireland);
            }
            else
            {
                ViewBag.Ireland = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Netherland()
        {
            var netherland = db.Players.Where(u => u.Team.Team_ID == 13).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (netherland != null)
            {
                return View(netherland);
            }
            else
            {
                ViewBag.Netherland = "There is no player in this team";
                return View();
            }
        }




        public ActionResult Belgium()
        {
            var belgium = db.Players.Where(u => u.Team.Team_ID == 14).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (belgium != null)
            {
                return View(belgium);
            }
            else
            {
                ViewBag.Belgium = "There is no player in this team";
                return View();
            }
        }




        public ActionResult Brazil()
        {
            var brazil = db.Players.Where(u => u.Team.Team_ID == 15).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (brazil != null)
            {
                return View(brazil);
            }
            else
            {
                ViewBag.Brazil = "There is no player in this team";
                return View();
            }
        }

        


        public ActionResult France()
        {
            var france = db.Players.Where(u => u.Team.Team_ID == 17).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (france != null)
            {
                return View(france);
            }
            else
            {
                ViewBag.France = "There is no player in this team";
                return View();
            }
        }




        public ActionResult Italy()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 18).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Italy = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Argentina()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 19).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Argentina = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Portugal()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 20).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Portugal = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Spain()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 21).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Spain = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Mexico()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 22).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Mexico = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Denmark()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 23).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Denmark = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Netherlands()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 24).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Netherlands = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Uruguay()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 25).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Uruguya = "There is no player in this team";
                return View();
            }
        }


        public ActionResult USA()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 26).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.USA = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Germany()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 27).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Germany = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Switzerland()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 28).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Switzerland = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Colombia()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 29).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Colombia = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Croatia()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 30).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Croatia = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Arsenal()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 31).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Arsenal = "There is no player in this team";
                return View();
            }
        }


        public ActionResult AstonVilla()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 32).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.AstonVilla = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Chelsea()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 33).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Chelsea = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Liverpool()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 34).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Liverpool = "There is no player in this team";
                return View();
            }
        }


        public ActionResult ManchesterCity()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 35).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.ManchesterCity = "There is no player in this team";
                return View();
            }
        }


        public ActionResult ManchesterUnited()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 36).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.ManchesterUnited = "There is no player in this team";
                return View();
            }
        }



        public ActionResult TottenhamHotspur()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 37).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.TottenhamHotspur = "There is no player in this team";
                return View();
            }
        }



        public ActionResult NewcastleUnited()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 38).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.NewcastleUnited = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Barcelona()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 39).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Barcelona = "There is no player in this team";
                return View();
            }
        }

        public ActionResult RealMadrid()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 40).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.RealMadrid = "There is no player in this team";
                return View();
            }
        }



        public ActionResult AtléticoMadrid()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 41).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.AtléticoMadrid = "There is no player in this team";
                return View();
            }
        }

        public ActionResult RealBetis()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 42).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.RealBetis = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Sevilla()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 43).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Sevilla = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Valencia()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 44).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Valencia = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Levante()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 45).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Levante = "There is no player in this team";
                return View();
            }
        }


        public ActionResult CeltaVigo()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 46).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.CeltaVigo = "There is no player in this team";
                return View();
            }
        }

        public ActionResult Osasuna()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 47).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Osasuna = "There is no player in this team";
                return View();
            }
        }



        public ActionResult AtleticoBilbao()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 48).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.AtleticoBilbao = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Granada()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 49).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Granada = "There is no player in this team";
                return View();
            }
        }

        public ActionResult RealMallorca()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 50).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.RealMallorca = "There is no player in this team";
                return View();
            }
        }


        public ActionResult RealSociedad()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 51).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.RealSociedad = "There is no player in this team";
                return View();
            }
        }

        public ActionResult RealValladollid()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 52).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.RealValladollid = "There is no player in this team";
                return View();
            }
        }



        public ActionResult WestHam()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 53).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.WestHam = "There is no player in this team";
                return View();
            }
        }


        public ActionResult Southampton()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 54).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Southampton = "There is no player in this team";
                return View();
            }
        }



        public ActionResult Everton()
        {
            var team = db.Players.Where(u => u.Team.Team_ID == 55).ToList();
            //var newzealand = db.Players.SqlQuery("SELECT Team_Logo,Player_Name,Player_Image from Team join Player on Team.Team_ID=Player.Team_ID and Team.Team_ID=1").ToList();
            if (team != null)
            {
                return View(team);
            }
            else
            {
                ViewBag.Everton = "There is no player in this team";
                return View();
            }
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
