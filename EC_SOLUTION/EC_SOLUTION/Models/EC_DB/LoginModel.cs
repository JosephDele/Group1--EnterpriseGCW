using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EC_SOLUTION.Models.EC_DB
{
    public class LoginModel
    {
        ECDB_EWDFINALEntities testEC = new ECDB_EWDFINALEntities();
        public static Login AuthenticateUser(string username, string password)
        {
            foreach (Login login in (new LoginModel()).getAllLogin())
            {
                if (login.username == username && login.password == password)
                {
                    return login;
                }
            }
            return null;
        }

        public List<Login> getAllLogin()
        {
            List<Login> Alllogin = new List<Login>();
            foreach (EC_COORDINATOR coord in testEC.EC_COORDINATOR.ToList())
            {
                Login login = new Login();
                login.username = coord.Username;
                login.password = coord.Password;
                login.role = "COORDINATOR";
                login.userid = coord.EC_CoodId;
                Alllogin.Add(login);
            }

            foreach (EC_ADMINISTRATOR admin in testEC.EC_ADMINISTRATOR.ToList())
            {
                Login login = new Login();
                login.username = admin.Username;
                login.password = admin.Password;
                login.role = "ADMINISTRATOR";
                login.userid = admin.AdminId;
                Alllogin.Add(login);

            }

            foreach (EC_MANAGER manager in testEC.EC_MANAGER.ToList())
            {
                Login login = new Login();
                login.username = manager.Username;
                login.password = manager.Password;
                login.role = "MANAGER";
                login.userid = manager.ManagerId;
                Alllogin.Add(login);

            }

            foreach (STUDENT stu in testEC.STUDENTs.ToList())
            {
                Login login = new Login();
                login.username = stu.Username;
                login.password = stu.Password;
                login.role = "STUDENT";
                login.userid = stu.StudentId;
                Alllogin.Add(login);

            }
            return Alllogin;
        }
    }
}