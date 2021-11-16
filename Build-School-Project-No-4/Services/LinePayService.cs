﻿using Build_School_Project_No_4.DataModels;
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
        private readonly OrderUtil _orderUtil;
        private readonly CheckoutService _checkoutService;

        public LinePayService()
        {
            _repo = new Repository();
            _linePayVM = new LinePayViewModel();
            _orderUtil = new OrderUtil();
            _checkoutService = new CheckoutService();
        }
        private static string baseUri = "https://sandbox-api-pay.line.me";
        private static string channelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
        private static string channelId = "1656554768";
        private static string customerId = MemberUtil.GetMemberId();
        public static string HashLinePayRequest(string orderId, string body)
        {
            string apiurl = "/v3/payments/request";
            var nonce = Utilities.PaymentUtil.CreateTransactionUID(customerId);
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
        public static string HashLinePayRequest(string channelSecret, string apiUrl, string body, string orderId, string key)
        {
            var request = channelSecret + apiUrl + body + orderId;
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
            var redirectUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/Checkout/Success";
            var order = _orderUtil.GetOrder(confirmation);
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
                var customerId = MemberUtil.GetMemberId();
                var nonce = Utilities.PaymentUtil.CreateTransactionUID(customerId);
                _checkoutService.CreateTransaction(confirmation, (int)Enums.PaymentType.LinePay, nonce);
                string Signature = HashLinePayRequest(channelSecret, apiurl, body, nonce, channelSecret);

                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", channelSecret);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUri + apiurl, content);
                var result = response.Content.ReadAsStringAsync().Result;
                var paymentUrl = (JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).Info.paymentUrl.web);
                return paymentUrl;
            }
        }

        public int GetOrderTotal(string confirmation)
        {
            var order = _orderUtil.GetOrder(confirmation);
            var total = (int)Math.Round(order.UnitPrice * order.Rounds, 0);
            return total;
        }

        public async Task<string> ConfirmApiPost(long transactionId, string orderId)
        {
            using (var client = new HttpClient())
            {
                var requestbody = new
                {
                    amount = GetOrderTotal(orderId),
                    currency = "TWD"
                };
                var body = JsonConvert.SerializeObject(requestbody);
                string apiurl = $"/v3/payments/{transactionId}/confirm";
                var nonce = Utilities.PaymentUtil.CreateTransactionUID(customerId);
                string Signature = LinePayService.HashLinePayRequest(channelSecret, apiurl, body, nonce, channelSecret);

                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", channelSecret);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUri + apiurl, content);
                var result = await response.Content.ReadAsStringAsync();
                if (JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).returnMessage == "Success.")
                {
                    return Enums.PayAttempt.Success.ToString();
                }
                return Enums.PayAttempt.Failed.ToString();
            }
        }
    }
}