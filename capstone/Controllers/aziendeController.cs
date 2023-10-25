using capstone.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace capstone.Controllers
{
    public class aziendeController : Controller
    {
        // GET: aziende
        private Model1 model1 = new Model1();

        public List<SelectListItem> Listacategoria
        {
            get
            {
                List<SelectListItem> list = new List<SelectListItem>();
                List<Categoria> lista = new List<Categoria>();
                lista = model1.Categoria.ToList();
                foreach (Categoria p in lista)
                {
                    SelectListItem item = new SelectListItem { Text = $"{p.categoria}", Value = $"{p.idcategoria}" };
                    list.Add(item);
                }
                return list;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult aggiungicollaboratore1()

        {
            if (User.IsInRole("utente"))
            {
                ViewBag.categoria = Listacategoria;
                return View();
            }
            else
            {
                return RedirectToAction("aggiungicollaboratore2");
            }
        }

        [HttpPost]
        public ActionResult aggiungicollaboratore1([Bind(Exclude = "dataAprovazione,verificata,inattesa")] aziende a)
        {
            if (ModelState.IsValid)
            {
                aziende aziende1 = model1.aziende.FirstOrDefault((e) => e.nomeazienda == a.nomeazienda || e.piva == a.piva);
                if (aziende1 == null)
                {
                    a.verificata = false;
                    a.inattesa = true;
                    a.idcategoria = Convert.ToInt32(a.categoria.categoria);
                    a.categoria = null;
                    model1.aziende.Add(a);
                    model1.SaveChanges();
                    List<aziende> aziende = model1.aziende.ToList();
                    aziende.Reverse();
                    imprenditori i = new imprenditori
                    {
                        idaziende = aziende[0].idaziende
                    };
                    utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
                    i.idUtenti = utenti.idUtenti;
                    model1.imprenditori.Add(i);

                    model1.SaveChanges();
                    return RedirectToAction("aggiungicollaboratore2");
                }
            }
            ViewBag.errore = "azienda gia registrata";
            ViewBag.categoria = Listacategoria;
            return View();
        }

        [HttpGet]
        public ActionResult modificaAzienda(int id)
        {
            if (!User.IsInRole("amministratore"))
            {
                utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
                if (utenti != null)
                {
                    imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                    if (i != null)
                    {
                        aziende a = model1.aziende.FirstOrDefault((e) => e.idaziende == id && e.idaziende == i.idaziende);
                        if (a != null)
                        {
                            ViewBag.categoria = Listacategoria;
                            TempData["id"] = id;
                            return View(a);
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                }
            }

            return RedirectToAction("aggiungicollaboratore2");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult modificaAzienda([Bind(Exclude = "dataAprovazione,verificata,inattesa")] aziende a)
        {
            a.idaziende = (int)TempData["id"];
            aziende aziende = model1.aziende.Find(a.idaziende);
            a.verificata = aziende.verificata;
            a.inattesa = aziende.inattesa;

            a.idcategoria = Convert.ToInt32(a.categoria.categoria);
            a.categoria = null;
            model1.Entry(a).State = EntityState.Modified;
            model1.SaveChanges();
            ViewBag.categoria = Listacategoria;
            ViewBag.MODIFICA = "prodotto modificato con successo";
            return View(a);
        }

        public ActionResult aggiungicollaboratore2()
        {
            List<aziende> a = new List<aziende>();
            if (User.IsInRole("amministratore"))
            {
                a = model1.aziende.Where((e) => e.verificata == false && e.inattesa == true).ToList();
                foreach (var item in a)
                {
                    item.categoria = model1.Categoria.Find(item.idcategoria);
                }
            }
            else
            {
                imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name && e.aziende.verificata == false);
                if (i == null)
                {
                    imprenditori im = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);

                    if (im == null)
                    {
                        return RedirectToAction("aggiungicollaboratore1");
                    }
                    else
                    {
                        TempData["azienda"] = User.Identity.Name + "benvenuto nel team";
                        return RedirectToAction("prodotti", "prodotti");
                    }
                }
                i.aziende.categoria = model1.Categoria.Find(i.aziende.idcategoria);
                a.Add(i.aziende);
                if (i.aziende.inattesa == true)
                {
                    ViewBag.azienda = " riechiesta in attesa di approvazione";
                }
                else
                {
                    ViewBag.azienda = " riechiesta respinta";
                    a = null;
                    aziende aziende = model1.aziende.Find(i.idaziende);
                    model1.imprenditori.Remove(i);
                    model1.aziende.Remove(aziende);
                    model1.SaveChanges();
                }
            }
            return View(a);
        }

        public ActionResult nonapprovare(int id)
        {
            aziende aziende = model1.aziende.Find(id);
            aziende.inattesa = false;
            aziende.verificata = false;
            model1.Entry(aziende).State = EntityState.Modified;
            model1.SaveChanges();

            return RedirectToAction("aggiungicollaboratore2");
        }

        public ActionResult approvare(int id)
        {
            aziende aziende = model1.aziende.Find(id);
            aziende.inattesa = false;
            aziende.verificata = true;
            model1.Entry(aziende).State = EntityState.Modified;
            imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.idaziende == id);
            utenti u = model1.utenti.Find(i.idUtenti);
            u.role = "d-mandager";
            u.confermapassword = u.password;
            model1.Entry(u).State = EntityState.Modified;
            model1.SaveChanges();
            return RedirectToAction("aggiungicollaboratore2");
        }
    }
}