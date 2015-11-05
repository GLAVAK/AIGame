using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace GameServer.Controllers
{
    public class StaticPageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
