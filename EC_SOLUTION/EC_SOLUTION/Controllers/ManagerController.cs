using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class ManagerController : Controller
    {
        ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();
        public ActionResult Statistics()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.EC_CLAIMS.ToList());
        }

        public ActionResult ExceptionReports(string ClaimStatus = "")
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            List<string> status = new List<string>();
            status.Add("All");
            status.Add("Pending");
            status.Add("Accepted");
            status.Add("Rejected");

            ViewBag.ClaimStatus = new SelectList(status);

            if (ClaimStatus == "" || ClaimStatus == "All")
            {
                return View(db.EC_CLAIMS.ToList());
            }
            else
            {
                return View(db.EC_CLAIMS.ToList().Where(c => c.STATUS == ClaimStatus));
            }
        }

        public ActionResult NumberOfClaimscharts(string type = "pie")
        {
            List<EC_CLAIMS> claims = db.EC_CLAIMS.ToList();
            string xvalues = "";
            string yvalues = "";

            foreach (FACULTY fac in db.FACULTies.ToList())
            {
                xvalues += fac.Faculty_Name + ",";
                yvalues += claims.Where(c => c.STUDENT.FACULTY.FacultyId == fac.FacultyId).Count() + ",";
            }

            xvalues = xvalues.Remove(xvalues.Length - 1, 1);
            yvalues = yvalues.Remove(yvalues.Length - 1, 1);

            var myChart = new Chart(width: 600, height: 400)
            .AddTitle("Number of Claim Pie Chart")
            .AddSeries(
                name: "Number of Claim This Week",
                chartType: type,
                xValue: xvalues.Split(','),
                yValues: yvalues.Split(',')).Write("png");

            return null;

        }


        public ActionResult PercentageOfClaimsByAcademicYear(string type = "pie")
        {
            List<EC_CLAIMS> claims = db.EC_CLAIMS.ToList();
            string xvalues = "";
            string yvalues = "";

            foreach (ACEDEMIC_YEAR year in db.ACEDEMIC_YEAR.ToList())
            {
                decimal percentage = (decimal.Divide(claims.Where(c => c.ACEDEMICYEARID == year.ACEDEMICYEARID).Count(), claims.Count())) * 100;
                xvalues += year.YEAR + " \n\n" + string.Format("{0:.#}", percentage) + "% ,";
                yvalues += percentage + ",";
            }

            xvalues = xvalues.Remove(xvalues.Length - 1, 1);
            yvalues = yvalues.Remove(yvalues.Length - 1, 1);

            var myChart = new Chart(width: 600, height: 400)
            .AddTitle("Percentage of Claims By Academic Year")
            .AddLegend("Percentage of Claims By Academic Year", "Percentage of Claims By Academic Year")
            .AddSeries(
                name: "Percentage of Claims By Academic Year",
                legend: "Percentage of Claims By Academic Year",
                chartType: type,
                xValue: xvalues.Split(','),
                yValues: yvalues.Split(',')).Write("png");
            return null;

        }

        public ActionResult linechart(string type = "line")
        {
            //ineach faculty how many claims are there in an acedemic year
            string xvalues = "";

            var chart = new System.Web.Helpers.Chart(width: 500, height: 300)
           .AddTitle("Statistics of Claim Per Faculty");
            int countt = 0;
            foreach (FACULTY fac in db.FACULTies.ToList())
            {
                try
                {
                    xvalues += db.ACEDEMIC_YEAR.ToList()[countt].YEAR + ",";
                }
                catch
                {
                    xvalues += " ,";
                }
                countt++;
            }
            xvalues = xvalues.Remove(xvalues.Length - 1, 1);
            int counter = 0;
            foreach (ACEDEMIC_YEAR year in db.ACEDEMIC_YEAR.ToList())
            {
                string yvalues = "";
                int count = 0;

                foreach (FACULTY fac in db.FACULTies.ToList())
                {
                    int claimcount = db.EC_CLAIMS.ToList().Where(c => c.ACEDEMICYEARID == year.ACEDEMICYEARID && c.STUDENT.FacultyId == fac.FacultyId).Count();
                    yvalues += claimcount + ",";
                    count++;
                }
                yvalues = yvalues.Remove(yvalues.Length - 1, 1);
                chart.AddSeries(
                    chartType: type,
                    xValue: xvalues.Split(','),
                    yValues: yvalues.Split(','),
                    name: db.FACULTies.ToList()[counter].Faculty_Name
                    );
                counter++;
            }
            chart.AddLegend();

            return File(chart.GetBytes("png"), "image/bytes");
        }

        public ActionResult ScheduleReport()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.EC_CLAIMS.ToList());
        }

        public ActionResult Claims()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            return View((new ECDB_EWDFINALEntities().EC_CLAIMS.ToList()));
        }
    }
}
