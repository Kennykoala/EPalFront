using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class ProfileEpalService
    {
        public readonly Repository _Repo;

        public ProfileEpalService()
        {
            _Repo = new Repository();
        }




        public List<ProfileViewModel> GetProfiles(int? assignMemberId)
        {

            var memberAll = _Repo.GetAll<Members>();
            var followAll = _Repo.GetAll<Followings>();
            var vistorAll = _Repo.GetAll<RecentVistors>();
            var languages = _Repo.GetAll<Language>();
            var commentAll = _Repo.GetAll<CommentDetails>();
            var products = _Repo.GetAll<Products>();

            var selectMemberLanguage = memberAll.Where(x => x.MemberId == assignMemberId).FirstOrDefault();
            var selectlanguage = languages.Where(x => x.LanguageId == selectMemberLanguage.LanguageId).FirstOrDefault();
            var selectmemberId = memberAll.Where(x => x.MemberId == assignMemberId).ToList();
            var follingCal = followAll.Where(x => x.MemberId == assignMemberId).ToList().Count();
            var followersCal = followAll.Where(x => x.FollowingId == assignMemberId).ToList().Count();
            var vistorCal = vistorAll.Where(x => x.MemberId == assignMemberId).ToList().Count();
            var serversCal = products.Where(x => x.CreatorId == assignMemberId).ToList().Count();
            var selectRecommend = commentAll.Where(x => x.MemberId == assignMemberId).ToList();
            var RecommendList = commentAll.Where(x => x.MemberId == assignMemberId).ToList();

            string languageNames = selectlanguage.LanguageName;
            int commentCal = selectRecommend.Count;

            //計算評分平均
            decimal avgStar = 0;
            avgStar = starAvgMethod(selectRecommend);

            List<ProfileViewModel> result = new List<ProfileViewModel>();
            foreach (var item in selectmemberId)
            {
                result.Add(new ProfileViewModel
                {
                    MemberId = item.MemberId,
                    MemberName = item.MemberName,
                    Gender = item.Gender,
                    ProfilePicture = item.ProfilePicture,
                    Bio = item.Bio,
                    FollingsNumber = follingCal,
                    FollowsNumber = followersCal,
                    LanguageName = languageNames,
                    VistorsNumber = vistorCal,
                    serverNumber = serversCal,
                    commentNumber = commentCal,
                    StarAvg = avgStar
                });
            }

            return result;
        }

        private static decimal starAvgMethod(List<CommentDetails> selectRecommend)
        {
    
            decimal avgStar = 0;
            decimal y = 0;
            foreach (var item in selectRecommend)
            {
                y = item.StarLevel + y;
            }

            if (y != 0)
            {
                avgStar = y / selectRecommend.Count();
            }
           
            return avgStar;
        }

    }
}