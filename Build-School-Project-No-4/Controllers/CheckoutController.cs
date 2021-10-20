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

namespace Build_School_Project_No_4.Controllers
{
    public class CheckoutController : Controller
    {
        //private readonly PaypalService _paypalService;
        private readonly OrderConfirmationService _orderConfirmService;
        private readonly EcPayService _ecPayService;
        private string trId;
        public CheckoutController()
        {
            //_paypalService = new PaypalService();
            _orderConfirmService = new OrderConfirmationService();
            _ecPayService = new EcPayService();
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


        public ActionResult PaymentWithEcPay()
        {
            List<string> enErrors = new List<string>();
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
                    oPayment.MerchantID = "2000132";//ECPay提供的特店編號

                    /* 基本參數 */
                    oPayment.Send.ReturnURL = "http://example.com";//付款完成通知回傳的網址
                    oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
                    oPayment.Send.OrderResultURL = "https://localhost:44322/ePals/ePal/1";//瀏覽器端回傳付款結果網址
                    oPayment.Send.MerchantTradeNo = "ECPay" + new Random().Next(0, 99999).ToString();//廠商的交易編號
                    oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
                    oPayment.Send.TotalAmount = Decimal.Parse("3280");//交易總金額
                    oPayment.Send.TradeDesc = "交易描述";//交易描述
                    oPayment.Send.ChoosePayment = PaymentMethod.ALL;//使用的付款方式
                    oPayment.Send.Remark = "";//備註欄位
                    oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
                    oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;//是否需要額外的付款資訊
                    oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
                    oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
                    oPayment.Send.PlatformID = "";//特約合作平台商代號
                    oPayment.Send.CustomField1 = "";
                    oPayment.Send.CustomField2 = "";
                    oPayment.Send.CustomField3 = "";
                    oPayment.Send.CustomField4 = "";
                    oPayment.Send.EncryptType = 1;

                    //訂單的商品資料
                    oPayment.Send.Items.Add(new Item()
                    {
                        Name = "蘋果",//商品名稱
                        Price = Decimal.Parse("3280"),//商品單價
                        Currency = "新台幣",//幣別單位
                        Quantity = Int32.Parse("1"),//購買數量
                        URL = "http://google.com",//商品的說明網址

                    });

                    /*************************非即時性付款:ATM、CVS 額外功能參數**************/

                    #region ATM 額外功能參數

                    //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
                    //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                    //oPayment.SendExtend.ClientRedirectURL = "";//Client 端回傳付款相關資訊

                    #endregion


                    #region CVS 額外功能參數

                    //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                    //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                    //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                    //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                    //oPayment.SendExtend.Desc_4 = "";//交易描述 4
                    //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                    //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                    #endregion

                    /***************************信用卡額外功能參數***************************/

                    #region Credit 功能參數

                    //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
                    //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
                    //oPayment.SendExtend.Language = ""; //語系設定

                    #endregion Credit 功能參數

                    #region 一次付清

                    //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
                    //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

                    #endregion

                    #region 分期付款

                    //oPayment.SendExtend.CreditInstallment = "3,6";//刷卡分期期數

                    #endregion 分期付款

                    #region 定期定額

                    //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
                    //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
                    //oPayment.SendExtend.Frequency = 1;//執行頻率
                    //oPayment.SendExtend.ExecTimes = 2;//執行次數
                    //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

                    #endregion

                    /* 產生訂單 */
                    var aaaa = oPayment.CheckOut();
                    //enErrors.AddRange(aaaa);
                    
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    // string szErrorMessage = String.Join("\\r\\n", enErrors);
                }

            }
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