using Prova_Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prova_Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(FormCollection form, HttpPostedFileBase imagemInput)
        {
            Produto produto = new Produto();
            produto.Nome = form["nomeInput"];
            produto.Descricao = form["descricaoInput"];
            string precoInput = form["valorinput"];
            string precocomdescontoInput = form["valorcomdescontoInput"];
            decimal preco;
            decimal precocomdesconto;
            if (decimal.TryParse(precoInput, NumberStyles.Number, CultureInfo.InvariantCulture, out preco))
            {
                produto.Preco = preco;
            }
            else
            {
                produto.Preco = 0;
            }

            if (decimal.TryParse(precocomdescontoInput, NumberStyles.Number, CultureInfo.InvariantCulture, out precocomdesconto))
            {
                produto.Comdesconto = precocomdesconto;
            }
            else
            {
                produto.Preco = 0;
            }

            if (imagemInput != null && imagemInput.ContentLength > 0)
            {
                string nomeOriginal = Path.GetFileNameWithoutExtension(imagemInput.FileName);
                string extensao = Path.GetExtension(imagemInput.FileName);
                string nomeArquivo = nomeOriginal + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extensao;
                string caminho = Path.Combine(Server.MapPath("~/wwwroot/uploads"), nomeArquivo);
                imagemInput.SaveAs(caminho);
                produto.Imagem = "/wwwroot/uploads/" + nomeArquivo;
            }

            Entities1 db = new Entities1();
            db.Produtos.Add(produto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            Entities1 db = new Entities1();
            var lista = db.Produtos.OrderBy(o => o.Nome).ToList();
            return View(lista);
        }

        public ActionResult Excluir(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            Entities1 db = new Entities1();
            var objetoProduto = db.Produtos.Where(w => w.ID == id).SingleOrDefault();
            if (objetoProduto == null)
            {
                return RedirectToAction("CadastroInexistente");
            }
            else
            {
                string caminhoImagemExcluida = Server.MapPath(objetoProduto.Imagem);
                if (System.IO.File.Exists(caminhoImagemExcluida))
                {
                    System.IO.File.Delete(caminhoImagemExcluida);
                }
                db.Produtos.Remove(objetoProduto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult CadastroInexistente()
        {
            return View();
        }

        public ActionResult Editar(int id = 0)
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

        [HttpPost]
        public ActionResult Editar(Produto produtoTelaEditar, HttpPostedFileBase imagemInput, FormCollection form)
        {
            Entities1 db = new Entities1();
            var objetoProdutoAntigo = db.Produtos.Where(w => w.ID == produtoTelaEditar.ID).SingleOrDefault();

            objetoProdutoAntigo.Nome = produtoTelaEditar.Nome;
            objetoProdutoAntigo.Descricao = form["descricaoInput"];
            objetoProdutoAntigo.Preco = produtoTelaEditar.Preco;
            objetoProdutoAntigo.Comdesconto = produtoTelaEditar.Comdesconto;
            if (imagemInput != null && imagemInput.ContentLength > 0)
            {
                string nomeOriginal = Path.GetFileNameWithoutExtension(imagemInput.FileName);
                string extensao = Path.GetExtension(imagemInput.FileName);
                string nomeArquivo = nomeOriginal + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extensao;
                string caminho = Path.Combine(Server.MapPath("~/wwwroot/uploads"), nomeArquivo);
                if (!string.IsNullOrEmpty(objetoProdutoAntigo.Imagem))
                {
                    string caminhoImagemAntiga = Server.MapPath(objetoProdutoAntigo.Imagem);
                    if (System.IO.File.Exists(caminhoImagemAntiga))
                    {
                        System.IO.File.Delete(caminhoImagemAntiga);
                    }
                }
                imagemInput.SaveAs(caminho);
                objetoProdutoAntigo.Imagem = "/wwwroot/uploads/" + nomeArquivo;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}