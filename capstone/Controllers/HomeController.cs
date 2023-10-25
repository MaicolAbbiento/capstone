using capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace capstone.Controllers
{
    public class HomeController : Controller
    {
        private Model1 model1 = new Model1();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signUp(utenti u)
        {
            if (ModelState.IsValid)
            {
                u.role = "utente";
                u.accauntVerificato = false;

                utenti ut = model1.utenti.FirstOrDefault((e) => e.username == u.username);
                if (ut == null)
                {
                    if (u.password == u.confermapassword)
                    {
                        model1.utenti.Add(u);
                        model1.SaveChanges();
                        TempData["Successo"] = "ti sei regitrato corettamente";
                        return RedirectToAction("login");
                    }
                    ViewBag.password = "le password non coincidono";
                }
                else
                {
                    ViewBag.errore = "utente gia registato";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login([Bind(Include = "username, password")] utenti u)
        {
            utenti ut = model1.utenti.FirstOrDefault((e) => e.username == u.username && e.password == u.password);
            if (ut != null)
            {
                FormsAuthentication.SetAuthCookie(u.username, true);
                return RedirectToAction("Index");
            }
            ViewBag.utente = "username o password eratti";
            return View();
        }
    }
}