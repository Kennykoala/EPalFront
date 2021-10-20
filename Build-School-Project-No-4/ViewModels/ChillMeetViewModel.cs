using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class ChillMeetViewModel
    {
        public int MemberId { get; set; }
        public int LikeId { get; set; }
        public int? Gender { get; set; }
        public string Country { get; set; }
        public string LanguageName { get; set; }
        public string MemberName { get; set; }
        public string MeetPicture { get; set; }

        public int UserId { get; set; }
    }
}