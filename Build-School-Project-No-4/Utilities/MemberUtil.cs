﻿using Build_School_Project_No_4.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Build_School_Project_No_4.Utilities
{
    public class MemberUtil
    {
        public static string GetMemberId()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            string userid = "";
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                var obj = JsonConvert.DeserializeObject<Members>(ticket.UserData);
                userid = obj.MemberId.ToString();
                return userid;
            }
            return null;
        }
    }
}