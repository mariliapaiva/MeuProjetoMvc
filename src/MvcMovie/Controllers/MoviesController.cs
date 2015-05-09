using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        // GET: Movies //Métodos com o verbo Get, devolvem o HTML que tem a estrutura do formulário que será vista pelo usuário
        public ActionResult Index(string movieGenre,string searchString)
        {
            var GenreLst = new List<string>();
            var GenreQuery = from d in db.Movies
                             orderby d.Genre
                             select d.Genre;

            GenreLst.AddRange(GenreQuery.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst); //O selectlist é usado para preencher dropdownlist

            var movies = from m in db.Movies //A consulta é definida neste ponto, mas ainda não foi executada no banco de dados.
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                //Consultas LINQ não são executadas quando eles são definidos, ou quando são modificados, chamando um método como Where ou OrderBy. 
                //Em vez disso, a execução da consulta é adiada, o que significa que a avaliação de uma expressão é adiada até que seu 
                //valor realizado, na verdade iterada ou o 
                //método ToList é chamado.Ou seja, só se tem o resultado de uma expressão lâmbida, quando: "um consumer consumir a query".
                // ToList e foreach são exemplos de consomer.
                movies = movies.Where(s => s.Title.Contains(searchString));
                // Outra maneira de escrever a linha acima é:
                //from s in movies
                //where s.Title.Contains(searchString)
                //select s
            }
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(g => g.Genre == movieGenre);
            }
            return View(movies);
        }

        // GET: Movies/Details/5 >> Controller/Action Método/ ID
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create() //The Create method passes an empty movie object to the Create view.
        {
            return View();
        }
        // POST: Movies/Create //O browser cria uma requisição post com os dados do formulário que será tratada pelo método Create abaixo.  
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate,Genre,Price, Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        //All the HttpGet methods follow a similar pattern. They get a movie object (or list of objects, in the case of Index), and pass the model to the view.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //O atributo ValidateAntiForgeryToken é usado para evitar a falsificação de uma solicitação.
                                   //Ele valida o token XSRF gerado pela chamada @Html.AntiForgeryToken() na View. 
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Price, Rating")] Movie movie)
        {
            if (ModelState.IsValid)// É um método.
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); //After saving the data, the code redirects the user to the Index action method of the MoviesController class, which displays the movie collection, including the changes just made.
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")] //é colocado o"ActionName("Delete")" , para que o ASP.NET possa achar o método Delete com verbo POST, qdo receber a requisição. Já que o
        //nome do método está:DeleteConfirmed
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
