using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class OrderDetailViewModel
    {
        public string PlayerName { get; set; }
        public int ProductId { get; set; }
        public string ProfilePic { get; set; }
        public string BuyerName { get; set; }
        public string BuyerProfilePic { get; set; }
        public int BuyerId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public string GameImg { get; set; }
        public string GameName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Rounds { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string OrderConfirmation { get; set; }
        public string PaymentConfirmation { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}