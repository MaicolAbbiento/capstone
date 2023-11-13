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
            List<prodotti> p = model1.prodotti.ToList();

            return View(p);
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
            p.recensioni = model1.recensioni.Where((e) => e.idprodotti == id).ToList();
            p.recensioni.Reverse();
            p.valutazione = 0;
            if (p.recensioni.Count > 0)
            {
                foreach (var item in p.recensioni)
                {
                    if (item.utenti.username == User.Identity.Name)
                    {
                        ViewBag.recesione = 1;
                    };
                    p.valutazione += item.valutazione;
                }
                decimal val = Convert.ToDecimal(p.valutazione);
                val /= p.recensioni.Count();
                val = Math.Round(val);
                p.valutazione = Convert.ToInt32(val);
            }
            model1.Entry(p).State = EntityState.Modified;
            model1.SaveChanges();
            aziende A = model1.aziende.Find(p.idaziende);
            imprenditori i = new imprenditori();
            foreach (imprenditori aziende in A.imprenditori)
            {
                i = model1.imprenditori.FirstOrDefault((e) => e.idimpreditori == aziende.idimpreditori && e.utenti.username == User.Identity.Name);
                if (i != null)
                {
                    ViewBag.modifica = true;
                }
            }

            return View(p);
        }

        [HttpPost]
        public JsonResult dettaglijs(int imp, int idn)
        {
            prodotti p = model1.prodotti.Find(idn);
            if (p.prodottiinmagazzino < imp)
            {
                string err = "";
                if (p.prodottiinmagazzino == 0)
                {
                    err = "prodotti insufficenti solo " + p.prodottiinmagazzino + " prodotto presente";
                }
                else
                {
                    err = "prodotti insufficenti solo " + p.prodottiinmagazzino + " prodotti presenti";
                }
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
        public JsonResult comment(string textarea, int valutazione, int id, string titolod)
        {
            recensioni recensioni = new recensioni();
            if (valutazione > 0)
            {
                utenti U = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
                if (U != null)
                {
                    recensioni r = model1.recensioni.FirstOrDefault((e) => e.idprodotti == id && e.idUtenti == U.idUtenti);
                    if (r == null)
                    {
                        recensioni.idUtenti = U.idUtenti;
                        recensioni.titolo = titolod;
                        recensioni.idprodotti = id;
                        recensioni.valutazione = valutazione;
                        recensioni.descrizione = textarea;
                        Model1 db = new Model1();
                        db.recensioni.Add(recensioni);
                        db.SaveChanges();
                    }
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
            Session["id"] = id;
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
            if (recensioni.valutazione == 5 || recensioni.valutazione == 1 || recensioni.valutazione == 2 || recensioni.valutazione == 3 || recensioni.valutazione == 4)
            {
                int id = (int)Session["id"];
                recensioni r = model1.recensioni.FirstOrDefault((e) => e.idrecensioni == id && e.utenti.username == User.Identity.Name);
                if (r != null)
                {
                    recensioni.idrecensioni = id;
                    recensioni.idprodotti = r.idprodotti;
                    recensioni.idUtenti = r.idUtenti;
                    Model1 db = new Model1();
                    db.Entry(recensioni).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.modifica = "modifica avvenuta con sucesso";
                }
            }
            else
            {
                ViewBag.errore = "inserire una valutazione da 1 a 5 ";
            }

            return View();
        }
    }
}