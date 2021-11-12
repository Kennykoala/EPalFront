using System;
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
        public ActionResult profile(int? id)
        {

            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var profileNoId = db.Members.Find(id);

            if (profileNoId == null)
            {
                return HttpNotFound();
            }
  
            try
            {
                var profiles = new ProfileEpalService();
                var profileGetAll = profiles.GetProfiles(id);

 
                return View(profileGetAll);

            }
            catch(Exception ex)
            {
                return Content("失敗:" + ex.ToString());
            }           

        }

        public ActionResult Followings()
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


            var memberGet = new FollowService();
            var members = memberGet.GetMemberFollow(memberId);
            var Followers = memberGet.GetMemberFollowers(memberId);



            ViewBag.Followings = members;
            ViewBag.Followers = Followers;

            return View();
        }




        [HttpGet]
        public ActionResult ForgetPwd()
        {
            return View();
        }

        /// <summary>
        /// 忘記密碼寄送驗證碼
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public ActionResult SendMailToken(SendMailTokenIn inModel)
        {
            SendMailTokenOut outModel = new SendMailTokenOut();

            // 檢查輸入來源
            if (string.IsNullOrEmpty(inModel.Email))
            {
                outModel.ErrMsg = "Please enter registered email";
                return Json(outModel);
            }

            //取得信箱驗證碼
            string AuthCode = string.Empty;

            //確認是否有此會員
            var member = _MemberService.MemberRigisterData()
                        .Where(m => m.Email == inModel.Email)
                        .FirstOrDefault();

            //完全未註冊過，或已透過第三方註冊但系統仍未註冊
            if (member == null || (member != null && member.IsAdmin == false))
            {
                outModel.ErrMsg = "Please register EPal website first";
            }
            else
            {
                //取得信箱驗證碼
                AuthCode = _MailService.GetValidateCode();

                string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/ForgetPwdEmailTemplate.html"));

                UriBuilder ValidateUrlForgetPwd = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("ResetPwd", "Members", new
                    {
                        Email = inModel.Email,
                        AuthCode = AuthCode
                    })
                };

                string MailBody = _MailService.GetForgetPwdMailBody(TempMail, inModel.Email, ValidateUrlForgetPwd.ToString().Replace("%3F", "?"));
                _MailService.SendForgetPwdMail(MailBody, inModel.Email);
                outModel.ResultMsg = "Please click on validate email and return to EPal website";
            }
            // 回傳 Json 給前端
            return Json(outModel);
        }






        // GET: 重設密碼頁面
        public ActionResult ResetPwd(string Email, string AuthCode)
        {
            ViewData["EmailValidate"] = _MemberService.EmailValidateforgetpwd(Email, AuthCode);
            // 驗證碼檢查成功，加入 Session
            Session["ResetPwdUserId"] = Email;
            return View();
        }


        /// <summary>
        /// 忘記密碼重設密碼
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public ActionResult DoResetPwd(DoResetPwdIn inModel)
        {
            DoResetPwdOut outModel = new DoResetPwdOut();

            // 檢查是否有輸入密碼
            if (string.IsNullOrEmpty(inModel.NewUserPwd))
            {
                outModel.ErrMsg = "Please enter new password";
                return Json(outModel);
            }
            if (string.IsNullOrEmpty(inModel.CheckUserPwd))
            {
                outModel.ErrMsg = "Please check new password";
                return Json(outModel);
            }
            if (inModel.NewUserPwd != inModel.CheckUserPwd)
            {
                outModel.ErrMsg = "new password is different from checked password";
                return Json(outModel);
            }

            // 檢查帳號 Session 是否存在
            if (Session["ResetPwdUserId"] == null || Session["ResetPwdUserId"].ToString() == "")
            {
                outModel.ErrMsg = "This email doesn't exist";
                return Json(outModel);
            }

            //將密碼Hash
            inModel.NewUserPwd = _MemberService.HashPassword(inModel.NewUserPwd);
            var email = Session["ResetPwdUserId"].ToString();

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var memberdata = db.Members.First(x => x.Email == email);
                    memberdata.Password = inModel.NewUserPwd;
                    db.SaveChanges();
                    tran.Commit();

                    outModel.ResultMsg = "reset password seccess";
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    outModel.ErrMsg = "reset password failure";
                }
            }
            // 回傳 Json 給前端
            return Json(outModel);
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
                return Content("未取得登入授權");
            }

            //從Code取回token
            var token = Utility.GetTokenFromCode(code,
                "1656564684",  //client_id
                "2af2ca5d39971c612d2a2dbccfdd2e54", 
                "https://epal-frontstage.azurewebsites.net/Members/LineLoginCallback");  

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

            linename = user.displayName;
            lineId = user.userId;

            string msg = "ok";
            if (msg == "ok" && lineemail != null)
            {
                //確認是否已註冊
                var memberRVM = _MemberService.MemberRigisterData()
                            .Where(m => m.Email == lineemail)
                            .FirstOrDefault();
                //完全沒註冊過
                if (memberRVM == null)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    string rndnumber = rnd.Next(0, 100).ToString();
                    //將密碼Hash
                    rndnumber = _MemberService.HashPassword(rndnumber);

                    //VM -> DM
                    Members emp = new Members
                    {
                        Email = lineemail,
                        Password = rndnumber,
                        LoginMethod = 3,
                        LineId = lineId
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        LineId = lineId,
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
                else
                {
                    var memberdata = db.Members.First(x => x.Email == lineemail);
                    memberdata.LineId = lineId;
                    memberdata.LoginMethod = 3;
                    //db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();

                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        LineId = lineId,
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
                TempData["message"] = "Welcome to Epal";
                return Redirect("/");
            }
            msg = "error";
            return Content(msg);  

        }





        //FB  login
        [HttpPost]
        public ActionResult FBLogin(string Fbemail, string Fbname, string FBId)
        {
            string msg = "ok";
            string email;
            string fullname;
            string fbid;

            if (msg == "ok" && Fbemail != null)
            {
                email = Fbemail;
                fullname = Fbname;
                fbid = FBId;
                //確認是否已註冊FB
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
                        LoginMethod = 2,
                        FBId = fbid
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        FBId = fbid,
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
                else
                {
                    var memberdata = db.Members.First(x => x.Email == email);
                    memberdata.FBId = fbid;
                    memberdata.LoginMethod = 2;
                    //db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();

                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        FBId = fbid,
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



        //google login
        [HttpPost]
        public async Task<ActionResult> GoogleLogin(string id_token)
        {
            string msg = "ok";
            string email;
            string fullname;
            string googleid;
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(id_token, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { "1025795679023-8g9j439beq7h92iv9us8nj3d77ifitr7.apps.googleusercontent.com" }//要驗證的client id
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
                googleid = payload.JwtId;
                //確認是否已註冊google
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
                        LoginMethod = 1,
                        GoogleId = googleid
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        GoogleId = googleid,
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
                else
                {
                    var memberdata = db.Members.First(x => x.Email == email);
                    memberdata.GoogleId = googleid;
                    memberdata.LoginMethod = 1;
                    //db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();

                    Members meminfo = new Members()
                    {
                        MemberId = memberRVM.MemberId,
                        GoogleId = googleid,
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

                    return Content("update linestatus success");
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    return Content("update linestatus failure:" + ex.ToString());
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
        public ActionResult EditProfile(MemberInfoViewModel EditMember)
        {   
            //密碼Hash
            EditMember.Password = _MemberService.HashPassword(EditMember.Password);

            //MemberInfoViewModel -> DM
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

                        TempData["message"] = "success";
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        TempData["msg"] = "failure";
                    }
                }

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(EditMember);
        }





        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("/Home/HomePage");

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

                Members meminfo = new Members()
                {
                    MemberId = user.MemberId,
                    MemberName = user.MemberName,
                    ProfilePicture = user.ProfilePicture,
                    LoginMethod = 0
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

                //完全未註冊過
                if (member == null)
                {
                    //將密碼Hash
                    newMember.Password = _MemberService.HashPassword(newMember.Password);

                    //VM -> DM
                    Members emp = new Members
                    {
                        Email = newMember.Email,
                        Password = newMember.Password,
                        AuthCode = AuthCode,
                        LineStatusId = 1,
                        IsAdmin = true
                    };
                    db.Members.Add(emp);
                    db.SaveChanges();


                    string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
                    UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                    {
                        Path = Url.Action("EmailValidate", "Members", new
                        {
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
                //已透過第三方登入註冊過
                //else if (member != null && (member.GoogleId != "" || member.FBId != "" || member.LineId != ""))
                else if (member.IsAdmin == false && (member.GoogleId != "" || member.FBId != "" || member.LineId != ""))
                {
                    _MemberService.UpdateThirdpartyRegister(newMember, AuthCode);

                    string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
                    UriBuilder ValidateUrl = new UriBuilder(Request.Url)
                    {
                        Path = Url.Action("EmailValidate", "Members", new
                        {
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
                else
                {
                    //用TempData儲存註冊訊息
                    TempData["RegisterState"] = "此帳號己有人使用，請重新註冊";
                    //重新導向頁面
                    return RedirectToAction("RegisterResult");
                }


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
            //TempData["valmsg"] = "valiOK";
            return View();
            //return Redirect("/Home/HomePage");
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
