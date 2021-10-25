using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Build_School_Project_No_4.Controllers
{
    public class LinePayApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        //[Route("api/linepaycompleted/{transactionId}/{orderId}")]
        [AcceptVerbs("GET", "POST")]
        // public IHttpActionResult linepaycompleted(long? transactionId, string orderId)
        public string linepaycompleted(long? transactionId, string orderId)
        {
            ////var idk = something;
            //if (transactionId != null)
            //{
            //    var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            //    response.Headers.Location = new Uri
            //    return Redirect()
            //}

            return "abc";

        }


    }
}
