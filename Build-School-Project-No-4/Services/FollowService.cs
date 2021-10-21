using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;

namespace Build_School_Project_No_4.Services
{
    public class FollowService
    {
        private readonly Repository _Repo;

        public FollowService()
        {
            _Repo = new Repository();
        }

        //取得Follow對應的會員資料
        public List<FollowViewModel> GetMemberFollow()
        {
            var followings = _Repo.GetAll<Followings>();
            var members = _Repo.GetAll<Members>();

            //demoId
            int ownId = 60;
            var ownfollow = followings.Where(x => x.MemberId == ownId);

            List<FollowViewModel> result = new List<FollowViewModel>();
            foreach (var item in ownfollow)
            {
                var y = members.First(x => x.MemberId == item.FollowingId);

                var m = new FollowViewModel
                {
                    MemberId = y.MemberId,
                    MemberName = y.MemberName,
                    ProfilePicture = y.ProfilePicture,
                    Gender = y.Gender

                };
                result.Add(m);

            }
            return result;
        }

        //取得Follow自己對應的會員資料
        public List<FollowViewModel> GetMemberFollowers()
        {
            var followings = _Repo.GetAll<Followings>();
            var members = _Repo.GetAll<Members>();

            //demoId
            int ownId = 60;
            var followers = followings.Where(x => x.FollowingId == ownId);

            List<FollowViewModel> result = new List<FollowViewModel>();
            foreach (var item in followers)
            {
                var y = members.First(x => x.MemberId == item.MemberId);

                var m = new FollowViewModel
                {
                    MemberId = y.MemberId,
                    MemberName = y.MemberName,
                    ProfilePicture = y.ProfilePicture,
                    Gender = y.Gender

                };
                result.Add(m);

            }
            return result;
        }

    }
}