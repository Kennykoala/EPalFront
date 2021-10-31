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
        public CheckoutController()
        {
            _paypalService = new PaypalService();
            _orderConfirmService = new OrderPaymentService();
            _ecPayService = new EcPayService();
            _linePayService = new LinePayService();
            _linePayVM = new LinePayViewModel();
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
            return RedirectToAction("Success", new { Confirmation = confirmation });
        }


        public ActionResult Success(string confirmation)
        {
            var isPaid = _orderConfirmService.UpdateToUnstarted(confirmation);
            if (isPaid == true)
            {
                var confirmationInfo = _orderConfirmService.GetConfirmationInfo(confirmation);
                return View(confirmationInfo);
            }
            else
            {
                return Content("order status change didn't go through");
            }
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
                if(payStatus == Enums.PayAttempt.Success.ToString())
                {
                    return Content(":D");
                }
                return Content(":(");
            }
            var payRedirectUrl = await _linePayService.RequestApiPost(confirmation);
            return Redirect(payRedirectUrl);
        }
    }
}



        //public async Task<ActionResult> PaymentWithLinePay(string Cancel = null)
        //{
        //    string confirmation = TempData["confirmation"] as string;
        //    if (Cancel == "true")
        //    {
        //        return RedirectToAction("Checkout", "ePals", new { Confirmation = confirmation });
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        var requestBody = _linePayService.LinePayCreateOrder(confirmation);
        //        var body = JsonConvert.SerializeObject(requestBody);
        //        string apiurl = "/v3/payments/request";
        //        var baseUri = "https://sandbox-api-pay.line.me";
        //        string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
        //        string ChannelId = "1656554768";
        //        var customerId = GetCustomerIdService.GetMemberId();
        //        var nonce = Utilities.PaymentUtility.CreateTransactionUID(customerId);


        //        string Signature = LinePayService.HashLinePayRequest(confirmation, body);

        //        client.BaseAddress = new Uri("https://sandbox-api-pay.line.me");
        //        client.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
        //        client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", ChannelSecret);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
        //        client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);
        //        var content = new StringContent(body, Encoding.UTF8, "application/json");
        //        var response =  await client.PostAsync(baseUri + apiurl, content);
        //        var result = await response.Content.ReadAsStringAsync();
        //        var linepayapi = (JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).info.paymentUrl.web);
        //        return Redirect(JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).info.paymentUrl.web);
        //    }
        //}


        #region
        //public ActionResult PaymentWithEcPay()
        //{
        //    List<string> enErrors = new List<string>();

        //        using (AllInOne oPayment = new AllInOne())
        //        {
        //            /* 服務參數 */
        //            oPayment.ServiceMethod = ECPay.Payment.Integration.HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
        //            oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
        //            oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
        //            oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
        //            oPayment.MerchantID = "2000132";//ECPay提供的特店編號


        //            /* 基本參數 */
        //            oPayment.Send.ReturnURL = "http://example.com";//付款完成通知回傳的網址
        //            oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
        //            oPayment.Send.OrderResultURL = "";//瀏覽器端回傳付款結果網址
        //            oPayment.Send.MerchantTradeNo = "ECPay" + new Random().Next(0, 99999).ToString();//廠商的交易編號
        //            oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
        //            oPayment.Send.TotalAmount = Decimal.Parse("3280");//交易總金額
        //            oPayment.Send.TradeDesc = "交易描述";//交易描述
        //            oPayment.Send.ChoosePayment = PaymentMethod.Credit;//使用的付款方式
        //            oPayment.Send.Remark = "";//備註欄位
        //            oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
        //            oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;//是否需要額外的付款資訊
        //            oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
        //            oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
        //            oPayment.Send.PlatformID = "";//特約合作平台商代號
        //            oPayment.Send.CustomField1 = "";
        //            oPayment.Send.CustomField2 = "";
        //            oPayment.Send.CustomField3 = "";
        //            oPayment.Send.CustomField4 = "";
        //            oPayment.Send.EncryptType = 1;




        //            //訂單的商品資料
        //            oPayment.Send.Items.Add(new ECPay.Payment.Integration.Item()
        //            {
        //                Name = "蘋果",//商品名稱
        //                Price = Decimal.Parse("3280"),//商品單價
        //                Currency = "新台幣",//幣別單位
        //                Quantity = Int32.Parse("1"),//購買數量
        //                URL = "http://google.com",//商品的說明網址


        //            });

        //            /*************************非即時性付款:ATM、CVS 額外功能參數**************/

        //            #region ATM 額外功能參數

        //            //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
        //            //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
        //            //oPayment.SendExtend.ClientRedirectURL = "";//Client 端回傳付款相關資訊

        //            #endregion


        //            #region CVS 額外功能參數

        //            //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
        //            //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
        //            //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
        //            //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
        //            //oPayment.SendExtend.Desc_4 = "";//交易描述 4
        //            //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
        //            //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

        //            #endregion

        //            /***************************信用卡額外功能參數***************************/

        //            #region Credit 功能參數

        //            //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
        //            //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
        //            //oPayment.SendExtend.Language = ""; //語系設定

        //            #endregion Credit 功能參數

        //            #region 一次付清

        //            //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
        //            //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

        //            #endregion

        //            #region 分期付款

        //            //oPayment.SendExtend.CreditInstallment = "3,6";//刷卡分期期數

        //            #endregion 分期付款

        //            #region 定期定額

        //            //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
        //            //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
        //            //oPayment.SendExtend.Frequency = 1;//執行頻率
        //            //oPayment.SendExtend.ExecTimes = 2;//執行次數
        //            //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

        //            #endregion
        //            string aaa = string.Empty;
        //            /* 產生訂單 */
        //            enErrors.AddRange(oPayment.CheckOutString(ref aaa));
        //        ViewBag.test = aaa;
        //        return View();
        //        }





        //}
        #endregion

