using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace EC_SOLUTION.Models.EC_DB
{
    public class MMS
    {
        public static void sendEmail(string subject, string content, string Receivermail, bool allowHtml = false)
        {
            string fromaddr = "bigabam19@gmail.com";
            string password = "abam1997";

            MailMessage msg = new MailMessage();
            msg.Subject = subject;
            msg.From = new MailAddress(fromaddr);
            msg.Body = content;
            msg.To.Add(new MailAddress(Receivermail));
            msg.IsBodyHtml = allowHtml;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(fromaddr, password);
            smtp.Credentials = nc;
            smtp.Send(msg);

        }
        public static bool AddStudent(STUDENT newSt)
        {
            try
            {
                ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();
                db.STUDENTs.Add(newSt);
                return db.SaveChanges() == 1;
            }
            catch
            {
                return false;
            }

        }
        public static bool UpdateStudent(STUDENT newSt)
        {
            try
            {
                ECDB_EWDFINALEntities db = new ECDB_EWDFINALEntities();
                STUDENT oldSt = db.STUDENTs.Find(newSt.StudentId);
                oldSt.Username = newSt.Username;
                oldSt.Password = newSt.Password;
                return db.SaveChanges() == 1;
            }
            catch
            {
                return false;
            }
        }
    }
}