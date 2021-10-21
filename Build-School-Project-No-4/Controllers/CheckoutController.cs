using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Build_School_Project_No_4.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PaypalService _paypalService;
        private readonly OrderConfirmationService _orderConfirmService;
        public CheckoutController()
        {
            _paypalService = new PaypalService();
            _orderConfirmService = new OrderConfirmationService();
        }


        // GET: Checkout
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            string confirmation = TempData["confirmation"] as string;
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Checkout/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = _paypalService.CreatePayment(apiContext, baseURI + "guid=" + guid, confirmation);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = _paypalService.ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
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
            //on successful payment, show success page to user.  

            return RedirectToAction("Success", new { Confirmation = confirmation });
        }

        //[NoDirectAccess]s
        public ActionResult Success(string confirmation)
        {
            var isPaid = _orderConfirmService.UpdateOrderStatus(confirmation);
            if (isPaid == true)
            {
                var confirmationInfo = _orderConfirmService.GetConfirmationInfo(confirmation);
                //GroupViewModel groupVM = new GroupViewModel
                //{
                //    OrderConfirmDetails = confirmationInfo
                //};
                return View(confirmationInfo);
            }
            else
            {
                return Content("order status change didn't go through");
            }
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