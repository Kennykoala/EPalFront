using Build_School_Project_No_4.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.ViewModels
{
    public class AddgameViewModel
    {
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }        

        //public int MemberId { get; set; }
        public int ProductId { get; set; }
        public GameCategoryList GameCategoryId { get; set; }

        public int CreatorId { get; set; }

        public string ProductImg { get; set; }

        [DataType(DataType.MultilineText)]
        public string Introduction { get; set; }

        public string CreatorImg { get; set; }

        public string RecommendationVoice { get; set; }




        public IEnumerable<Server> ServerItems { get; set; }
        //public List<ProductServer> ServerAllItems { get; set; }

        [Display(Name ="Server")]
        //public ServerEum ServerId { get; set; }
        public IEnumerable<int> ServerSelectedId { get; set; }


        public IEnumerable<Position> PositionItems { get; set; }
        //public List<ProductPosition> PositionId { get; set; }

        [Display(Name = "Position")]
        //public PositionEum PositionId { get; set; }
        public IEnumerable<int> PositionSelectedId { get; set; }



        public IEnumerable<Style> StyleItems { get; set; }
        //public List<StyleIdEum> StyleId { get; set; }
        [Display(Name = "Style")]
        //public StyleIdEum  StyleId { get; set; }
        public IEnumerable<int> StyleSelectedId { get; set; }



        [Display(Name = "Rank")]
        public RankEum RankId { get; set; }




        public string GameAvailableDay1 { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime1 { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime1 { get; set; }



        public string GameAvailableDay2 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime2 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime2 { get; set; }


        public string GameAvailableDay3 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime3 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime3 { get; set; }

        public string GameAvailableDay4 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime4 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime4 { get; set; }

        public string GameAvailableDay5 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime5 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime5 { get; set; }

        public string GameAvailableDay6 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime6 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime6 { get; set; }

        public string GameAvailableDay7 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameStartTime7 { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? GameEndTime7 { get; set; }




        //[Display(Name = "Game AvailableDay")]
        //public string GameAvailableDay { get; set; }

        //[Display(Name = "Start Time ")]
        //[DataType(DataType.Time)]
        //public DateTime?[] GameStartTime { get; set; }

        //[Display(Name = "End Time")]
        //[DataType(DataType.Time)]
        //public DateTime?[] GameEndTime { get; set; }





        //public List<ProductPlanSet> planset { get; set; }

    }

    //public class ProductPlanSet
    //{
    //    //public int ProductId { get; set; }
    //    public string GameAvailableDay { get; set; }

    //    [DataType(DataType.Time)]
    //    public DateTime?[] GameStartTime { get; set; }

    //    [DataType(DataType.Time)]
    //    public DateTime?[] GameEndTime { get; set; }
    //}



    public enum RankEum
    {
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Platinum = 4,
        Diamond = 5,
        Master = 6,
        Challenger = 7,
        Unranked = 8
    }




    public enum GameCategoryList
    {
        LOL = 1,
        EChat = 2,
        Movie = 3,
        Valorant = 4,
        CustomGame = 5,
        Minecraft = 18,
        AmongUs = 20,
        ApexLegends = 22,
        TeamfightTactics = 23,
        Overwatch = 24,
        SleepCall = 25,
        AnimalCrossing_NewHorizons = 27,
        ArenaofValor = 28,
        ARK_SurvivalEvolved = 29,
        AssassinsCreed_Valhalla = 30,
        BlackDesertOnline = 31,
        Borderlands3 = 32,
        BrawlStars = 33,
        Brawlhalla = 34,
        Karaoke = 43,
        ASMR = 45,
        Relationshipadvice = 47,
        Emotionalsupport = 49
    }

    public enum ServerEum
    {
        OCE=1,
        NA=2,
        LAN=3,
        BR=4,
        EU_West=5,
        EU_NorthEast =6
    }
     
    public enum StyleIdEum
    {
        Love_Inting=1,
        Try_Hard=2,
        Hard_Stuck=3,
        Sneaky=4,
        Global_Presence=5,
        One_Shot=6
    }

    public enum PositionEum
    {
        Top =1,
        Jungler=2,
        ADC=3,
        Support=4,
        Middle=5,   
    }
}