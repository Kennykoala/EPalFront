using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Build_School_Project_No_4.ViewModels;
using System.Web.Security;

namespace Build_School_Project_No_4.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductService _productService;
        private readonly EPalContext _ctx;
        private readonly DetailServices _detailService;
        private readonly AddToCartService _cartService;
        private readonly CheckoutService _checkoutService;
        private readonly OrderService _orderService;

        public OrderController()
        {
            _productService = new ProductService();
            _detailService = new DetailServices();
            _ctx = new EPalContext();
            _cartService = new AddToCartService();
            _checkoutService = new CheckoutService();
            _orderService = new OrderService();
        }
        // GET: Order
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public string GetMemberId()
        {
            var cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            string userid = "";
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                userid = ticket.UserData;
                return userid;
            }
            return null;
        }
        public ActionResult OrderSummary(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction( "OrderSummary", "Order", new { id = 1 });
            }
            //if (!id.HasValue)
            //{
            //    return RedirectToAction("Index");
            //}
            var order = new OrderService();
            var abc = order.GetOrderCardData(id.Value);
            // var ordercards = _orderService.GetOrderCardData(id.Value);
            GroupViewModel result = new GroupViewModel
            {
                Order = abc
            };





            return View(result);
        }
       
    }
}