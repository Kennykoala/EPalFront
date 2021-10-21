using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class WalletViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string MemberName { get; set; }
        public int Quality { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public string OrderStatusName { get; set; }
        public decimal TotalPrice { get; internal set; }
    }
}