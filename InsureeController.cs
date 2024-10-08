using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Car_Insurance9.Models;

namespace Car_Insurance9.Controllers
{
    public class InsureeController : Controller
    {
        private Insurance9Entities db = new Insurance9Entities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }
        //Add an Admin View for a site administrator to the Insuree Views. This page must show all quotes issued, along with the user's first name, last name, and email address.
        [HttpPost]
        public ActionResult SignUp(String FirtName, string LastName, string EmailAddress)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(EmailAddress))
            {
                return View(" /Views/Shared/Error.cshtml");
            }
            else
            {
                using (NewsletterEntities db = new NewsletterEntities())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;

                    db.SignUps.Add(signup);
                    db.SaveChanges();

                }
            }
        }
        public ActionResult Admin()
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signups = db.SignUps;
                var signupsVm = new List<SignuoVm>();
                foreach (var signup in signups)
                {
                    var signupVm = new SignupVm();
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;
                    signupVms.Add(signupVm);
                }
                return View(signupVms);
            }
        }
        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeeedingTickets,CoverageType,Qoute")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Qoute = CalculateQoute(insuree);
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }
        public decimal CalculateQoute(Insuree insuree)
        {
            // Start with a base of $50 / month.
            insuree.Qoute = 50;
            //If the user is 18 or under, add $100 to the monthly total.
            if (DateTime.Now.Year - insuree.DateOfBirth.Year < 18)
            {
                insuree.Qoute += 100;
            }
            //If the user is from 19 to 25, add $50 to the monthly total.
            if (DateTime.Now.Year - insuree.DateOfBirth.Year > 18 && DateTime.Now.Year - insuree.DateOfBirth.Year < 25)
            {
                insuree.Qoute += 50;
            }
            //if the user is 26 or older, add $25 to the monthly total. Double check your code to ensure all ages are covered.
            if (DateTime.Now.Year - insuree.DateOfBirth.Year > 100)
            {
                insuree.Qoute += 25;
            } 
        }
        public decimal CalculateCarYear(Insuree insuree)
        {
            insuree.CarYear = 50;
            //If the car's year is before 2000, add $25 to the monthly total.
            if (DateTime.Now.Year - insuree.CarYear < 2000)
            {
                insuree.CarYear += 25;
            }
            //if the car's year is after 2015, add $25 to the monthly total.
            if (DateTime.Now.Year - insuree.CarYear > 2000 && DateTime.Now.Year - insuree.CarYear < 2015)
            {
                insuree.CarYear += 25;
            }
        }
        public decimal CalculateCarMake(Insuree insuree)
        {
            //if the car's Make is a Porsche, add $25 to the price.
            if (insuree.CarMake == "Porsche")
            {
                insuree.CarMake += 25;
            }
            //If the car's Make is a Porsche and its model is a 911 Carrera, add an additional $25 to the price. (Meaning, this specific car will add a total of $50 to the price.)
            if (insuree.CarMake == "Porsche" && insuree.CarMake == "911 Carrera")
            {
                insuree.CarMake += 25;
            }
        }
        //Add $10 to the monthly total for every speeding ticket the user has.
        public decimal CalculateSpeeedingTickets(Insuree insuree)
        {
            insuree.Qoute += 10;
        }
        //if the user has ever had a DUI, add 25% to the total.
        public decimal CalculateDUI(Insuree insuree)
        {
            insuree.Qoute += 25;
        }
        //if it's full coverage, add 50% to the total.
        public decimal CalculateCoverageType(Insuree insuree)
        {
            insuree.Qoute += 50;
        }
        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeeedingTickets,CoverageType,Qoute")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
