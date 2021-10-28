using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Controllers
{
    public class LinePayApiController : ApiController
    {
        private readonly LinePayService _linePayService;
        public LinePayApiController()
        {
            _linePayService = new LinePayService();
        }
        //[HttpGet]
        [Authorize]
        //[Route("api/linepaycompleted/{transactionId}/{orderId}")]
        [AcceptVerbs("GET", "POST")]
        // public IHttpActionResult linepaycompleted(long? transactionId, string orderId)
        public async Task<string> linepaycompleted(long? transactionId, string orderId)
        {
            //var confirmRedirectUrl = await _linePayService.ConfirmApiPost(orderId, transactionId);
            //return confirmRedirectUrl;
            using (var client = new HttpClient())
            {

                var requestbody = new
                {
                    amount = _linePayService.GetOrderTotal(orderId),
                    currency = "TWD"
                };

                var body = JsonConvert.SerializeObject(requestbody);
                string apiurl = $"/v3/payments/{transactionId}/confirm";
                var baseUri = "https://sandbox-api-pay.line.me";
                string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
                string ChannelId = "1656554768";
                var nonce = Utilities.PaymentUtility.CreateTransactionUID("1");


                string Signature = LinePayService.HashLinePayRequest(ChannelSecret, apiurl, body, nonce, ChannelSecret);
                //string Signature = LinePayService.HashLinePayConfirmRequest(orderId, body, apiurl);

                client.BaseAddress = new Uri("https://sandbox-api-pay.line.me");
                client.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
                client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", ChannelSecret);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);

                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(baseUri + apiurl, content);
                var result = await response.Content.ReadAsStringAsync();
                int i = 0;



                return JsonConvert.DeserializeObject<LinePayViewModel.LinePayRequestResponse>(result).returnMessage;
                //using (var client = new HttpClient())
                //{

                //    var requestbody = new
                //    {
                //        amount = _linePayService.GetOrderTotal(orderId),
                //        currency = "TWD"
                //    };

                //    var body = JsonConvert.SerializeObject(requestbody);
                //    string apiurl = $"/v3/payments/{transactionId}/confirm";
                //    var baseUri = "https://sandbox-api-pay.line.me";
                //    string ChannelSecret = "c8244dcfe709313a3b55afb35f0da7d1";
                //    string ChannelId = "1656554768";
                //    var nonce = Utilities.PaymentUtility.CreateTransactionUID("1");


                //    string Signature = LinePayService.HashLinePayRequest(nonce, body, apiurl);

                //    client.BaseAddress = new Uri("https://sandbox-api-pay.line.me");
                //    client.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
                //    client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", ChannelSecret);
                //    client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                //    client.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);

                //    var content = new StringContent(body, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync(baseUri + apiurl, content);
                //    var result = await response.Content.ReadAsStringAsync();



                //    return JsonConvert.DeserializeObject<LinePayViewModel.LinePayConfirmResponse>(result).returnMessage;

                //}


            }
        }
    }
}
