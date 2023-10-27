using capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace capstone.Controllers
{
    [Authorize(Roles = "d-mandager,mandager")]
    public class ProdottiController : Controller
    {
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

        // GET: Prodotti
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult prodotti()
        {
            ViewBag.categoria = Listacategoria;
            return View();
        }

        [HttpPost]
        public ActionResult prodotti(prodotti p, HttpPostedFileBase fotoprodotto)
        {
            imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
            prodotti pr = model1.prodotti.FirstOrDefault((e) => e.invendita == false && e.idaziende == i.idaziende);
            if (pr == null)
            {
                if (ModelState.IsValid)
                {
                    if (fotoprodotto != null && fotoprodotto.ContentLength > 0)
                    {
                        p.invendita = false;
                        p.idaziende = i.idaziende;
                        p.fotoprodotto = fotoprodotto.FileName;
                        string pathToSave = Path.Combine(Server.MapPath("~/Content/img"), p.fotoprodotto);
                        fotoprodotto.SaveAs(pathToSave);
                        model1.prodotti.Add(p);
                        model1.SaveChanges();
                        return RedirectToAction("buildpagedetail");
                    }
                    else
                    {
                        ViewBag.fotoprodotto = "inserire un imaggine";
                    }
                }
                return View();
            }
            return RedirectToAction("buildpagedetail");
        }

        public ActionResult buildpagedetail(int? id)
        {
            if (id == null)
            {
                imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                prodotti pr = model1.prodotti.FirstOrDefault((e) => e.invendita == false && e.idaziende == i.idaziende);
                if (pr != null)
                {
                    prodotti prodotti = pr;
                    return View(prodotti);
                }
                else
                {
                    return RedirectToAction("prodotti", "Prodotti");
                }
            }
            else
            {
                prodotti prodotti = model1.prodotti.Find(id);
                Session["id"] = id;
                return View(prodotti);
            }
        }

        [HttpPost]
        public JsonResult adddestcrizione(string imp)
        {
            prodotti prodotti = new prodotti();
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                prodotti = model1.prodotti.Find(id);
            }
            else
            {
                imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                prodotti = model1.prodotti.FirstOrDefault((e) => e.invendita == false && e.idaziende == i.idaziende);
            }

            prodotti.descrizione = imp;
            model1.Entry(prodotti).State = EntityState.Modified;
            model1.SaveChanges();

            return Json(imp);
        }
    }
}