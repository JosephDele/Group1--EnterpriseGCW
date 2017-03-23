using EC_SOLUTION.Models.EC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult AddStudent()
        {
            ViewBag.FacultyId = new SelectList((new ECDB_EWDFINALEntities().FACULTies.ToList()), "FacultyId", "Faculty_Name");
            return View();
        }
        private string generateID()
        {
            Random random = new Random();
            
            string id = "GT";
            for(int a = 0; a < 3; a++)
            {
                id += random.Next(9);
            }
            return id;
        }
            [HttpPost]
        public ActionResult AddStudent(STUDENT student)
        {
            if (ModelState.IsValid)
            {
                if (student.Email.Contains("@") && student.Email.Contains("."))
                {
                    using (ECDB_EWDFINALEntities ec = new ECDB_EWDFINALEntities())
                    {

                        student.StudentId = generateID();
                        student.Username = student.First_Name + student.StudentId;
                        student.Password = "12345";
                        MMS.UpdateStudent(student);
                        MMS.sendEmail("Student Username and Password", "Username : " + student.Username + "\nPassword : " + student.Password, student.Email);
                        ec.STUDENTs.Add(student);
                        ViewBag.fid = student.FacultyId;
                        ViewBag.FacultyId = new SelectList((new ECDB_EWDFINALEntities().FACULTies.ToList()), "FacultyId", "Faculty_Name");
                        ec.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.Clear();
                return View();
            }
            return View();
        }
        public ActionResult Home()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");
            else
            {
                Login login = (Login)Session["loggedUser"];
                if (login.password == "12345" && login.role.ToLower() == "student")
                {
                    return Redirect("ResetPassword");
                }
            }
            return View();
        }

        public ActionResult ResetPassword()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");

            Login login = (Login)Session["loggedUser"];
            return View(login);
        }

        [HttpPost]
        public ActionResult ResetPassword(Login login)
        {
            if (ModelState.IsValid)
            {
                STUDENT student = (new ECDB_EWDFINALEntities().STUDENTs).ToList().Single(s => s.StudentId == login.userid);
                student.Password = login.password;
                MMS.UpdateStudent(student);
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult Index()
        {
            if (Session["loggedUser"] == null)
                return RedirectToAction("Index", "Login");
            else
            {
                Login login = (Login)Session["loggedUser"];
                if (login.role != "ADMINISTRATOR")
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            return View((new ECDB_EWDFINALEntities().STUDENTs).ToList());
        }
        public ActionResult Generate(string id)
        {
            STUDENT student = (new ECDB_EWDFINALEntities().STUDENTs).ToList().Single(s => s.StudentId == id);
            student.Username = student.First_Name + student.StudentId;
            student.Password = "12345";
            MMS.UpdateStudent(student);
            MMS.sendEmail("Student Username and Password", "Username : " + student.Username + "\nPassword : " + student.Password, student.Email);
            return View(student);
        }
    }
}