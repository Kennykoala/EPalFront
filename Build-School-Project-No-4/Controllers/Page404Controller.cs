using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.Controllers
{
    public class Page404Controller : Controller
    {
        // GET: Page404
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Page404()
        {
            Response.StatusCode = 404;

            return View();
        }
    }
}