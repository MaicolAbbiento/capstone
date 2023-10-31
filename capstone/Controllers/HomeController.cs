using capstone.Migrations;
using capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                FormsAuthentication.SetAuthCookie(u.username, false);
                return RedirectToAction("Index");
            }
            ViewBag.utente = "username o password eratti";
            return View();
        }

        [HttpGet]
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }

        [HttpGet]
        public ActionResult dettagli(int? id)
        {
            prodotti p = model1.prodotti.Find(id);
            if (p == null)
            {
                return RedirectToAction("Index");
            }
            List<carello> c = model1.carello.Where((e) => e.idprodotti == id).ToList();
            List<vendita> v = new List<vendita>();
            foreach (var item in c)
            {
                if (item.vendita.utenti.username == User.Identity.Name)
                {
                    v.Add(item.vendita);
                }
            }
            p.vendita = v;
            if (p.recensioni.Count > 0)
            {
                foreach (var item in p.recensioni)
                {
                    if (item.utenti.username == User.Identity.Name)
                    {
                        ViewBag.recesione = 1;
                    };
                }
            }
            p.recensioni = model1.recensioni.Where((e) => e.idprodotti == id).ToList();
            p.recensioni.Reverse();
            return View(p);
        }

        [HttpPost]
        public JsonResult dettaglijs(int imp, int idn)
        {
            prodotti p = model1.prodotti.Find(idn);
            if (p.prodottiinmagazzino < imp)
            {
                string err = "prodotti insufficanti solo " + p.prodottiinmagazzino + " presenti";
                return Json(err);
            }
            else
            {
                p.prodottiinmagazzino -= imp;
                model1.Entry(p).State = EntityState.Modified;
                vendita v = model1.vendita.FirstOrDefault((e) => e.utenti.username == User.Identity.Name && e.stato == "in carello");
                if (v != null)
                {
                    carello c = new carello();
                    c.idprodotti = idn;
                    c.idvendita = v.idvendita;
                    int n = 0;
                    foreach (carello e in v.carello)
                    {
                        if (e.idprodotti == idn)
                        {
                            n++;
                            c.quantita = e.quantita + imp;
                            c.idcarello = e.idcarello;
                            Model1 db = new Model1();
                            db.Entry(c).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (n == 0)
                    {
                        c.quantita = imp;
                        model1.carello.Add(c);
                    }
                    model1.SaveChanges();
                }
                else
                {
                    vendita v1 = new vendita();
                    v1.stato = "in carello";
                    utenti U = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
                    v1.idUtenti = U.idUtenti;
                    model1.vendita.Add(v1);
                    model1.SaveChanges();
                    vendita vendita = model1.vendita.OrderByDescending(e => e.idvendita).FirstOrDefault();
                    carello c = new carello();
                    if (vendita != null)
                    {
                        c.idvendita = vendita.idvendita;
                    }

                    c.idprodotti = idn;

                    c.quantita = imp;
                    model1.carello.Add(c);
                    model1.SaveChanges();
                }
            }
            string success = "ok";
            return Json(success);
        }

        [HttpPost]
        public JsonResult comment(string textarea, int valutazione, int id)
        {
            recensioni recensioni = new recensioni();
            if (valutazione > 0)
            {
                utenti U = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
                if (U != null)
                {
                    recensioni r = model1.recensioni.FirstOrDefault((e) => e.idprodotti == id && e.idUtenti == U.idUtenti);
                    recensioni.idUtenti = U.idUtenti;
                    recensioni.idprodotti = id;
                    recensioni.valutazione = valutazione;
                    recensioni.descrizione = textarea;
                    model1.recensioni.Add(recensioni);
                    model1.SaveChanges();
                }
            }
            else
            {
                valutazione = 0;
                return Json(valutazione);
            }
            return Json(recensioni);
        }

        [HttpGet]
        public ActionResult modificarecensione(int id)
        {
            recensioni r = model1.recensioni.FirstOrDefault((e) => e.idrecensioni == id && e.utenti.username == User.Identity.Name);
            if (r != null)
            {
                return View(r);
            }
            else
            {
                string link = "dettagli/" + id;
                return RedirectToAction(link);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult modificarecensione(recensioni recensioni)
        {
            if (ModelState.IsValid)
            {
                if (recensioni.valutazione <= 5 && recensioni.valutazione >= 1)
                {
                    model1.Entry(recensioni).State = EntityState.Modified;
                    model1.SaveChanges();
                    ViewBag.modifica = "modifica avvenuta con sucesso";
                }
            }
            return View();
        }
    }
}