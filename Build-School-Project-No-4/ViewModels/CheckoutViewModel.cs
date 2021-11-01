using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class CheckoutViewModel
    {
        public string OrderConfirmation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public int Rounds { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string GameName { get; set; }
        public string PlayerPic { get; set; }
        public int ProductId { get; set; }
        public int SelectedPayment { get; set; }
        public int OrderStatus { get; set; }

    }
}