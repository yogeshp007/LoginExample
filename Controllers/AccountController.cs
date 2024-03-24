using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using LoginExample.Models;

namespace LoginExample.Controllers
{
    public class AccountController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        // GET: Account
        public ActionResult Login()
        {
            ViewBag.error = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string uemail = fc["uemail"];
            string upass = fc["upass"];

            if (!string.IsNullOrWhiteSpace(uemail)
                && !string.IsNullOrWhiteSpace(upass))
            {
                tblUser user = db.tblUsers.Where(x => x.uemail == uemail && x.upass == upass).FirstOrDefault();
                if (user != null)
                {
                    Session["userid"] = user.userId;
                    Session["uemail"] = user.uemail;
                    Session["uname"] = user.uname;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "please enter correct email and password";
                }
            }
            else
            {
                ViewBag.error = "please enter proper email and password";
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("jamilmoughal786@gmail.com", "Jamil");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Your Email Password here";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}