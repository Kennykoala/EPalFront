using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class PaymentViewModel
    {
        public string ePalName { get; set; }
        public string CustomerName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Rounds { get; set; }
        public string GameName { get; set; }
        public string CustomerId { get; set; }
        public string ePalImg { get; set; }
        
    }
}