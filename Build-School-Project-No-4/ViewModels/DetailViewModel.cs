using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class DetailViewModel
    {
        public int PlayerId { get; set; }
        public string MemberName { get; set; }
        public string Status { get; set; }
        public double UnitPrice { get; set; }
        public string Recording { get; set; }
        public string Intro { get; set; }
        public string PlayerPic { get; set; }
        public string GameScreenshot { get; set; }
        public string GameName { get; set; }
        public string GameBackdrop { get; set; }
        public string ServerName { get; set; }
        public string RankName { get; set; }
        public string LanguageName { get; set; }
        public string MobileGameImg { get; set; }

        public int Rounds { get; set; }
        public List<PlayerAvailability> Availability { get; set; }


    }
    public class PlayerAvailability
    {
        public int ProductId { get; set; }
        public string AvailDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

}