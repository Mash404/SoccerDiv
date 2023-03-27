using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SoccerDiv.Models;

namespace SoccerDiv.Controllers
{
    public class CustomersController : Controller
    {
        private SoccerDivEntities db = new SoccerDivEntities();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.Team);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Customer_ID,Team_ID,Customer_Name,Customer_Email,Customer_PhoneNO,Customer_Address,Customer_Password,Customer_Image")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", customer.Team_ID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", customer.Team_ID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer custm)
        {
            string fileName = Path.GetFileNameWithoutExtension(custm.CustomerImageFile.FileName);
            string extension = Path.GetExtension(custm.CustomerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            custm.Customer_Image = "~/CustomerImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/CustomerImage/"), fileName);
            custm.CustomerImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Entry(custm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CustomerProfile");
            }
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", custm.Team_ID);
            return View(custm);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Login");
        }




        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name");
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(Customer cust)
        {
            string fileName = Path.GetFileNameWithoutExtension(cust.CustomerImageFile.FileName);
            string extension = Path.GetExtension(cust.CustomerImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            cust.Customer_Image = "~/CustomerImage/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/CustomerImage/"), fileName);
            cust.CustomerImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                ViewBag.Success = "successfully registered";
            }
            else
            {
                ViewBag.Failed = "Registration failed! Something went wrong! Please try again";
            }
            ViewBag.Team_ID = new SelectList(db.Teams, "Team_ID", "Team_Name", cust.Team_ID);
            ModelState.Clear();
            return View();
        }



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(TempCustomer tempCustomer)
        {
            if (ModelState.IsValid)
            {
                var customer = db.Customers.Where(c => c.Customer_Name.Equals(tempCustomer.Customer_Name)
                        && c.Customer_Email.Equals(tempCustomer.Customer_Email)
                        && c.Customer_Password.Equals(tempCustomer.Customer_Password)).FirstOrDefault();

                if (customer != null)
                {
                    FormsAuthentication.SetAuthCookie(tempCustomer.Customer_Name, false);
                    Session["CustomerEmail"] = customer.Customer_Email;
                    Session["type"] = "Customer";
                    return RedirectToAction("CustomerProfile");
                }
                else
                {
                    ViewBag.Failed = "Login Faild! Please try again";
                    return View();
                }
            }
            return View();
        }



        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Events");

        }



        [HttpGet]
        public ActionResult CustomerProfile()
        {
            String email = Convert.ToString(Session["CustomerEmail"]);
            var customer = db.Customers.Where(u => u.Customer_Email.Equals(email)).FirstOrDefault();
            return View(customer);
        }



        public ActionResult CustomerList()
        {
            var customers = db.Customers.Include(c => c.Team);
            return View(customers.ToList());
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
