﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.Controllers
{
    public class GameInformationController : Controller
    {
        // GET: GameInformation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GameInformation()
        {
            return View();
        }

        public ActionResult Rule_VoiceRecord()
        {
            return View();
        }

        public ActionResult Rule_ListCover()
        {
            return View();
        }

        public ActionResult Rule_ScreenshotRules()
        {
            return View();
        }
    }
}