using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class PayTrans
    {
        public string TransUID { get; set; }
        public DateTime TransDatetime { get; set; }
        public string ConfirmationId { get; set; }
        public string PayMethod { get; set; }
    }
}