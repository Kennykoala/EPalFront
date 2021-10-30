using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class LinestatusViewModel
    {

        public int MemberId { get; set; }

        public int? LineStatusId { get; set; }
    }


    public enum linestatusenum
    {
        ONLINE = 1,
        OFFLINE = 2,
        //RESTING = 3,
        //IN AN ORDER = 4
    }
}