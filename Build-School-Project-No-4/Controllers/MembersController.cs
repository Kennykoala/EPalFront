﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using System.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Google.Apis.Auth;
using Google.Apis.Http;
using isRock.LineLoginV21;
using System.Text;
using System.Collections.Specialized;

namespace Build_School_Project_No_4.Controllers
{
    public class MembersController : Controller
    {

        private EPalContext db = new EPalContext();
        private MemberService _MemberService;
        private MailService _MailService;
        public MembersController()
        {
            _MemberService = new MemberService();
            _MailService = new MailService();
        }



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


       

        //[Authorize]
        public ActionResult profile()
        {
            int memberId;
            bool IsSuccess = true;
            string memId = GetMemberId();
            IsSuccess = int.TryParse(memId, out memberId);

            if(!IsSuccess)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //throw new NotImplementedException();
            }

            try
            {
                var profiles = new ProfileEpalService();
                var profileGetAll = profiles.GetProfiles(memberId);

                //GroupViewModel profileContent = new GroupViewModel
                //{
                //    Profiles = profileGetAll
                //};
                return View(profileGetAll);

            }
            catch(Exception ex)
            {
                return Content("失敗:" + ex.ToString());
            }           

        }

        public ActionResult Followings()
        {
            var memberGet = new FollowService();
            var members = memberGet.GetMemberFollow();
            var Followers = memberGet.GetMemberFollowers();

            //GroupViewModel followSelectMembers = new GroupViewModel
            //{
            //    FollowingMember = members,
            //    FollowerMember = Followers
            //};

            ViewBag.Followings = members;
            ViewBag.Followers = Followers;

            return View();
        }














        //public ActionResult LoginProcess()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> LoginProcess(string token)
        //{

        //    string msg = "ok";
        //    GoogleJsonWebSignature.Payload payload = null;
        //    try
        //    {
        //        payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings()
        //        {
        //            Audience = new List<string>() { "1025795679023-8g9j439beq7h92iv9us8nj3d77ifitr7.apps.googleusercontent.com" }//要驗證的client id，把自己申請的Client ID填進去
        //        });
        //        string email = payload.Email;
        //        string Id = payload.JwtId;
        //        string name = payload.Name;
        //    }
        //    catch (Google.Apis.Auth.InvalidJwtException ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    catch (Newtonsoft.Json.JsonReaderException ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //    }

        //    if (msg == "ok" && payload != null)
        //    {//都成功
        //        string user_id = payload.Subject;//取得user_id
        //        msg = $@"您的 user_id :{user_id}";
        //    }

        //    return Content(msg);

        //    ////// Nuget套件 System.IdentityModel.Tokens.Jwt
        //    ////var user = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token);

        //    //// 除此之外，也可以透過Google API 取得
        //    //var url = $"https://oauth2.googleapis.com/tokeninfo?id_token={token}";
        //    //var client = _clientFactory.CreateClient();
        //    //var response = await client.GetAsync(url);
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var responseContent = await response.Content.ReadAsStringAsync();
        //    //    // 略...
        //    //}

        //    //return View();
        //}


        ////line v2
        //[HttpPost]
        //public ActionResult LineLogin(string id_token)
        //{
        //    //利用id_token取得Claim資料
        //    var JwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
        //    var email = "";
        //    //如果有email
        //    if (JwtSecurityToken.Claims.ToList().Find(c => c.Type == "email") != null)
        //        email = JwtSecurityToken.Claims.First(c => c.Type == "email").Value;

        //    //ViewBag
        //    ViewBag.email = email;
        //    //ViewBag.access_token = token.access_token;
        //    //ViewBag.displayName = user.displayName;


        //    //return Json("OK", JsonRequestBehavior.AllowGet);
        //    //return View("LineResult");
        //    //return RedirectToAction("GetUserProfile");

        //    string lineemail = TempData["lineemail"].ToString();
        //    string linename = TempData["linename"].ToString();
        //    string msg = "ok";

        //    if (msg == "ok" && lineemail != null)
        //    {
        //        //確認是否已註冊Line
        //        //var memberDM = _MemberService.MemberLoginData()
        //        //            .Where(m => m.Email == email   )
        //        //            .FirstOrDefault();
        //        var memberRVM = _MemberService.MemberRigisterData()
        //                    .Where(m => m.Email == lineemail)
        //                    .FirstOrDefault();

        //        if (memberRVM == null)
        //        {
        //            Random rnd = new Random(Guid.NewGuid().GetHashCode());
        //            string rndnumber = rnd.Next(0, 100).ToString();
        //            //將密碼Hash
        //            rndnumber = _MemberService.HashPassword(rndnumber);

        //            //GroupViewModel -> DM
        //            Members emp = new Members
        //            {
        //                Email = lineemail,
        //                Password = rndnumber,
        //                LoginMethod = 3
        //            };
        //            db.Members.Add(emp);
        //            db.SaveChanges();


        //            Members meminfo = new Members()
        //            {
        //                MemberId = memberRVM.MemberId,
        //                MemberName = linename,
        //                //ProfilePicture = memberRVM.ProfilePicture
        //            };
        //            string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //            //建立FormsAuthenticationTicket
        //            var ticket = new FormsAuthenticationTicket(
        //                        version: 1,
        //                        name: lineemail.ToString(), //可以放使用者Id
        //                        issueDate: DateTime.UtcNow,//現在UTC時間
        //                        expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //                        isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //                        userData: JsonMeminfo, //可以放使用者角色名稱
        //                        cookiePath: FormsAuthentication.FormsCookiePath);

        //            //加密Ticket
        //            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //            //Create the cookie.
        //            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //            Response.Cookies.Add(cookie);

        //        }
        //        else
        //        {
        //            Members meminfo = new Members()
        //            {
        //                MemberId = memberRVM.MemberId,
        //                MemberName = memberRVM.MemberName,
        //                ProfilePicture = memberRVM.ProfilePicture,
        //                LoginMethod = 3
        //            };
        //            string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //            //建立FormsAuthenticationTicket
        //            var ticket = new FormsAuthenticationTicket(
        //                        version: 1,
        //                        name: lineemail.ToString(), //可以放使用者Id
        //                        issueDate: DateTime.UtcNow,//現在UTC時間
        //                        expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //                        isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //                        userData: JsonMeminfo, //可以放使用者角色名稱
        //                        cookiePath: FormsAuthentication.FormsCookiePath);

        //            //加密Ticket
        //            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //            //Create the cookie.
        //            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //            Response.Cookies.Add(cookie);

        //        }

        //        return Json(true);
        //    }
        //    msg = "error";
        //    return Content(msg);
        //}







        //line v1
        public ActionResult LineResult()
        {
            //return RedirectToAction("LineLogin");
            return Json("fail", JsonRequestBehavior.AllowGet);
        }

        //Line login
        [HttpGet]
        public ActionResult LineLoginCallback()
        {
            //取得返回的code
            var code = Request.QueryString["code"];
            if (code == null)
            {
                ViewBag.access_token = "沒有正確的code...";
                return View("LineResult");
            }

            //從Code取回token
            var token = Utility.GetTokenFromCode(code,
                "1656564684",  //TODO:請更正為你自己的 client_id
                "2af2ca5d39971c612d2a2dbccfdd2e54", //TODO:請更正為你自己的 client_secret
                "https://localhost:44322/Members/LineLoginCallback");  //TODO:請檢查此網址必須與你的LINE Login後台Call back URL相同

            //利用access_token取得用戶資料
            var user = Utility.GetUserProfile(token.access_token);
            //利用id_token取得Claim資料
            var JwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token.id_token);
            string lineemail = "";
            string linename = "";
            string lineId = "";
            //如果有email
            if (JwtSecurityToken.Claims.ToList().Find(c => c.Type == "email") != null)
                lineemail = JwtSecurityToken.Claims.First(c => c.Type == "email").Value;

            //ViewBag
            //ViewBag.email = lineemail;
            //ViewBag.access_token = token.access_token;
            linename = user.displayName;
            lineId = user.userId;
            //TempData["lineemail"] = email;
            //TempData["linename"] = user.displayName;

            string msg = "ok";
            //string email;
            //string fullname;
            if (msg == "ok" && lineemail != null)
            {
                //確認是否已註冊Line
                //var memberDM = _MemberService.MemberLoginData()
                //            .Where(m => m.Email == email   )
                //            .FirstOrDefault();
                var memberRVM = _MemberService.MemberRigisterData()
                            .Where(m => m.Email == lineemail)
                            .FirstOrDefault();

                if (memberRVM == null)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    string rndnumber = rnd.Next(0, 100).ToString();
                    //將密碼Hash
                    rndnumber = _MemberService.HashPassword(rndnumber);

                    //GroupViewModel -> DM
                    Members emp = new Members
                    {
                        Email = lineemail,
                        Password = rndnumber,
                        LoginMethod = 3
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        //MemberName = linename,
                        //ProfilePicture = memberRVM.ProfilePicture
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: lineemail.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                }
                else
                {
                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        MemberName = memberRVM.MemberName,
                        ProfilePicture = memberRVM.ProfilePicture,
                        LoginMethod = 3
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: lineemail.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                }

                return Redirect("/");
            }
            msg = "error";
            return Content(msg);

            //return Json("OK", JsonRequestBehavior.AllowGet);
            //return View("LineResult");
            //return RedirectToAction("GetUserProfile");       

        }



        //[HttpPost]
        //public ActionResult GetUserProfile(string Token, string email)
        //{
        //    //透過token取得用戶資料
        //    var user = Utility.GetUserProfile(Token);
        //    ViewBag.UserProfileJSON = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        //    //ViewBag
        //    ViewBag.email = email;
        //    ViewBag.access_token = Token;
        //    return View("LineResult");

        //    //string lineemail = TempData["lineemail"].ToString();
        //    //string linename = TempData["linename"].ToString();
        //    //string msg = "ok";
        //    ////string email;
        //    ////string fullname;

        //    //if (msg == "ok" && lineemail != null)
        //    //{
        //    //    //確認是否已註冊Line
        //    //    //var memberDM = _MemberService.MemberLoginData()
        //    //    //            .Where(m => m.Email == email   )
        //    //    //            .FirstOrDefault();
        //    //    var memberRVM = _MemberService.MemberRigisterData()
        //    //                .Where(m => m.Email == lineemail)
        //    //                .FirstOrDefault();

        //    //    if (memberRVM == null)
        //    //    {
        //    //        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        //    //        string rndnumber = rnd.Next(0, 100).ToString();
        //    //        //將密碼Hash
        //    //        rndnumber = _MemberService.HashPassword(rndnumber);

        //    //        //GroupViewModel -> DM
        //    //        Members emp = new Members
        //    //        {
        //    //            Email = lineemail,
        //    //            Password = rndnumber,
        //    //            LoginMethod = 3
        //    //        };
        //    //        db.Members.Add(emp);
        //    //        db.SaveChanges();


        //    //        Members meminfo = new Members()
        //    //        {
        //    //            MemberId = memberRVM.MemberId,
        //    //            MemberName = linename,
        //    //            //ProfilePicture = memberRVM.ProfilePicture
        //    //        };
        //    //        string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //    //        //建立FormsAuthenticationTicket
        //    //        var ticket = new FormsAuthenticationTicket(
        //    //                    version: 1,
        //    //                    name: lineemail.ToString(), //可以放使用者Id
        //    //                    issueDate: DateTime.UtcNow,//現在UTC時間
        //    //                    expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //    //                    isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //    //                    userData: JsonMeminfo, //可以放使用者角色名稱
        //    //                    cookiePath: FormsAuthentication.FormsCookiePath);

        //    //        //加密Ticket
        //    //        var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //    //        //Create the cookie.
        //    //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    //        Response.Cookies.Add(cookie);

        //    //    }
        //    //    else
        //    //    {
        //    //        Members meminfo = new Members()
        //    //        {
        //    //            MemberId = memberRVM.MemberId,
        //    //            MemberName = memberRVM.MemberName,
        //    //            ProfilePicture = memberRVM.ProfilePicture,
        //    //            LoginMethod = 3
        //    //        };
        //    //        string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //    //        //建立FormsAuthenticationTicket
        //    //        var ticket = new FormsAuthenticationTicket(
        //    //                    version: 1,
        //    //                    name: lineemail.ToString(), //可以放使用者Id
        //    //                    issueDate: DateTime.UtcNow,//現在UTC時間
        //    //                    expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //    //                    isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //    //                    userData: JsonMeminfo, //可以放使用者角色名稱
        //    //                    cookiePath: FormsAuthentication.FormsCookiePath);

        //    //        //加密Ticket
        //    //        var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //    //        //Create the cookie.
        //    //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    //        Response.Cookies.Add(cookie);

        //    //    }

        //    //    return Json(true);
        //    //    ////獲取該頁面url的參數資訊
        //    //    //string returnURL = Request.Params["HTTP_REFERER"];
        //    //    //int index = returnURL.IndexOf('=');
        //    //    //returnURL = returnURL.Substring(index + 1);

        //    //    ////如果參數為空，則跳轉到首頁，否則切回原頁面
        //    //    //if (string.IsNullOrEmpty(returnURL))
        //    //    //    return Redirect("/Home/HomePage");
        //    //    //else
        //    //    //    return Redirect(returnURL);
        //    //}
        //    //msg = "error";
        //    //return Content(msg);
        //}



        ////line v3
        //public ActionResult LineLoginDirect()
        //{
        //    string response_type = "code";
        //    string client_id = "1656564684";
        //    string redirect_uri = HttpUtility.UrlEncode("https://localhost:44322");
        //    string state = "linelogin";
        //    string LineLoginUrl = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope=openid%20profile%20email&nonce=09876xyz",
        //        response_type,
        //        client_id,
        //        redirect_uri,
        //        state
        //        );
        //    return Redirect(LineLoginUrl);
        //}

        //public ActionResult callback(string code, string state)
        //{
        //    if (state == "linelogin")
        //    {
        //        #region Api變數宣告
        //        WebClient wc = new WebClient();
        //        wc.Encoding = Encoding.UTF8;
        //        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        //        string result = string.Empty;
        //        NameValueCollection nvc = new NameValueCollection();
        //        #endregion

        //        string displayName;
        //        string useremail;
        //        try
        //        {
        //            //取回Token
        //            string ApiUrl_Token = "https://api.line.me/oauth2/v2.1/token";
        //            nvc.Add("grant_type", "authorization_code");
        //            nvc.Add("code", code);
        //            nvc.Add("redirect_uri", "https://localhost:44322");
        //            nvc.Add("client_id", "1656564684");
        //            nvc.Add("client_secret", "2af2ca5d39971c612d2a2dbccfdd2e54");
        //            string JsonStr = Encoding.UTF8.GetString(wc.UploadValues(ApiUrl_Token, "POST", nvc));
        //            LineLoginToken ToKenObj = JsonConvert.DeserializeObject<LineLoginToken>(JsonStr);
        //            wc.Headers.Clear();

        //            //取回User Profile
        //            string ApiUrl_Profile = "https://api.line.me/v2/profile";
        //            wc.Headers.Add("Authorization", "Bearer " + ToKenObj.access_token);
        //            string UserProfile = wc.DownloadString(ApiUrl_Profile);
        //            LineProfile ProfileObj = JsonConvert.DeserializeObject<LineProfile>(UserProfile);

        //            displayName = ProfileObj.displayName;
        //            useremail = ProfileObj.useremail;

        //            return RedirectToAction("UserProfile", "Home", new { displayName = ProfileObj.displayName, pictureUrl = ProfileObj.pictureUrl });
        //        }
        //        catch (Exception ex)
        //        {
        //            string msg = ex.Message;
        //            throw;
        //        }
        //    }


        //    //    if (msg == "ok" && lineemail != null)
        //    //    {
        //    //        //確認是否已註冊Line
        //    //        //var memberDM = _MemberService.MemberLoginData()
        //    //        //            .Where(m => m.Email == email   )
        //    //        //            .FirstOrDefault();
        //    //        var memberRVM = _MemberService.MemberRigisterData()
        //    //                    .Where(m => m.Email == lineemail)
        //    //                    .FirstOrDefault();

        //    //        if (memberRVM == null)
        //    //        {
        //    //            Random rnd = new Random(Guid.NewGuid().GetHashCode());
        //    //            string rndnumber = rnd.Next(0, 100).ToString();
        //    //            //將密碼Hash
        //    //            rndnumber = _MemberService.HashPassword(rndnumber);

        //    //            //GroupViewModel -> DM
        //    //            Members emp = new Members
        //    //            {
        //    //                Email = lineemail,
        //    //                Password = rndnumber,
        //    //                LoginMethod = 3
        //    //            };
        //    //            db.Members.Add(emp);
        //    //            db.SaveChanges();


        //    //            Members meminfo = new Members()
        //    //            {
        //    //                MemberId = memberRVM.MemberId,
        //    //                MemberName = linename,
        //    //                //ProfilePicture = memberRVM.ProfilePicture
        //    //            };
        //    //            string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //    //            //建立FormsAuthenticationTicket
        //    //            var ticket = new FormsAuthenticationTicket(
        //    //                        version: 1,
        //    //                        name: lineemail.ToString(), //可以放使用者Id
        //    //                        issueDate: DateTime.UtcNow,//現在UTC時間
        //    //                        expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //    //                        isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //    //                        userData: JsonMeminfo, //可以放使用者角色名稱
        //    //                        cookiePath: FormsAuthentication.FormsCookiePath);

        //    //            //加密Ticket
        //    //            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //    //            //Create the cookie.
        //    //            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    //            Response.Cookies.Add(cookie);

        //    //        }
        //    //        else
        //    //        {
        //    //            Members meminfo = new Members()
        //    //            {
        //    //                MemberId = memberRVM.MemberId,
        //    //                MemberName = memberRVM.MemberName,
        //    //                ProfilePicture = memberRVM.ProfilePicture,
        //    //                LoginMethod = 3
        //    //            };
        //    //            string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

        //    //            //建立FormsAuthenticationTicket
        //    //            var ticket = new FormsAuthenticationTicket(
        //    //                        version: 1,
        //    //                        name: lineemail.ToString(), //可以放使用者Id
        //    //                        issueDate: DateTime.UtcNow,//現在UTC時間
        //    //                        expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
        //    //                        isPersistent: memberRVM.Remember,// 是否要記住我 true or false
        //    //                        userData: JsonMeminfo, //可以放使用者角色名稱
        //    //                        cookiePath: FormsAuthentication.FormsCookiePath);

        //    //            //加密Ticket
        //    //            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //    //            //Create the cookie.
        //    //            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    //            Response.Cookies.Add(cookie);

        //    //        }

        //    //        return Json(true);
        //    //    }
        //    //    msg = "error";
        //    //    return Content(msg);



        //    return View();
        //}

        public class LineLoginToken
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }

        public class LineProfile
        {
            public string userId { get; set; }
            public string displayName { get; set; }
            public string pictureUrl { get; set; }
            public string statusMessage { get; set; }
            public string useremail { get; set; }
        }






        //FB  login
        [HttpPost]
        public ActionResult FBLogin(string Fbemail, string Fbname)
        {
            string msg = "ok";
            string email;
            string fullname;

            if (msg == "ok" && Fbemail != null)
            {
                email = Fbemail;
                fullname = Fbname;
                //確認是否已註冊FB
                //var memberDM = _MemberService.MemberLoginData()
                //            .Where(m => m.Email == email   )
                //            .FirstOrDefault();
                var memberRVM = _MemberService.MemberRigisterData()
                            .Where(m => m.Email == email)
                            .FirstOrDefault();

                if (memberRVM == null)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    string rndnumber = rnd.Next(0, 100).ToString();
                    //將密碼Hash
                    rndnumber = _MemberService.HashPassword(rndnumber);

                    //GroupViewModel -> DM
                    Members emp = new Members
                    {
                        Email = email,
                        Password = rndnumber,
                        LoginMethod = 2
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        MemberName = fullname,
                        //ProfilePicture = memberRVM.ProfilePicture
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: email.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                }
                else
                {

                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        MemberName = memberRVM.MemberName,
                        ProfilePicture = memberRVM.ProfilePicture,
                        LoginMethod = 2
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: email.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                }

                return Json(true);
            }
            msg = "error";
            return Content(msg);

        }



        //google 
        [HttpPost]
        public async Task<ActionResult> GoogleLogin(string id_token)
        {
            string msg = "ok";
            string email;
            string fullname;
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { "1025795679023-8g9j439beq7h92iv9us8nj3d77ifitr7.apps.googleusercontent.com" }//要驗證的client id，把自己申請的Client ID填進去
                });
                //email = payload.Email;
                //string Id = payload.JwtId;
                //string name = payload.Name;
            }
            catch (Google.Apis.Auth.InvalidJwtException ex)
            {
                msg = ex.Message;
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                msg = ex.Message;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }


            if (msg == "ok" && payload != null)
            {
                email = payload.Email;
                fullname = payload.Name;
                //確認是否已註冊google
                //var memberDM = _MemberService.MemberLoginData()
                //            .Where(m => m.Email == email   )
                //            .FirstOrDefault();
                var memberRVM = _MemberService.MemberRigisterData()
                            .Where(m => m.Email == email  )
                            .FirstOrDefault();

                if (memberRVM == null)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    string rndnumber = rnd.Next(0, 100).ToString();
                    //將密碼Hash
                    rndnumber = _MemberService.HashPassword(rndnumber);

                    //GroupViewModel -> DM
                    Members emp = new Members
                    {
                        Email = email,
                        Password = rndnumber,
                        LoginMethod = 1
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        MemberName = fullname,
                        //ProfilePicture = memberRVM.ProfilePicture
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: email.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);


                    //msg = "新增會員成功";
                    //return Content(msg);
                    //return RedirectToAction("HomePage", "Home");
                }
                else
                {
                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        MemberName = memberRVM.MemberName,
                        ProfilePicture = memberRVM.ProfilePicture,
                        LoginMethod = 1
                    };
                    string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                    //建立FormsAuthenticationTicket
                    var ticket = new FormsAuthenticationTicket(
                                version: 1,
                                name: email.ToString(), //可以放使用者Id
                                issueDate: DateTime.UtcNow,//現在UTC時間
                                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                                isPersistent: memberRVM.Remember,// 是否要記住我 true or false
                                userData: JsonMeminfo, //可以放使用者角色名稱
                                cookiePath: FormsAuthentication.FormsCookiePath);

                    //加密Ticket
                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    //Create the cookie.
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                }

                return Json(true);
            }
            msg = "error";
            return Content(msg);

        }



        //[HttpPut]
        [HttpPost]
        public ActionResult MemberStatus(int LineStatusId)
        {
            int memberid = int.Parse(GetMemberId());

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var memberdata = db.Members.First(x => x.MemberId == memberid);
                    memberdata.LineStatusId = LineStatusId;
                    //db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    tran.Commit();

                    return Content("更新linestatus成功");
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    return Content("更新linestatus失敗:" + ex.ToString());
                }
            }
            //return View();
        }



        [HttpGet]
        [Authorize]
        public ActionResult EditProfile()
        {
            MemberInfoViewModel MemberInfo = _MemberService.GetEditProfileInfo(int.Parse(GetMemberId()));

            //Members emp = db.Members.Find(int.Parse(GetMemberId()));
            //if (emp == null)
            //{
            //    return HttpNotFound();
            //}

            //if(emp.Gender == null)
            //{
            //    emp.Gender = 0;
            //}
            //if (emp.LanguageId == null)
            //{
            //    emp.LanguageId = 0;
            //}

            ////DM -> MemberInfoViewModel -> GroupViewModel
            //MemberInfoViewModel MemberInfo = new MemberInfoViewModel()
            //{
            //    MemberId = emp.MemberId,
            //    MemberName = emp.MemberName,
            //    Phone = emp.Phone,
            //    Country = emp.Country,
            //    Gender = (Genders)emp.Gender,
            //    BirthDay = emp.BirthDay,
            //    TimeZone = emp.TimeZone,
            //    LanguageId = (LanguageCategories)emp.LanguageId,
            //    Bio = emp.Bio,
            //    Email = emp.Email,
            //    Password = emp.Password,
            //    ProfilePicture = emp.ProfilePicture
            //};


            //GroupViewModel groupMember = new GroupViewModel()
            //{
            //    MemberInfo = new MemberInfoViewModel()
            //};

            //groupMember.MemberInfo = MemberInfo;
            ViewBag.Avatar = MemberInfo.ProfilePicture;                        

            return View("EditProfile", MemberInfo);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "MemberInfo")] MemberInfoViewModel EditMember)
        {   
            //密碼Hash
            EditMember.Password = _MemberService.HashPassword(EditMember.Password);

            //GroupViewModel -> MemberInfoViewModel -> DM
            Members emp = new Members
            {
                MemberId = EditMember.MemberId,
                MemberName = EditMember.MemberName,
                Phone = EditMember.Phone,
                Country = EditMember.Country,
                Gender = (int)EditMember.Gender,
                BirthDay = EditMember.BirthDay,
                TimeZone = EditMember.TimeZone,
                LanguageId = (int)EditMember.LanguageId,
                Bio = EditMember.Bio,
                Email = EditMember.Email,
                Password = EditMember.Password
            };

            if (ModelState.IsValid)
            {            
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                        tran.Commit();

                        return Content("寫入資料庫成功");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        return Content("寫入資料庫失敗:" + ex.ToString());
                    }
                }

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(EditMember);
        }





        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            //獲取該頁面url的參數資訊
            string returnURL = Request.Params["HTTP_REFERER"];
            int index = returnURL.IndexOf('=');
            returnURL = returnURL.Substring(index + 1);
            //如果參數為空，則跳轉到首頁，否則切回原頁面
            if (string.IsNullOrEmpty(returnURL))
                return Redirect("/Home/HomePage");
            else
                return Redirect(returnURL);
        }





        //宣告使用者在session標示
        public static string LoginUserKey = "UserInfo@CCC";

        //Get: Members/Login
        public ActionResult Login()
        {
            return View();
        }

        //Post: Members/Login
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(MemberLoginViewModel loginMember)
        {
            //未通過Model驗證
            if (!ModelState.IsValid)
            {
                return View();
            }

            //驗證登入email.密碼，回傳結果
            string ValidateStr = _MemberService.LoginCheck(loginMember.Email, loginMember.Password);

            Members user = _MemberService.GetDataByAccount(loginMember.Email);

            if (String.IsNullOrEmpty(ValidateStr))
            {
                //通過Model驗證後, 使用HtmlEncode將帳密做HTML編碼, 去除有害的字元
                string email = HttpUtility.HtmlEncode(loginMember.Email);
                //string password = HashService.MD5Hash(HttpUtility.HtmlEncode(loginVM.Password));

                Members meminfo = new Members()
                {
                    MemberId = user.MemberId,
                    MemberName = user.MemberName,
                    ProfilePicture = user.ProfilePicture
                };
                string JsonMeminfo = JsonConvert.SerializeObject(meminfo);

                //建立FormsAuthenticationTicket
                var ticket = new FormsAuthenticationTicket(
                            version: 1,
                            name: user.Email.ToString(), //可以放使用者Id
                            issueDate: DateTime.UtcNow,//現在UTC時間
                            expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                            isPersistent: loginMember.Remember,// 是否要記住我 true or false
                            userData: JsonMeminfo, //可以放使用者角色名稱
                            //userData: user.MemberId.ToString(), //可以放使用者角色名稱
                            cookiePath: FormsAuthentication.FormsCookiePath);

                //加密Ticket
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                //Create the cookie.
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                ////4.取得original URL.
                //var url = FormsAuthentication.GetRedirectUrl(email, true);

                ////5.導向original URL
                //return Redirect(url);

                //FormsAuthentication.RedirectFromLoginPage(loginMember.Email, true);
                ////TempData["LoginState"] = "Welcome!";
                ////return RedirectToAction("LoginResult");



                //反回原頁面
                //獲取使用者登錄中的資訊
                string loginName = Request["email"];
                string password = Request["password"];

                //把使用者的資訊儲存在session中
                Session[LoginUserKey] = loginMember.Email;

                //獲取該頁面url的參數資訊
                string returnURL = Request.Params["HTTP_REFERER"];
                int index = returnURL.IndexOf('=');
                returnURL = returnURL.Substring(index + 1);

                //如果參數為空，則跳轉到首頁，否則切回原頁面
                if (string.IsNullOrEmpty(returnURL))
                    return Redirect("/Home/HomePage");
                else
                    return Redirect(returnURL);
            }
            else
            {
                //用TempData儲存登入訊息
                TempData["LoginState"] = ValidateStr;
                //重新導向頁面
                return RedirectToAction("LoginResult");
            }

        }

        //[Authorize]
        //登入結果
        public ActionResult LoginResult()
        {            
            return View();
        }






        //Get:Members/Register
        //[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //Post:Members/Register
        //[AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberRegisterViewModel newMember)
        {
            if (ModelState.IsValid)
            {
                //取得信箱驗證碼
                string AuthCode = _MailService.GetValidateCode();

                //註冊成為新會員
                var member = _MemberService.MemberRigisterData()
                            .Where(m => m.Email == newMember.Email)
                            .FirstOrDefault();
                if (member == null)
                {
                    //將密碼Hash
                    newMember.Password = _MemberService.HashPassword(newMember.Password);

                    //GroupViewModel -> DM
                    Members emp = new Members
                    {
                        Email = newMember.Email,
                        Password = newMember.Password,
                        AuthCode = AuthCode,
                        LineStatusId = 1
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();
                }
                //與第三方登入判斷
                else if (member != null && (member.GoogleId !="" || member.FBId != "" || member.LineId != ""))
                {
                    //將密碼Hash
                    newMember.Password = _MemberService.HashPassword(newMember.Password);
                    member.Password = newMember.Password;
                    member.AuthCode = AuthCode;
                    db.SaveChanges();
                }
                else
                {
                    //用TempData儲存註冊訊息
                    TempData["RegisterState"] = "此帳號己有人使用，請重新註冊";
                    //重新導向頁面
                    return RedirectToAction("RegisterResult");
                }


                string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));

                UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidate", "Members", new{
                        Email = newMember.Email,
                        AuthCode = AuthCode
                    })
                };

                string MailBody = _MailService.GetRegisterMailBody(TempMail, newMember.Email, ValidateUrl.ToString().Replace("%3F", "?"));

                _MailService.SendRegisterMail(MailBody, newMember.Email);

                //用TempData儲存註冊訊息
                TempData["RegisterState"] = "註冊成功，請到註冊信箱進行驗證";
                //重新導向頁面
                return RedirectToAction("RegisterResult");

            }

            //未經驗證清空密碼相關欄位
            newMember.Password = null;

            //獲取使用者登錄中的資訊
            string loginName = Request["email"];
            string password = Request["password"];

            //獲取該頁面url的參數資訊
            string returnURL = Request.Params["HTTP_REFERER"];
            int index = returnURL.IndexOf('=');
            returnURL = returnURL.Substring(index + 1);

            //如果參數為空，則跳轉到首頁，否則切回原頁面
            if (string.IsNullOrEmpty(returnURL))
                return Redirect("/Home/HomePage");
            else
                return Redirect(returnURL);

        }

        //註冊結果
        public ActionResult RegisterResult()
        {
            return View();
        }

        //接收驗證信連結傳進來的Action
        public ActionResult EmailValidate(string Email, string AuthCode)
        {
            ViewData["EmailValidate"] = _MemberService.EmailValidate(Email, AuthCode);
            return RedirectToAction("ePal", "ePal");
        }




        //Privacy Policy
        public ActionResult PrivacyPolicy()
        {
            return View();
        }




















        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.CityId).Include(m => m.Language);
            return View(members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName");
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "LanguageName");
            return View();
        }

        // POST: Members/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,MemberName,RegistrationDate,Email,Password,Phone,Country,CityId,Gender,BirthDay,TimeZone,LanguageId,Bio,ProfilePicture,LineStatus")] Members member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", member.CityId);
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "LanguageName", member.LanguageId);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", member.CityId);
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "LanguageName", member.LanguageId);
            return View(member);
        }

        // POST: Members/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,MemberName,RegistrationDate,Email,Password,Phone,Country,CityId,Gender,BirthDay,TimeZone,LanguageId,Bio,ProfilePicture,LineStatus")] Members member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", member.CityId);
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "LanguageName", member.LanguageId);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
