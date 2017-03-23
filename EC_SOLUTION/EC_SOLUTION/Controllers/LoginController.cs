using EC_SOLUTION.Models.EC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EC_SOLUTION.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login collection)
        {
            ViewBag.ErrorMessage = collection.username + " <br> " + collection.password;
            if (string.IsNullOrEmpty(collection.username) || string.IsNullOrEmpty(collection.password))
            {
                ViewBag.ErrorMessage = "Username or Password cannot be null";
                return View();
            }
            Login login = LoginModel.AuthenticateUser(collection.username.Trim(), collection.password);
            if (login == null)
            {
                ViewBag.ErrorMessage = "Incorrect User Credential";
            }
            else
            {
                ViewBag.ErrorMessage = "Correct user";
                Session["loggedUser"] = login;
                //Response.Redirect("Info");
                if (login.role.ToLower() == "student" && (new ECDB_EWDFINALEntities()).STUDENTs.Single(l => l.StudentId == login.userid).Password == "12345")
                {
                    return RedirectToAction("ResetPassword", "Student");
                }
                else if (login.role.ToLower() == "student" && (new ECDB_EWDFINALEntities()).STUDENTs.Single(l => l.StudentId == login.userid).Password != "12345")
                {
                    return RedirectToAction("Index", "EC_CLAIMS");
                }
                else if (login.role == "ADMINISTRATOR")
                {
                    return RedirectToAction("Index", "Student");
                }
                else if (login.role == "COORDINATOR")
                {
                    return RedirectToAction("Index", "Coordinator");
                }
                else
                {
                    return RedirectToAction("Claims", "Manager");
                }

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["loggedUser"] = null;
            return View("Index");
        }

        public ActionResult Info()
        {
            Login login = (Login)Session["loggedUser"];
            return View(login);
        }
        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
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