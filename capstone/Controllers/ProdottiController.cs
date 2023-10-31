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
    [Authorize(Roles = "d-manager,manager")]
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
                        p.valutazione = null;
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
                imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                prodotti prodotti = model1.prodotti.FirstOrDefault((e) => e.idaziende == i.idaziende);
                if (prodotti == null)
                {
                    return RedirectToAction("home", "index");
                }
                Session["id"] = id;
                return View(prodotti);
            }
        }

        [HttpPost]
        public JsonResult adddestcrizione(string imp)

        {
            if (imp != null)
            {
                prodotti prodotti = new prodotti();
                if (Session["id"] != null)
                {
                    int id = (int)Session["id"];
                    imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                    prodotti = model1.prodotti.Find(id);
                }
                else
                {
                    imprenditori i = model1.imprenditori.FirstOrDefault((e) => e.utenti.username == User.Identity.Name);
                    prodotti = model1.prodotti.FirstOrDefault((e) => e.invendita == false && e.idaziende == i.idaziende);
                }
                int n = -1;
                if (prodotti.descrizione != null)
                {
                    n = prodotti.descrizione.IndexOf(" dsi5zx ");
                }
                if (n >= 0)
                {
                    string parteDopo = prodotti.descrizione.Substring(n);
                    prodotti.descrizione = imp + " tr5zx " + parteDopo;
                }
                else
                {
                    int n2 = -1;
                    if (prodotti.descrizione != null)
                    {
                        n2 = prodotti.descrizione.IndexOf(" dsi5zx ");
                    }
                    if (n2 >= 0)
                    {
                        string parteDopo = prodotti.descrizione.Substring(n2);
                        prodotti.descrizione = imp + " tr5zx " + parteDopo;
                    }
                    else
                    {
                        prodotti.descrizione = imp;
                    }
                }

                model1.Entry(prodotti).State = EntityState.Modified;
                model1.SaveChanges();
            }
            return Json(imp);
        }

        public JsonResult adddestcrizionelg(string imp)
        {
            if (imp != null)
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
                int n = -1;
                if (prodotti.descrizione != null)
                {
                    n = prodotti.descrizione.IndexOf(" dsi5zx ");
                }
                if (n >= 0)
                {
                    string partePrima = " tr5zx " + prodotti.descrizione.Substring(0, n + " dsi5zx ".Length);
                    int n2 = -1;
                    if (prodotti.descrizione != null)
                    {
                        n2 = prodotti.descrizione.IndexOf(" dsi5zx ");
                    }
                    string parteDopo = "";
                    if (n2 > 0)
                    {
                        parteDopo = imp.Substring(n2);
                    }
                    ;
                    string res = partePrima + " " + imp + " " + parteDopo;
                    prodotti.descrizione = res;
                    model1.Entry(prodotti).State = EntityState.Modified;
                    model1.SaveChanges();
                }
                else
                {
                    imp = " tr5zx dsi5zx " + imp;
                    string partePrima = prodotti.descrizione;
                    int n2 = -1;
                    if (prodotti.descrizione != null)
                    {
                        n2 = prodotti.descrizione.IndexOf(" dsi5zx ");
                    }

                    string parteDopo = "";
                    if (n2 > 0)
                    {
                        parteDopo = imp.Substring(n2);
                    }

                    string res = partePrima + imp + parteDopo;
                    prodotti.descrizione = res;
                    model1.Entry(prodotti).State = EntityState.Modified;
                    model1.SaveChanges();
                }
            }
            return Json(imp);
        }

        public ActionResult concludi(int id)
        {
            prodotti prodotti = model1.prodotti.Find(id);
            prodotti.invendita = true;
            model1.Entry(prodotti).State = EntityState.Modified;
            model1.SaveChanges();
            return RedirectToAction($"dettagli/{id}", "Home");
        }
    }
}