using EC_SOLUTION.Models.EC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class CoordinatorController : Controller
    {
        private ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();

        // GET: Coordinator
        public ActionResult Index(string ClaimStatus = "")
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            List<string> status = new List<string>();
            status.Add("Pending");
            status.Add("Accepted");
            status.Add("Rejected");

            ViewBag.ClaimStatus = new SelectList(status);

            string coordinatorid = ((Login)Session["loggedUser"]).userid;
            string facultyid = (db.EC_COORDINATOR.ToList().Single(coord => coord.EC_CoodId == coordinatorid)).FacultyId;

            if (ClaimStatus != "")
            {
                return View(db.EC_CLAIMS.ToList().Where(claim => claim.STUDENT.FacultyId == facultyid && claim.STATUS == (ClaimStatus)));
            }
            return View(db.EC_CLAIMS.Where(claim => claim.STUDENT.FacultyId == facultyid));
        }

        // GET: Coordinator/Details/5
        public ActionResult Details(int id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.EC_CLAIMS.ToList().Single(claim => claim.ClaimId == id));
        }

        public ActionResult Accepted(int id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            EC_CLAIMS claim = db.EC_CLAIMS.Find(id);
            if (claim == null)
                return HttpNotFound();
            else
            {
                claim.STATUS = "Accepted";
                db.SaveChanges();

                MMS.sendEmail("Claim Accepted", "Congratulation \nYour Claim has been Accepted \nby your Faculty coordinator\n",
                   (db.STUDENTs.ToList().First(std => std.StudentId == claim.StudentId)).Email);
            }

            ViewBag.SuccessMessage = "Claim have been successfully Accepted";
            return View("Details", (db.EC_CLAIMS.Find(id)));

        }
        public ActionResult Rejected(int id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            EC_CLAIMS claim = db.EC_CLAIMS.Find(id);

            if (claim == null)
                return HttpNotFound();
            else
            {
                claim.STATUS = "Rejected";
                db.SaveChanges();

                MMS.sendEmail("Claim Rejected", "Your Claim has been rejected \nby your Faculty coordinator\n",
                    (db.STUDENTs.ToList().First(std => std.StudentId == claim.StudentId)).Email);
            }

            ViewBag.SuccessMessage = "Claim have been successfully rejected";
            return View("Details", (db.EC_CLAIMS.Find(id)));
        }

    }
}
