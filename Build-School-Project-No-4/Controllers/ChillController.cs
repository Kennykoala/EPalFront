using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Build_School_Project_No_4.Controllers
{
    public class ChillController : Controller
    {
        // GET: Chill
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Live()
        {
            return View();
        }

        //public ActionResult Meet()
        //{
        //    return View();
        //}

        //取得登入者的memberId
        public string GetMemberId()
        {
            var cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

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

        public ActionResult Meet()
        {
  

            int memberId;
            bool IsSuccess = true;
            string memId = GetMemberId();
            IsSuccess = int.TryParse(memId, out memberId);

            if (!IsSuccess)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //throw new NotImplementedException();
            }

            var meetGet = new ChillMeetService();
            var meetsContent = meetGet.GetMeetFiles(memberId);

            if (memberId == null)
            {

            }
            //GroupViewModel meetlikes = new GroupViewModel
            //{
            //    ChillMeetResult = meetsContent
            //};

            return View(meetsContent);
        }
        
        public ActionResult MeetLikes()
        {

            int memberId;
            bool IsSuccess = true;
            string memId = GetMemberId();
            IsSuccess = int.TryParse(memId, out memberId);

            if (!IsSuccess)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var memberlikeGet = new ChillMeetService();
            var memberMatchGet = new ChillMeetService();

            var memberMatch = memberMatchGet.GetMemberMatch();
            var memberlike = memberlikeGet.GetMemberLike(memberId);

            ViewBag.Match = memberMatch;
            ViewBag.Like = memberlike;
            //GroupViewModel meetlikes = new GroupViewModel
            //{
            //    MeetLikes = memberlike,
            //    Matches = memberMatch
            //};
            return View();
        }

        public ActionResult MeetMatches()
        {
            var memberGet = new MemberService();
            var members = memberGet.GetMember();

            return View(members);
        }
    }
}