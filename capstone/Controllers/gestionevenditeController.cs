using capstone.Models;
using System;
using System.Collections.Generic;
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
            vendita venidita = model1.vendita.FirstOrDefault((e) => e.idUtenti == utenti.idUtenti && e.stato == "in carello");
            List<carello> c = model1.carello.Where((e) => e.idvendita == venidita.idvendita).ToList();
            return View(c);
        }

        public ActionResult modifica(int number)
        {
            utenti utenti = model1.utenti.FirstOrDefault((e) => e.username == User.Identity.Name);
            vendita venidita = model1.vendita.FirstOrDefault((e) => e.idUtenti == utenti.idUtenti && e.stato == "in carello");
            if (venidita != null)
            {
                carello c = model1.carello.FirstOrDefault((e) => e.idvendita == venidita.idvendita);
            }
            return RedirectToAction("carello");
        }
    }
}