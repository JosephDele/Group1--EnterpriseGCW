using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class ManagerController : Controller
    {
        ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();
        public ActionResult Statistics()
        {
            return View(db.EC_CLAIMS.ToList());
        }

        public ActionResult ExceptionReports()
        {
            return View(db.EC_CLAIMS.ToList().Where(c => c.Uploads.Count() == 0));
        }

        public ActionResult Claims()
        {
            return View((new ECDB_EWDFINALEntities().EC_CLAIMS.ToList()));
        }
    }
}
