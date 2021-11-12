using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.Models;

namespace Build_School_Project_No_4.Controllers
{
    public class RecommendController : Controller
    {
        private readonly ProductService _productService;


        public RecommendController()
        {
            _productService = new ProductService();
        }
        public ActionResult Recommened(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Recommened", "Recommend", new { id = 1 });
            }
            var GamesDeatils = _productService.GetGamesAllAndDeatils(id.Value);


            return View("Recommened", GamesDeatils);
        }

        public ActionResult GamesJson(int id)
        {
            ViewBag.ProductCard = _productService.GetProductCardsJson(id);

            return View();
        }
    }
}