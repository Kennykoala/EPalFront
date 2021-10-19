using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.Repositories;
using Build_School_Project_No_4.DataModels;


namespace Build_School_Project_No_4.Controllers
{
    public class ePalsController : Controller
    {
        private readonly ProductService _productService;
        private readonly EPalContext _ctx;
        private readonly DetailServices _detailService;
        private readonly AddToCartService _cartService;
        private readonly CheckoutService _checkoutService;
        private readonly GetCustomerIdService _custIdService;

        public ePalsController()
        {
            _productService = new ProductService();
            _detailService = new DetailServices();
            _ctx = new EPalContext();
            _cartService = new AddToCartService();
            _checkoutService = new CheckoutService();
            _custIdService = new GetCustomerIdService();
        }

        public ActionResult ePal(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("ePal", "ePals", new { id = 1 });
            }
            //var ProductCards = _productService.GetProductCardsData(id.Value);
            var GamesDeatils = _productService.GetGamesAllAndDeatils(id.Value);

            GroupViewModel result = new GroupViewModel
            {
                GamesDetails = GamesDeatils,
                //ProductCards = ProductCards
            };
            //ViewBag.ProductCard = _productService.GetProductCardsJson(id.Value);
            return View("ePal",result);
        }

        public ActionResult GamesJson(int id)
        {
            ViewBag.ProductCard = _productService.GetProductCardsJson(id);

            return View();
        }



        /// <summary>
        /// Sonias shit don't touch
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Detail(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var playerListing = _detailService.FindPlayerListing(id);
            if (playerListing == null)
            {
                return RedirectToAction("NotFound");
            }
            GroupViewModel groupVM = new GroupViewModel
            {
                Deets = playerListing
            };
            return View(groupVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(GroupViewModel AddCartVM, string startTime, int id)
        {

            string currentUrl = Request.Url.AbsoluteUri;
            if (GetCustomerIdService.GetMemberId() == null)
            {
                return Redirect(currentUrl);
            }
            else
            {
                var unpaid = _cartService.CreateUnpaidOrder(AddCartVM, startTime, id);
                var isSuccess = _cartService.AddCartSuccess(unpaid);
                if (isSuccess)
                {
                    var confirmation = unpaid.OrderConfirmation;
                    return RedirectToAction("Checkout", new { Confirmation = confirmation });
                }
                else
                {
                    return Content("Failed to create new order");
                }
            }


        }
        [HttpGet]
        public ActionResult Checkout(string confirmation)
        {
            if (confirmation == null)
            {
                return Content("Order not found!");
            }
            var checkoutVM = _checkoutService.GetCheckoutDetails(confirmation);

            GroupViewModel groupVM = new GroupViewModel
            {
                Checkout = checkoutVM
            };
            return View(groupVM);
        }
        [HttpPost]
        public ActionResult Checkout(GroupViewModel x, string confirmation, string payType)
        {
            
            //add 判斷 for routing to right payment action
            if (payType == "payal")
            {
                return RedirectToAction("PaymentWithPaypal", "Checkout");
            }
            else
            {
                return RedirectToAction("PaymentWithEcPay", "Checkout");
            }
            
            
        }
    }
}