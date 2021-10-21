using System.Web.Mvc;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
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

        public ePalsController()
        {
            _productService = new ProductService();
            _detailService = new DetailServices();
            _ctx = new EPalContext();
            _cartService = new AddToCartService();
            _checkoutService = new CheckoutService();
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
        public ActionResult DetailPage(int? id)
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

            return View(playerListing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailPage(DetailViewModel AddCartVM, string startTime, int id)
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
        [HttpGet]
        public ActionResult Checkout(string confirmation)
        {
            if (confirmation == null)
            {
                return Content("Order not found!");
            }
            var checkoutVM = _checkoutService.GetCheckoutDetails(confirmation);

            //GroupViewModel groupVM = new GroupViewModel
            //{
            //    Checkout = checkoutVM
            //};

            return View(checkoutVM);
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutViewModel x, string confirmation)
        {
            TempData["confirmation"] = confirmation;
            return RedirectToAction("PaymentWithPaypal", "Checkout");
        }
    }
}