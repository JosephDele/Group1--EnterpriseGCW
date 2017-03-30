using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EC_SOLUTION;

namespace EC_SOLUTION.Controllers
{
    public class ACEDEMIC_YEARController : Controller
    {
        private ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();

        // GET: ACEDEMIC_YEAR
        public ActionResult Index()
        {
            return View(db.ACEDEMIC_YEAR.ToList());
        }

        // GET: ACEDEMIC_YEAR/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACEDEMIC_YEAR aCEDEMIC_YEAR = db.ACEDEMIC_YEAR.Find(id);
            if (aCEDEMIC_YEAR == null)
            {
                return HttpNotFound();
            }
            return View(aCEDEMIC_YEAR);
        }

        // GET: ACEDEMIC_YEAR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ACEDEMIC_YEAR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ACEDEMICYEARID,YEAR,STATUS,closuredate")] ACEDEMIC_YEAR aCEDEMIC_YEAR)
        {
            if (ModelState.IsValid)
            {

                db.ACEDEMIC_YEAR.Add(aCEDEMIC_YEAR);
                if (aCEDEMIC_YEAR.STATUS == true)
                {
                    foreach (ACEDEMIC_YEAR year in (db.ACEDEMIC_YEAR.ToList()))
                    {
                        if (year.STATUS == true)
                        {
                            year.STATUS = false;

                        }
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(aCEDEMIC_YEAR);
        }

        // GET: ACEDEMIC_YEAR/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACEDEMIC_YEAR aCEDEMIC_YEAR = db.ACEDEMIC_YEAR.Find(id);
            if (aCEDEMIC_YEAR == null)
            {
                return HttpNotFound();
            }
            return View(aCEDEMIC_YEAR);
        }

        // POST: ACEDEMIC_YEAR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ACEDEMICYEARID,YEAR,STATUS,closuredate")] ACEDEMIC_YEAR aCEDEMIC_YEAR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aCEDEMIC_YEAR).State = EntityState.Modified;
                if (aCEDEMIC_YEAR.STATUS == true)
                {
                    foreach (ACEDEMIC_YEAR year in (db.ACEDEMIC_YEAR.ToList().Where(AY => AY.ACEDEMICYEARID != aCEDEMIC_YEAR.ACEDEMICYEARID)))
                    {
                        if (year.STATUS == true)
                        {
                            year.STATUS = false;
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aCEDEMIC_YEAR);
        }

        // GET: ACEDEMIC_YEAR/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACEDEMIC_YEAR aCEDEMIC_YEAR = db.ACEDEMIC_YEAR.Find(id);
            if (aCEDEMIC_YEAR == null)
            {
                return HttpNotFound();
            }
            return View(aCEDEMIC_YEAR);
        }

        // POST: ACEDEMIC_YEAR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ACEDEMIC_YEAR aCEDEMIC_YEAR = db.ACEDEMIC_YEAR.Find(id);
            db.ACEDEMIC_YEAR.Remove(aCEDEMIC_YEAR);
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
