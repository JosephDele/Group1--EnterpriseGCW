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
            ViewBag.ClaimStatus = new SelectList((new ECDB_EWDFINALEntities().ClaimStatus.ToList()), "ClaimStatusID", "Status");

            string coordinatorid = ((Login)Session["loggedUser"]).userid;
            string facultyid = (db.EC_COORDINATOR.ToList().Single(coord => coord.EC_CoodId == coordinatorid)).FacultyId;

            if (ClaimStatus != "")
            {
                return View(db.EC_CLAIMS.ToList().Where(claim => claim.FacultyId == facultyid && claim.ClaimStatusID == int.Parse(ClaimStatus)));
            }
            return View(db.EC_CLAIMS.Where(claim => claim.FacultyId == facultyid));
        }

        // GET: Coordinator/Details/5
        public ActionResult Details(int id)
        {
            return View(db.EC_CLAIMS.ToList().Single(claim => claim.ClaimId == id));
        }

        public ActionResult Accepted(int id)
        {
            EC_CLAIMS claim = db.EC_CLAIMS.Find(id);
            if (claim == null)
                return HttpNotFound();
            else
            {
                claim.ClaimStatusID = db.ClaimStatus.ToList().Single(status => status.Status.ToLower() == "accepted").ClaimStatusID;
                db.SaveChanges();

                MMS.sendEmail("Claim Accepted", "Congratulation \nYour Claim has been Accepted \nby your Faculty coordinator\n",
                   (db.STUDENTs.ToList().First(std => std.StudentId == claim.StudentId)).Email);
            }
            string coordinatorid = ((Login)Session["loggedUser"]).userid;
            string facultyid = (db.EC_COORDINATOR.ToList().Single(coord => coord.EC_CoodId == coordinatorid)).FacultyId;
            ViewBag.ClaimStatus = new SelectList((new ECDB_EWDFINALEntities().ClaimStatus.ToList()), "ClaimStatusID", "Status");

            return View("Index", (db.EC_CLAIMS.Where(c => c.FacultyId == facultyid)));
        }
        public ActionResult Rejected(int id)
        {
            EC_CLAIMS claim = db.EC_CLAIMS.Find(id);

            if (claim == null)
                return HttpNotFound();
            else
            {
                claim.ClaimStatusID = db.ClaimStatus.ToList().Single(status => status.Status.ToLower() == "rejected").ClaimStatusID;
                db.SaveChanges();

                MMS.sendEmail("Claim Rejected", "Your Claim has been rejected \nby your Faculty coordinator\n",
                    (db.STUDENTs.ToList().First(std => std.StudentId == claim.StudentId)).Email);
            }
            string coordinatorid = ((Login)Session["loggedUser"]).userid;
            string facultyid = (db.EC_COORDINATOR.ToList().Single(coord => coord.EC_CoodId == coordinatorid)).FacultyId;
            ViewBag.ClaimStatus = new SelectList((new ECDB_EWDFINALEntities().ClaimStatus.ToList()), "ClaimStatusID", "Status");

            return View("Index", (db.EC_CLAIMS.Where(c => c.FacultyId == facultyid)));
        }

    }
}
