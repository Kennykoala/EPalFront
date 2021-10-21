using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
//using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ECPay.Payment.Integration;
using Newtonsoft.Json;
using System.Net;

namespace Build_School_Project_No_4.Controllers
{
    public class CheckoutController : Controller
    {
        //private readonly PaypalService _paypalService;
        private readonly OrderConfirmationService _orderConfirmService;
        private readonly EcPayService _ecPayService;
        //private readonly LinePayService _linePayService;
        private string trId;
        public CheckoutController()
        {
            //_paypalService = new PaypalService();
            _orderConfirmService = new OrderConfirmationService();
            _ecPayService = new EcPayService();
            //_linePayService = new LinePayService();
        }
        
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            string confirmation = TempData["confirmation"] as string;
            //APIContext apiContext = PaypalConfiguration.GetAPIContext();
            //try
            //{
            //    string payerId = Request.Params["PayerID"];
            //    if (string.IsNullOrEmpty(payerId))
            //    {
            //        string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Checkout/PaymentWithPayPal?";
            //        var guid = Convert.ToString((new Random()).Next(100000));
            //        var createdPayment = _paypalService.CreatePayment(apiContext, baseURI + "guid=" + guid, confirmation);
            //        var links = createdPayment.links.GetEnumerator();
            //        string paypalRedirectUrl = null;
            //        while (links.MoveNext())
            //        {
            //            Links lnk = links.Current;
            //            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
            //            {
            //                paypalRedirectUrl = lnk.href;
            //            }
            //        }
            //        Session.Add(guid, createdPayment.id);
            //        return Redirect(paypalRedirectUrl);
            //    }
            //    else
            //    {
            //        var guid = Request.Params["guid"];

            //        var executedPayment = _paypalService.ExecutePayment(apiContext, payerId, Session[guid] as string);
            //        trId = executedPayment.transactions[0].related_resources[0].sale.id;
            //        if (executedPayment.state.ToLower() != "approved")
            //        {
            //            return View("Failure");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Content(ex.ToString());
            //}
            return RedirectToAction("Success", new { Confirmation = confirmation });
        }

        //[NoDirectAccess]
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


        public ActionResult PaymentWithEcPay()
        {
            string ValueInToken = JsonConvert.SerializeObject(payin);
            string apiurl = "/v3/payments/request";
            string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
            string ChannelId = "1656554768";
            string nonce = Guid.NewGuid().ToString();
            string Signature = LinePayService.HmacSHA256((ChannelSecret + apiurl + ValueInToken + nonce), ChannelSecret)
                var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("X-LINE-ChannelId", ChannelId);
            request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
            request.Headers.Add("X-LINE-Authorization", Signature);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //底下取得line回傳資訊 這邊我是建立一個model LinePayOut 去包
var backModel = JsonConvert.DeserializeObject<LinePayOut>(responseString);

            if (backModel.returnCode == "0000")
            {
                return Redirect(backModel.info.paymentUrl.web);
            }
            else
            {
                return Content(backModel.returnMessage);
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