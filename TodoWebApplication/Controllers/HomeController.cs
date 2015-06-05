using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akka.Actor;
using Akka.Routing;
using TodoActorService;

namespace WebApplicationSystem1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ActorSystem _system;

        public HomeController()
        {
            _system = WebApiApplication.MasterCatalogFactory.ActorSystem;    
        }

        public ActionResult Index()
        {   
            ViewBag.Title = "Home Page";

            var todosActorService = new TodosActorService(_system);
            todosActorService.SendTodo("task " + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString());

            return View();
        }
    }
}
