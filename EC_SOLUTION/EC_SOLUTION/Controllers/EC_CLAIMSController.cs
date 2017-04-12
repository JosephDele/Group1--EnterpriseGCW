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
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            var eC_CLAIMS = db.EC_CLAIMS.Include(e => e.ACEDEMIC_YEAR).Include(e => e.STUDENT.FACULTY).Include(e => e.STUDENT);
            string sid = ((Login)Session["loggedUser"]).userid;
            return View(eC_CLAIMS.ToList().Where(ec => ec.StudentId == sid).OrderBy(claims => claims.Date).Reverse());
        }

        // GET: EC_CLAIMS/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

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
        public ActionResult CreateStep1()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            string stdid = ((Login)Session["loggedUser"]).userid;
            string facid = (db.STUDENTs.SingleOrDefault(st => st.StudentId == stdid)).FacultyId;
            ViewBag.assesment = new SelectList((db.ASSESSEMENTs.Where(ass => ass.FACULTYID == facid)).ToList(), "ASSID", "ASSNAME");
            //ViewBag.academicyear = new SelectList((), "ACEDEMICYEARID", "YEAR");
            return View();
        }
        public ActionResult CreateStep2(int assesment)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Itemid = new SelectList((db.ASSESSEMENTs.SingleOrDefault(ass => ass.ASSID == assesment).ITEMs.ToList()), "ITEMID", "ITEM_NAME");
            return View();
        }

        private int generateNum(int num)
        {
            return (new Random()).Next(158953);
        }

        private bool isDuplicated(string sid, string desc)
        {
            if (string.IsNullOrWhiteSpace(desc))
                return false;
            foreach (EC_CLAIMS ec in (db.EC_CLAIMS).ToList().Where(ec => ec.STUDENT.StudentId == sid))
            {
                if (string.IsNullOrWhiteSpace(ec.Description))
                {
                    continue;
                }
                if (ec.Description.ToLower().Trim() == desc.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStep2([Bind(Include = "ClaimId,StudentId,Description,Date,ACEDEMICYEARID,FacultyId")] EC_CLAIMS eC_CLAIMS, int assesment, int Itemid)
        {
            ViewBag.Itemid = new SelectList((db.ASSESSEMENTs.SingleOrDefault(ass => ass.ASSID == assesment).ITEMs.ToList()), "ITEMID", "ITEM_NAME");
            if (this.isDuplicated(((Login)Session["loggedUser"]).userid, eC_CLAIMS.Description))
            {
                ViewBag.DuplicateMessage = "You have a claim with same description";
                return View();
            }
            if (ModelState.IsValid)
            {
                if (((Login)Session["loggedUser"]).role != "STUDENT")
                    return RedirectToAction("Index", "Login");
                eC_CLAIMS.ClaimId = generateNum((new Random()).Next(9));
                eC_CLAIMS.ACEDEMICYEARID = (db.ACEDEMIC_YEAR.ToList().Where(year => year.STATUS.Value == true).ElementAt(0)).ACEDEMICYEARID;
                eC_CLAIMS.StudentId = ((Login)Session["loggedUser"]).userid;
                eC_CLAIMS.Itemid = Itemid;
                eC_CLAIMS.Date = DateTime.Parse(DateTime.Now.ToString());
                eC_CLAIMS.STATUS = "Pending";
                db.EC_CLAIMS.Add(eC_CLAIMS);
                db.SaveChanges();
                // int numberofCoordinFac = (db.EC_COORDINATOR.ToList().Where(c => c.FacultyId == eC_CLAIMS.FacultyId)).Count();
                string studentid = (((Login)Session["loggedUser"]).userid);
                string facid = (db.STUDENTs.Single(s => s.StudentId == studentid).FacultyId);
                EC_COORDINATOR coord = (db.EC_COORDINATOR.ToList().Where(c => c.FacultyId == facid)).ToList().ElementAt(0);
                if (coord != null)
                {
                    STUDENT student = db.STUDENTs.Find(eC_CLAIMS.StudentId);
                    string content = "Student with id " + eC_CLAIMS.StudentId + " has request a claim \nwhich decision should be made within 14 days" +
                        "\nStudent Name : " + student.First_Name + " " + student.Last_Name;
                    MMS.sendEmail("Claim Request to your Faculty", content, coord.Email);
                }
                ModelState.Clear();
                ViewBag.SuccessMessage = "Claim Has been successfully added";
                ViewBag.Itemid = new SelectList((db.ASSESSEMENTs.SingleOrDefault(ass => ass.ASSID == assesment).ITEMs.ToList()), "ITEMID", "ITEM_NAME");

            }
            return View();

        }

        // GET: EC_CLAIMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

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
