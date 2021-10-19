using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ECPay.Payment.Integration;


namespace Build_School_Project_No_4.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PaypalService _paypalService;
        private readonly OrderConfirmationService _orderConfirmService;
        private string trId;
        public CheckoutController()
        {
            _paypalService = new PaypalService();
            _orderConfirmService = new OrderConfirmationService();
        }


        
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {

            string confirmation = TempData["confirmation"] as string;
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Checkout/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = _paypalService.CreatePayment(apiContext, baseURI + "guid=" + guid, confirmation);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];

                    var executedPayment = _paypalService.ExecutePayment(apiContext, payerId, Session[guid] as string);
                    trId = executedPayment.transactions[0].related_resources[0].sale.id;
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return RedirectToAction("Success", new { Confirmation = confirmation });
        }

        //[NoDirectAccess]
        public ActionResult Success(string confirmation)
        {
            var isPaid = _orderConfirmService.UpdateOrderStatus(confirmation);
            if (isPaid == true)
            {
                var confirmationInfo = _orderConfirmService.GetConfirmationInfo(confirmation);
                GroupViewModel groupVM = new GroupViewModel
                {
                    OrderConfirmDetails = confirmationInfo
                };
                return View(groupVM);
            }
            else
            {
                return Content("order status change didn't go through");
            }
        }

        public ActionResult PaymentWithEcPay(string confirmation)
        {
            CreateEcPayment();
            return View();
        }

        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        //public class NoDirectAccessAttribute : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (filterContext.HttpContext.Request.UrlReferrer == null ||
        //                    filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
        //        {
        //            filterContext.Result = new RedirectToRouteResult(new
        //                           RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
        //        }
        //    }
        //}
    }
}