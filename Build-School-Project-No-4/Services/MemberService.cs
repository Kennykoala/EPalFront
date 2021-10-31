using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Web.Security;
using Newtonsoft.Json;

namespace Build_School_Project_No_4.Services
{
    public class MemberService
    {

        private readonly Repository _Repo;
        public MemberService()
        {
            _Repo = new Repository();
        }



        public List<MemberViewModel> GetMember()
        {
            List<Members> members = _Repo.GetAll<Members>().ToList();

            List<MemberViewModel> result = new List<MemberViewModel>();
            foreach (var item in members)
            {
                result.Add(new MemberViewModel{
                    MemberId = item.MemberId,
                    MemberName = item.MemberName,
                    RegistrationDate = item.RegistrationDate,
                    Email = item.Email,
                    Password = item.Password,
                    Phone = item.Phone,
                    Country = item.Country,
                    CityId = item.CityId,
                    Gender = item.Gender,
                    BirthDay = item.BirthDay,
                    TimeZone = item.TimeZone,
                    LanguageId = item.LanguageId,
                    Bio = item.Bio,
                    ProfilePicture = item.ProfilePicture,
                });
            }
            return result;
        }


        public List<MemberRegisterViewModel> MemberRigisterData()
        {
            List<Members> members = _Repo.GetAll<Members>().ToList();

            List<MemberRegisterViewModel> result = new List<MemberRegisterViewModel>();

            foreach (var item in members)
            {
                result.Add(new MemberRegisterViewModel
                {
                    MemberId = item.MemberId,
                    RegistrationDate = item.RegistrationDate,
                    Email = item.Email,
                    Password = item.Password,
                    AuthCode = item.AuthCode
                });
            }
            return result;
        }


        public List<Members> MemberLoginData()
        {
            List<Members> members = _Repo.GetAll<Members>().ToList();

            List<Members> result = new List<Members>();
            foreach (var item in members)
            {
                result.Add(new Members
                {
                    MemberId = item.MemberId,
                    RegistrationDate = item.RegistrationDate,
                    Email = item.Email,
                    Password = item.Password,
                    ProfilePicture = item.ProfilePicture,
                    MemberName = item.MemberName                    
                });
            }
            return result;
        }


        //Hash密碼
        public string HashPassword(string Password)
        {
            string saltkey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            string saltAndPassword = String.Concat(Password, saltkey);
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            byte[] HashDate = sha256Hasher.ComputeHash(PasswordData);
            string Hashresult = Convert.ToBase64String(HashDate);
            return Hashresult;
        }


        //由email取得member單筆資料
        public Members GetDataByAccount(string Email)
        {
            Members Data = new Members();
            try
            {
                var member = _Repo.GetAll<Members>().ToList().Where(m => m.Email == Email).FirstOrDefault();

                Data.MemberId = member.MemberId;
                Data.MemberName = member.MemberName;
                Data.Email = member.Email;
                Data.Password = member.Password;
                Data.AuthCode = member.AuthCode;
                Data.IsAdmin = member.IsAdmin;
                Data.ProfilePicture = member.ProfilePicture;
            }
            catch (Exception ex)
            {
                Data = null;
            }
            return Data;
        }


        //信箱驗證碼
        public string EmailValidate(string Email, string AuthCode)
        {
            Members ValidateMember = GetDataByAccount(Email);

            string ValidateStr = string.Empty;
            if (ValidateMember != null)
            {

                if (ValidateMember.AuthCode == AuthCode)
                {                    
               
                    using(EPalContext _ctx = new EPalContext())
                    {
                        var m = _ctx.Members.ToList().Find(x => x.Email.Equals(Email));
                        try
                        {                            
                            m.AuthCode = string.Empty;
                            _ctx.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message.ToString());
                        }

                        ValidateStr = "註冊信箱驗證成功，請從Log In登入";
                    }
                }
                else
                {
                    ValidateStr = "驗證碼錯誤或已經過認證，請重新確認或再註冊";
                }
            }
            else
            {
                ValidateStr = "傳送資料有誤，請重新確認或再註冊";
            }
            return ValidateStr;
        }


        //登入帳密確認
        public string LoginCheck(string Email, string Password)
        {            
            Members loginMember = GetDataByAccount(Email);
            if (loginMember != null)
            {
                //判斷有經驗證後，驗證碼欄位會被清空
                if (String.IsNullOrWhiteSpace(loginMember.AuthCode))
                {
                    //帳號密碼確認
                    if (PasswordCheck(loginMember, Password))
                    {
                        return "";
                    }
                    else if (loginMember.GoogleId != "" || loginMember.FBId != "" || loginMember.LineId != "")
                    {
                        return "此會員帳號尚未註冊";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                else
                {
                    return "此帳號尚未經過Email驗證，請去收信";
                }
            }
            else
            {
                return "無此會員帳號，請去註冊";
            }
        }

        //密碼確認
        public bool PasswordCheck(Members CheckMember, string Password)
        {
            //判斷資料庫裡的密碼資料與傳入密碼資料Hash後是否一樣
            bool result = CheckMember.Password.Equals(HashPassword(Password));
            return result;
        }



        public MemberInfoViewModel GetEditProfileInfo(int memberid)
        {
            var emp = _Repo.GetAll<Members>().FirstOrDefault(x=> x.MemberId == memberid);

            //Members emp = db.Members.Find(memberid);
            if (emp == null)
            {
                throw new NotImplementedException();
            }

            if (emp.Gender == null)
            {
                emp.Gender = 0;
            }
            if (emp.LanguageId == null)
            {
                emp.LanguageId = 0;
            }

            //DM -> MemberInfoViewModel -> GroupViewModel
            MemberInfoViewModel MemberInfo = new MemberInfoViewModel()
            {
                MemberId = emp.MemberId,
                MemberName = emp.MemberName,
                Phone = emp.Phone,
                Country = emp.Country,
                Gender = (Genders)emp.Gender,
                BirthDay = emp.BirthDay,
                TimeZone = emp.TimeZone,
                LanguageId = (LanguageCategories)emp.LanguageId,
                Bio = emp.Bio,
                Email = emp.Email,
                Password = emp.Password,
                ProfilePicture = emp.ProfilePicture
            };

            return MemberInfo;

        }

        public LinestatusViewModel GetStatus(int memberid)
        {
            var emp = _Repo.GetAll<Members>().FirstOrDefault(x => x.MemberId == memberid);

            //Members emp = db.Members.Find(memberid);
            if (emp == null)
            {
                throw new NotImplementedException();
            }

            //DM -> MemberInfoViewModel 
            LinestatusViewModel MemberInfo = new LinestatusViewModel()
            {
                MemberId = emp.MemberId,
                LineStatusId = emp.LineStatusId
            };

            return MemberInfo;

        }


        public void UpdateThirdpartyRegister(MemberRegisterViewModel newMember, string AuthCode)
        {
            var member = _Repo.GetAll<Members>().FirstOrDefault(m => m.Email == newMember.Email);

            //var member = MemberRigisterData()
            //            .Where(m => m.Email == newMember.Email)
            //            .FirstOrDefault();

            //將密碼Hash
            newMember.Password = HashPassword(newMember.Password);
            member.Password = newMember.Password;
            member.AuthCode = AuthCode;
            member.IsAdmin = true;
            _Repo.Update(member);
            _Repo.SaveChanges();

        }

    }
}