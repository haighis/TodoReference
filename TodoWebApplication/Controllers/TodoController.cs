using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akka.Actor;
using TodoActorService;
using WebApplicationSystem1.Models;

namespace WebApplicationSystem1.Controllers
{
    public class TodoController : Controller
    {
        private readonly ActorSystem _system;

        public TodoController()
        {
            _system = WebApiApplication.TodoFactory.ActorSystem;    
        }

        // GET: Todo
        public ActionResult Index()
        {
            return View("Create");
        }

        // GET: Todo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Todo/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create Todo";
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        public ActionResult Create(TodoViewModel model) // [Bind(Include = "TaskName")]
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var taskName = model.TaskName;

                    var todosActorService = new TodosActorService(_system);
                    todosActorService.SendTodo(taskName);
                }
                
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Todo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Todo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Todo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Todo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
