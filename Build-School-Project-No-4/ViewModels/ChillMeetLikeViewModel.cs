using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class ChillMeetLikeViewModel
    {
        public int MemberId { get; set; }

        [Display(Name = "Name")]
        public string MemberName { get; set; }


        public int? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }

        public int? TimeZone { get; set; }

        public int? LanguageId { get; set; }

        public string Bio { get; set; }

        public string ProfilePicture { get; set; }

        public int? LineStatusId { get; set; }

        public bool? IsAdmin { get; set; }

        /// <summary>
        /// ChillMeetLike的追隨使用
        /// </summary>
        public int FollowingId { get; set; }
        /// <summary>
        /// ChillMeetLike的追隨的使用者Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 判斷是否已經Follow
        /// </summary>
        public bool isFollow { get; set; }


    }
}