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
        public ActionResult Index()
        {   
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
