using capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace capstone.Controllers
{
    public class gestionevenditeController : Controller
    {
        private Model1 model1 = new Model1();

        // GET: gestionevendite
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult carello()
        {
            utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
            if (utenti != null)
            {
                vendita venidita = model1.vendita.FirstOrDefault((e) => e.idUtenti == utenti.idUtenti && e.stato == "in carello");
                if (venidita != null)
                {
                    List<carello> c = model1.carello.Where((e) => e.idvendita == venidita.idvendita).ToList();
                    return View(c);
                }
            }
            return RedirectToAction("index", "home");
        }

        public JsonResult modifica(int component, int idprod)
        {
            utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
            vendita venidita = model1.vendita.FirstOrDefault((e) => e.idUtenti == utenti.idUtenti && e.stato == "in carello");
            if (venidita != null)
            {
                carello c = model1.carello.FirstOrDefault((e) => e.idvendita == venidita.idvendita && e.idprodotti == idprod);
                if (c != null)
                {
                    c.quantita = component;
                    model1.Entry(c).State = EntityState.Modified;
                    model1.SaveChanges();
                }
            }
            return Json(idprod);
        }

        public ActionResult aquista(string id)
        {
            utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
            vendita venidita = model1.vendita.FirstOrDefault((e) => e.idUtenti == utenti.idUtenti && e.stato == "in carello");
            if (venidita != null)
            {
                venidita.stato = "aquistato";
                venidita.prezzotot = Convert.ToDecimal(id);
                model1.Entry(venidita).State = EntityState.Modified;
                model1.SaveChanges();
            }
            return RedirectToAction("statoSpedizioni");
        }

        public ActionResult statoSpedizioni()
        {
            utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
            List<vendita> vendita = new List<vendita>();

            vendita = model1.vendita.Where((e) => e.idUtenti == utenti.idUtenti && e.stato != "consegnato").ToList();

            List<carello> c = new List<carello>();
            if (vendita.Count > 0)
            {
                foreach (var item in vendita)
                {
                    List<carello> carello = model1.carello.Where((e) => e.idvendita == item.idvendita).ToList();
                    foreach (var item1 in carello)
                    {
                        c.Add(item1);
                    }
                }
            }

            return View(c);
        }
    }
}