using Prova_Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prova_Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Entities1 db = new Entities1();
            var lista = db.Produtos.OrderBy(o => o.Nome).ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Detalhar(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            Entities1 db = new Entities1();
            var registro = db.Produtos.Where(w => w.ID == id).SingleOrDefault();
            if (registro == null)
            {
                return RedirectToAction("CadastroInexistente");
            }
            else
            {
                return View(registro);
            }
        }
    }
}