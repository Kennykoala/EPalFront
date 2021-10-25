using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class LinePayService
    {
        private readonly Repository _repo;
        private readonly LinePayViewModel _linePayVM;
        public LinePayService()
        {
            _repo = new Repository();
            _linePayVM = new LinePayViewModel();
        }
		public static string LinePayHMACSHA256(string key, string message)
		{
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(key);

			HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

			byte[] messageBytes = encoding.GetBytes(message);
			byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

			//注意他原本的公式是直接轉為string
			return Convert.ToBase64String(hashmessage);
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
            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join m in members on p.CreatorId equals m.MemberId
                          join g in gameCat on p.GameCategoryId equals g.GameCategoryId
                          where o.OrderConfirmation == confirmation
                          select new LinePayViewModel.LinePayRequest
                          {
                              amount = 1,
                              currency = "TWD",
                              orderId = confirmation,
                              redirectUrls = new LinePayViewModel.RedirectUrls()
                              {
                                  cancelUrl = "https://facebook.com",
                                  confirmUrl = "https://localhost:44322/api/linepayapi/linepaycompleted"
                              },
                              packages = new List<LinePayViewModel.Package>
                             {
                                 new LinePayViewModel.Package()
                                 {
                                     id = "package-1",
                                     amount = 1,
                                     name = "test-name",
                                     
                                     products = new List<LinePayViewModel.Product>()
                                     {
                                         new LinePayViewModel.Product()
                                         {
                                             name = "prod-1",
                                             quantity = 1,
                                             price = 1
                                         }
                                     }
                             },

                          }
                          }
                              ).SingleOrDefault();
            return result;
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