using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EC_SOLUTION;
using EC_SOLUTION.Models.EC_DB;

namespace EC_SOLUTION.Controllers
{
    public class EC_CLAIMSController : Controller
    {
        private ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();

        // GET: EC_CLAIMS
        public ActionResult Index()
        {
            var eC_CLAIMS = db.EC_CLAIMS.Include(e => e.ACEDEMIC_YEAR).Include(e => e.FACULTY).Include(e => e.STUDENT);
            string sid = ((Login)Session["loggedUser"]).userid;
            return View(eC_CLAIMS.ToList().Where(ec => ec.StudentId == sid));
        }

        // GET: EC_CLAIMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EC_CLAIMS eC_CLAIMS = db.EC_CLAIMS.Find(id);
            if (eC_CLAIMS == null)
            {
                return HttpNotFound();
            }
            return View(eC_CLAIMS);
        }

        // GET: EC_CLAIMS/Create
        public ActionResult Create()
        {

            return View();
        }

        private int generateNum(int num)
        {
            return (new Random()).Next(158953);
        }

        // POST: EC_CLAIMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimId,StudentId,Description,Date,ACEDEMICYEARID,FacultyId")] EC_CLAIMS eC_CLAIMS)
        {
            if (ModelState.IsValid)
            {
                if (((Login)Session["loggedUser"]).role != "STUDENT")
                    return RedirectToAction("Index", "Login");
                eC_CLAIMS.ClaimId = generateNum((new Random()).Next(9));
                eC_CLAIMS.ACEDEMICYEARID = (new ECDB_EWDFINALEntities()).ACEDEMIC_YEAR.Single(a => a.STATUS == true).ACEDEMICYEARID;
                eC_CLAIMS.StudentId = ((Login)Session["loggedUser"]).userid;
                eC_CLAIMS.FacultyId = (new ECDB_EWDFINALEntities().STUDENTs.Single(s => s.StudentId == eC_CLAIMS.StudentId)).FacultyId;
                eC_CLAIMS.Date = DateTime.Parse(DateTime.Now.ToString());
                eC_CLAIMS.ClaimStatusID = 1;
                db.EC_CLAIMS.Add(eC_CLAIMS);
                db.SaveChanges();
                // int numberofCoordinFac = (db.EC_COORDINATOR.ToList().Where(c => c.FacultyId == eC_CLAIMS.FacultyId)).Count();
                EC_COORDINATOR coord = (db.EC_COORDINATOR.ToList().Where(c => c.FacultyId == eC_CLAIMS.FacultyId)).ToList().ElementAt(0);
                if (coord != null)
                {
                    STUDENT student = db.STUDENTs.Find(eC_CLAIMS.StudentId);
                    string content = "Student with id " + eC_CLAIMS.StudentId + " has request a claim \nwhich decision should be made within 14 days" +
                        "\nStudent Name : " + student.First_Name + " " + student.Last_Name;
                    MMS.sendEmail("Claim Request to your Faculty", content, coord.Email);
                }

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        // GET: EC_CLAIMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EC_CLAIMS eC_CLAIMS = db.EC_CLAIMS.Find(id);
            if (eC_CLAIMS == null)
            {
                return HttpNotFound();
            }


            return View(eC_CLAIMS);
        }

        // POST: EC_CLAIMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimId,StudentId,Description,Date,ACEDEMICYEARID,FacultyId")] EC_CLAIMS eC_CLAIMS)
        {
            if (ModelState.IsValid)
            {
                EC_CLAIMS claim = db.EC_CLAIMS.Find(eC_CLAIMS.ClaimId);
                claim.Description = eC_CLAIMS.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eC_CLAIMS);
        }

        // GET: EC_CLAIMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EC_CLAIMS eC_CLAIMS = db.EC_CLAIMS.Find(id);
            if (eC_CLAIMS == null)
            {
                return HttpNotFound();
            }
            return View(eC_CLAIMS);
        }

        // POST: EC_CLAIMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EC_CLAIMS eC_CLAIMS = db.EC_CLAIMS.Find(id);
            db.EC_CLAIMS.Remove(eC_CLAIMS);
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
