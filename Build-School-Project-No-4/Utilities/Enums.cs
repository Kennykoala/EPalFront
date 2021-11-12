using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Utilities
{
    public class Enums
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
        public enum PaymentType
        {
            PayPal,
            LinePay
        }
        public enum PayAttempt
        {
            Success = 1,
            Failed = 2
        }
        public enum DayOfWeek
        {
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday
        }
    }
}