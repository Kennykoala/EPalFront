﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class MemberInfoViewModel
    {

        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please enter your Name")]
        [Display(Name = "Name")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Please enter your Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please select your Gender")]
        public Genders? Gender { get; set; }

        [Required(ErrorMessage = "Please select your BirthDay")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "TimeZone(UTC)")]
        [Range((-11), 11)]
        public int? TimeZone { get; set; }

        [Required(ErrorMessage = "Please select your Language")]
        [Display(Name = "Language")]
        public LanguageCategories? LanguageId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        //[StringLength(15, MinimumLength = 6, ErrorMessage = "密碼需大於6個字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        //[Display(Name = "Avatar")]
        //[DataType(DataType.ImageUrl)]
        //public string ProfilePicture { get; set; }
    }

    
    public enum Genders
    {
        //None = 0,
        Male = 1,
        Female = 2,
        Other = 3,
    }

    public enum LanguageCategories
    {
        MandarinChinese = 1,
        Spanish = 2,
        English = 3,
        Hindi = 4,
        Arabic = 5,
        Bengali = 6,
        Portuguese = 7,
        Russian = 8,
        Japanese = 9,
        Turkish = 10,
        Korean = 11,
        French = 12,
        German = 13,
        Vietnamese = 14,
        Italian = 15,
        YueChinese = 16,
        Others = 17
    }
}