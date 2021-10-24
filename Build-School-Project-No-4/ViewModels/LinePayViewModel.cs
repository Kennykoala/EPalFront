using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class LinePayViewModel
    {
        public class Package
        {
            public string id { get; set; }
            public decimal amount { get; set; }
            public string name { get; set; }
            public List<Product> products { get; set; }

        }
        public class Product
        {
            public string name { get; set; }
            public int quantity { get; set; }
            public int price { get; set; }
        }

        public class RedirectUrls
        {
            public string confirmUrl { get; set; }
            public string cancelUrl { get; set; }
        }
        public class LinePayRequest
        {
            public int amount { get; set; }
            public string currency { get; set; }
            public string orderId { get; set; }
            public List<Package> packages { get; set; }
            public RedirectUrls redirectUrls { get; set; }
        }
        //public int Amount { get; set; }
        //public string currency { get; set; }
        //public string orderId { get; set; }
        //public Product product { get; set; }
        //public RedirectUrls redirectUrls { get; set; }

    }
}