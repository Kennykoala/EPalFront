using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using Build_School_Project_No_4.Utilities;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;

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
        private static string baseUri = "https://sandbox-api-pay.line.me";
        private static string channelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
        private static string channelId = "1656554768";
        private static string customerId = GetCustomerIdService.GetMemberId();
        public static string HashLinePayRequest(string orderId, string body)
        {
            string apiurl = "/v3/payments/request";
            var nonce = Utilities.PaymentUtility.CreateTransactionUID(customerId);
            var request = channelSecret + apiurl + body + nonce;
            
            var key = channelSecret ?? "";
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
            var redirectUrl = "https://facebook.com";
            //var redirectUrl = HttpContext.Current.Request.Url.Host + "://" + HttpContext.Current.Request.Url.Authority + "/Checkout/PaymentWithLinePay?";
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

        public async Task<string> RequestApiPost(string confirmation)
        {
            using (var client = new HttpClient())
            {
                var requestBody = LinePayCreateOrder(confirmation);
                var body = JsonConvert.SerializeObject(requestBody);
                string apiurl = "/v3/payments/request";
                var baseUri = "https://sandbox-api-pay.line.me";
                string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
                string ChannelId = "1656554768";
                var customerId = GetCustomerIdService.GetMemberId();
                var nonce = Utilities.PaymentUtility.CreateTransactionUID(customerId);


                string Signature = LinePayService.HashLinePayRequest(confirmation, body);

                client.BaseAddress = new Uri("https://sandbox-api-pay.line.me");
                client.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", ChannelSecret);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);



                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUri + apiurl, content);
                var result = await response.Content.ReadAsStringAsync();

                var linepayapi = JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).info.paymentUrl.web;

                return linepayapi;
            }

        }

        public async Task<string> ConfirmApiPost(string payRedirectUrl)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            
        }





    }
}