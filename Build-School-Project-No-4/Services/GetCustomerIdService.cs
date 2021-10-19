using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Build_School_Project_No_4.Services
{
    public class GetCustomerIdService
    {
        public static string GetMemberId()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            string userid = "";
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                userid = ticket.UserData;
                return userid;
            }
            return null;
        }
    }
}