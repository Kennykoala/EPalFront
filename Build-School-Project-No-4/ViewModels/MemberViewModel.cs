using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
        
        [Display(Name="Name")]
        public string MemberName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "密碼需大於6個字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }

        public int? CityId { get; set; }

        public int? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }

        public int? TimeZone { get; set; }

        public int? LanguageId { get; set; }

        public string Bio { get; set; }

        public string ProfilePicture { get; set; }

        public int? LineStatusId { get; set; }

        [StringLength(10)]
        public string AuthCode { get; set; }

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


    /// <summary>
    /// [寄送驗證碼]參數
    /// </summary>
    public class SendMailTokenIn
    {
        public string Email { get; set; }
    }

    /// <summary>
    /// [寄送驗證碼]回傳
    /// </summary>
    public class SendMailTokenOut
    {
        public string ErrMsg { get; set; }
        public string ResultMsg { get; set; }
    }


    /// <summary>
    /// [重設密碼]參數
    /// </summary>
    public class DoResetPwdIn
    {
        public string NewUserPwd { get; set; }
        public string CheckUserPwd { get; set; }
    }

    /// <summary>
    /// [重設密碼]回傳
    /// </summary>
    public class DoResetPwdOut
    {
        public string ErrMsg { get; set; }
        public string ResultMsg { get; set; }
    }





    //public class SignupViewModel
    //{
    //    [Required(ErrorMessage = "Please enter your Email")]
    //    [EmailAddress]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "Please enter your password")]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    //[DataType(DataType.Password)]
    //    //[Display(Name = "確認密碼")]
    //    //[Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
    //    //public string ConfirmPassword { get; set; }
    //}


}