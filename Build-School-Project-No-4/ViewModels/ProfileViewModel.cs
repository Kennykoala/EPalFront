using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class ProfileViewModel
    {

        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int? Gender { get; set; }
        public string LineStatus { get; set; }
        public int FollowingId { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int RecentVistorId { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }

        public int FollowsNumber { get; set; }
        public int FollingsNumber { get; set; }
        public int VistorsNumber { get; set; }

        public string RecommendContent { get; set; }
        public int ProductId { get; set; }
        public int ServerId { get; set; }
        public int StarLevel { get; set; }

        public decimal StarAvg { get; set; }
        public int serverNumber { get; set; }
        public int commentNumber { get; set; }

    }
}