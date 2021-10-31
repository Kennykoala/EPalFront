﻿using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class ChillMeetService
    {
        private readonly Repository _Repo;

        public ChillMeetService()
        {
            _Repo = new Repository();
        }

        public List<ChillMeetViewModel> GetMeetFiles(int? assignMemberId)
        {
            var MeetLikesAll = _Repo.GetAll<MeetLikes>();
            var MembersAll = _Repo.GetAll<Members>();
            var LanguagesAll = _Repo.GetAll<Language>();

            //排除重複的
            var filterIsLikes = MeetLikesAll.Where(x => x.MemberId == assignMemberId);

            List<Members> Memberlikes = new List<Members>();
            foreach (var item in filterIsLikes)
            {
                Memberlikes.Add(new Members { MemberId = item.LikeId });
            }

            var isLike = Memberlikes.Select(x => x.MemberId).ToList();
            var memberAlls = MembersAll.Select(x => x.MemberId).ToList();
            var MemberDislike = memberAlls.Except(isLike);


            List<Members> resultMembers = new List<Members>();
            List<ChillMeetViewModel> result = new List<ChillMeetViewModel>();

            foreach (var dislike in MemberDislike)
            {
                var y = MembersAll.FirstOrDefault(x => x.MemberId == dislike && x.MeetPicture != null && x.MemberId != assignMemberId);

                if (y != null)
                {
                    var mem = new ChillMeetViewModel
                    {
                        MemberId = y.MemberId,
                        MemberName = y.MemberName,
                        Gender = y.Gender,
                        Country = y.Country,
                        MeetPicture = y.MeetPicture,
                        UserId = (int)assignMemberId
                    };
                    result.Add(mem);
                }
            }

            return result;
        }

        //取得LikeID對應的會員資料
        public List<MemberViewModel> GetMemberLike(int ownId)
        {
            var meetLikes = _Repo.GetAll<MeetLikes>();
            var members = _Repo.GetAll<Members>();
            var followings = _Repo.GetAll<Followings>();

            //demoId
            var ownLike = meetLikes.Where(x => x.MemberId == ownId);

            //篩選ownId(Like清單)的followId == memberId
            //var b = followings.Where(x => x.FollowingId)
            bool isFollowData;



            List<MemberViewModel> result = new List<MemberViewModel>();
            foreach (var item in ownLike)
            {
                var y = members.First(x => x.MemberId == item.LikeId);

                //var b = followings.First(x => x.FollowingId == y.MemberId);
                var b = followings.Where(x => x.MemberId == ownId).FirstOrDefault(h => h.FollowingId == y.MemberId);

                if (b != null)
                {
                    isFollowData = true;
                }
                else
                {
                    isFollowData = false;
                }

         
                var m = new MemberViewModel
                {
                    MemberId = y.MemberId,
                    MemberName = y.MemberName,
                    Bio = y.Bio,
                    ProfilePicture = y.ProfilePicture,
                    Gender = y.Gender,
                    UserId = ownId,
                    isFollow = isFollowData
                };
                result.Add(m);

            }
            return result;
        }


        //取得LikeID對應的會員資料
        public List<MemberViewModel> GetMemberMatch()
        {
            var meetLikes = _Repo.GetAll<MeetLikes>();
            var members = _Repo.GetAll<Members>();

            //demoId
            int ownId = 60;
            var ownLike = meetLikes.Where(x => x.MemberId == ownId);

            List<MeetLikes> MatchList = new List<MeetLikes>();
            //onwLike有多個清單
            foreach (var item in ownLike)
            {
                var a = meetLikes.Where(x => x.MemberId == item.LikeId);
                var b = a.FirstOrDefault(x => x.LikeId == ownId);
                if (b != null)
                {
                    MatchList.Add(b);
                }

            }


            List<MemberViewModel> result = new List<MemberViewModel>();
            foreach (var item in MatchList)
            {
                var y = members.First(x => x.MemberId == item.MemberId);

                var m = new MemberViewModel
                {
                    MemberId = y.MemberId,
                    MemberName = y.MemberName,
                    Bio = y.Bio,
                    ProfilePicture = y.ProfilePicture,
                    Gender = y.Gender

                };
                result.Add(m);

            }
            return result;
        }

    }
}