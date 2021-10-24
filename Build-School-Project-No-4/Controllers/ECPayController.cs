using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Build_School_Project_No_4.Controllers
{
    public class ECPayController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public string GetPaymentInfo(string something)
        {
            var idk = something;

            return something;
        }
    }
}
