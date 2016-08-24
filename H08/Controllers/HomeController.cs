using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace H08.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            H08.Models.H08 theH08 = new H08.Models.H08();

            ViewBag.Base64 = theH08.base64String;

            return View();
        }

    }
}
