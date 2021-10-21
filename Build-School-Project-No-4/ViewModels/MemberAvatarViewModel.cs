using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class MemberAvatarViewModel
    {
        public int MemberId { get; set; }

        //[Display(Name = "Avatar")]
        //[DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }
    }
}