﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class GroupViewModel
    {
        public IEnumerable<MemberViewModel> MeetMatches { get; set; }
        public MemberViewModel MemberData { get; set; }
    }

}