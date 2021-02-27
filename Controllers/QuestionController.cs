using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishBattleApp.Controllers
{
    public class QuestionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Question()
        {
            ViewBag.VerbInfinitif = "Guru Project Infinity !";
            return View();
        }
    }
}