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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PaypalService _paypalService;
        private readonly OrderPaymentService _orderConfirmService;
        private readonly EcPayService _ecPayService;
        private readonly LinePayService _linePayService;
        private string trId;
        private readonly LinePayViewModel _linePayVM;
        private readonly CheckoutService _checkoutService;
        public CheckoutController()
        {
            _paypalService = new PaypalService();
            _orderConfirmService = new OrderPaymentService();
            _ecPayService = new EcPayService();
            _linePayService = new LinePayService();
            _linePayVM = new LinePayViewModel();
            _checkoutService = new CheckoutService();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            string confirmation = TempData["confirmation"] as string;
            if (Cancel == "true")
            {
                return RedirectToAction("Checkout", "ePals", new { Confirmation = confirmation });
            }
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    var paypalRedirectUrl = _paypalService.GetRedirectUrl(confirmation, apiContext);
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
            return RedirectToAction("Success", new { orderId = confirmation, transactionId = trId });
        }
        public async Task<ActionResult> PaymentWithLinePay(string Cancel = null, string transactionId = null)
        {
            string confirmation = TempData["confirmation"] as string;
            if (Cancel == "true")
            {
                return RedirectToAction("Checkout", "ePals", new { Confirmation = confirmation });
            }
            if (transactionId != null)
            {
                var payStatus = await _linePayService.ConfirmApiPost(long.Parse(transactionId), confirmation);
                if (payStatus == Enums.PayAttempt.Success.ToString())
                {
                    return Content("Success");
                }
                return Content("Fail");
            }
            var payRedirectUrl = await _linePayService.RequestApiPost(confirmation);
            return Redirect(payRedirectUrl);
        }

        public ActionResult Success(string transactionId, string orderId)
        {

            var isPaid = _orderConfirmService.UpdateToUnstarted(orderId, transactionId);

            if (isPaid == true)
            {
                var confirmationInfo = _orderConfirmService.GetConfirmationInfo(orderId);
                return View(confirmationInfo);
            }
            else
            {
                var productinfo = _checkoutService.GetPlayerIdFromConfirmation(orderId);
                return RedirectToAction("Detail", "ePals", new { id = productinfo.ProductId });
            }
        }


    }
}

