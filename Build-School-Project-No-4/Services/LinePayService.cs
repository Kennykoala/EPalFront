using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Services
{
    public class LinePayService
    {
        private readonly Repository _repo;
        private readonly LinePayViewModel _linePayVM;
        private readonly OrderUtility _orderUtil;
        public LinePayService()
        {
            _repo = new Repository();
            _linePayVM = new LinePayViewModel();
            _orderUtil = new OrderUtility();
        }
        public static string HashLinePayRequest(string channelSecret, string apiUrl, string body, string orderId, string key)
        {
            var request = channelSecret + apiUrl + body + orderId;
            key = key ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(request);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public LinePayViewModel.LinePayRequest LinePayCreateOrder(string confirmation)
        {
            var redirectUrl = HttpContext.Current.Request.Url.Host + "://" + HttpContext.Current.Request.Url.Authority + "/Checkout/PaymentWithLinePay?";
            var order = _orderUtil.GetOrder(confirmation);

            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var linePayRequest = new LinePayViewModel.LinePayRequest
            {
                amount = (int)Math.Round(order.UnitPrice * order.Rounds, 0),
                currency = "TWD",
                orderId = confirmation,
                redirectUrls = new LinePayViewModel.RedirectUrls()
                {
                    cancelUrl = redirectUrl + "&Cancel=true",
                    confirmUrl = redirectUrl
                },
                packages = new List<LinePayViewModel.Package>
                {
                    new LinePayViewModel.Package()
                    {
                        id = confirmation,
                        amount = (int)Math.Round(order.UnitPrice * order.Rounds, 0),

                        products = new List<LinePayViewModel.Product>()
                        {
                            new LinePayViewModel.Product()
                            {
                                name = $"{order.Rounds} round(s) of {order.GameName} with {order.ePalName}",
                                quantity = order.Rounds,
                                price = (int)Math.Round(order.UnitPrice, 0),
                                imageUrl = order.ePalImg
                            }
                        }
                    },
                }
            }; 

            return linePayRequest;
        }

        //public  Task RequestLinePay(string confirmation, string baseUri)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var requestBody = LinePayCreateOrder(confirmation);

        //        var body = JsonConvert.SerializeObject(requestBody);
        //        string apiurl = "/v3/payments/request";
        //        string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
        //        string ChannelId = "1656554768";
        //        //string nonce = Guid.NewGuid().ToString();
        //        string Signature = LinePayService.LinePayHMACSHA256((ChannelSecret + apiurl + body + confirmation), ChannelSecret);

        //        httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
        //        httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", ChannelSecret);
        //        httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", confirmation);
        //        httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);

        //        var content = new StringContent(body, Encoding.UTF8, "application/json");

        //        //var apiUrl = "https://localhost:44322/api/linepayapi/Confirm";
        //        HttpContent apiUrl = "https://localhost:44322/api/linepayapi/Confirm";
        //        var response = httpClient.PostAsync(baseUri, apiUrl, content);

        //    }

        //}
	}
}