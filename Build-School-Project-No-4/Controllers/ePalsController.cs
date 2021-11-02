using System.Web.Mvc;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Utilities;

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
            var GamesDeatils = _productService.GetGamesAllAndDeatils(id.Value);
            return View("ePal", GamesDeatils);
        }
        public ActionResult GamesJson(int id)
        {
            ViewBag.ProductCard = _productService.GetProductCardsJson(id);

            return View();
        }
        [HttpGet]
        public ActionResult Detail(int? id)
        {
            var playerListing = _detailService.FindPlayerListing(id);
            if (id == null || playerListing == null)
            {
                return RedirectToAction("Detail", new { id = 1 });
            }
            return View(playerListing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(AddToCartViewModel AddCartVM, string startTime, int id)
        {

            string currentUrl = Request.Url.AbsoluteUri;
            if (MemberUtil.GetMemberId() == null)
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
                return RedirectToAction("ePal");
            }
            var checkoutVM = _checkoutService.GetCheckoutDetails(confirmation);
            return View(checkoutVM);
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutViewModel x, string confirmation, string payType)
        {
            TempData["confirmation"] = confirmation;

            bool canCheckout = _checkoutService.ValidCheckoutTime(confirmation);
            int ePalId = _checkoutService.GetPlayerIdFromConfirmation(confirmation).ProductId;
            int id = ePalId;
            int orderStatus = _checkoutService.GetOrderStatus(confirmation);
            if (canCheckout == false || orderStatus != (int)Enums.PaymentStatus.Unpaid)
            {
                return RedirectToAction("Detail", new { id = id});
            }
            else
            {
                if (payType == "paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Checkout");
                }
                else
                {
                    return RedirectToAction("PaymentWithLinePay", "Checkout");
                }
            }
        }
    }
}