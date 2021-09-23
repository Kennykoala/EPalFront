﻿using System;
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
        
        public string MemberName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RegistrationDate { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
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

        public string LineStatus { get; set; }
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