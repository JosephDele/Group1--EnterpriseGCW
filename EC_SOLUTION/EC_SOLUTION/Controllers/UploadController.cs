using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index(int id)
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }
        [HttpPost]
        public ActionResult Index(int id, HttpPostedFileBase file, Evidence upload)
        {
            if (ModelState.IsValid)
            {
                using (ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities())
                {
                    upload.ClaimId = id;
                    upload.FileName = "/ClamDoc/" + file.FileName.Split('\\')[file.FileName.Split('\\').Length - 1];
                    file.SaveAs(Server.MapPath("~/ClamDoc/") + file.FileName.Split('\\')[file.FileName.Split('\\').Length - 1]);
                    db.Evidences.Add(upload);
                    db.SaveChanges();
                    return RedirectToAction("Index", "EC_CLAIMS");
                }
            }
            return View();
        }

        // GET: Upload/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Upload/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
