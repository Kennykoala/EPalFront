using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class LinePayViewModel
    {
        public class Product
        {
            public string ePalName { get; set; }
            public string CustomerName { get; set; }
            public decimal UnitPrice { get; set; }
            public int Rounds { get; set; }
            public string GameName { get; set; }
            public string CustomerId { get; set; }
        }

        public class RedirectUrls
        {
            public string confirmUrl { get; set; }
            public string cancelUrl { get; set; }
        }
        public class LineForm
        {
            public decimal amount { get; set; }
            public string currency { get; set; }
            public string orderId { get; set; }
            public Product product { get; set; }
            public RedirectUrls redirectUrls { get; set; }
        }
        //public int Amount { get; set; }
        //public string currency { get; set; }
        //public string orderId { get; set; }
        //public Product product { get; set; }
        //public RedirectUrls redirectUrls { get; set; }

    }
}