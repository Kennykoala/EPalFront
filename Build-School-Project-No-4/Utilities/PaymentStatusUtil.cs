using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Utilities
{
    public class PaymentStatusUtil
    {
        public enum PaymentStatus
        {
            Unpaid = 1, 
            Pending = 2, 
            NotStarted = 3, 
            Started = 4, 
            Completed = 5, 
            Cancelled = 6
        }
    }
}