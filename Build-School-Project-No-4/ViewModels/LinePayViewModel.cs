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
            public string imageUrl { get; set; }
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
        public class LinePayRequestResponse
        {
            public string returnCode { get; set; }
            public string returnMessage { get; set; }
            public Info info { get; set; }
        }
        public class Info
        { 
            public Paymenturl paymentUrl { get; set; }
            public long transactionId { get; set; }
            public string paymentAccessToken { get; set; }
        }
        public class Paymenturl
        {
            public string web { get; set; }
            public string app { get; set; }
        }
        public class LinePayConfirmResponse
        {
            public string returnMessage { get; set; }
        }
    }
}