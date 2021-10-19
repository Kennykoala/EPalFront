using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Build_School_Project_No_4.Services;

namespace Build_School_Project_No_4.Services
{
    public class PaymentUIDService
    {

        public static string CreateTransactionUID(string id)
        {
            var utcTimestamp = TimestampService.UtcDateTimeToUnix(DateTime.Now.ToUniversalTime());
            string transId = id + utcTimestamp;
            return transId;
        }

    }
}