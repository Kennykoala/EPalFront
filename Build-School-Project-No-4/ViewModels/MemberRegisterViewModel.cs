using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class MemberRegisterViewModel
    {
        public int MemberId { get; set; }


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

        [StringLength(10)]
        public string AuthCode { get; set; }

        public bool? IsAdmin { get; set; }

        [Display(Name = "記得我")]
        public bool Remember { get; set; }

        [StringLength(50)]
        public string MemberName { get; set; }

        public string ProfilePicture { get; set; }
    }
}