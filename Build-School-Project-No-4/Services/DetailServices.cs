using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;

namespace Build_School_Project_No_4.Services
{
    public class DetailServices
    {
        private readonly Repository _repo;
        public DetailServices()
        {
            _repo = new Repository();
        }
        public DetailViewModel FindPlayerListing(int? id)
        {
            var playerData = new DetailViewModel();


            var products = _repo.GetAll<Products>();
            var gameInfo = _repo.GetAll<GameCategories>();
            var playerInfo = _repo.GetAll<Members>();
            var productServer = _repo.GetAll<ProductServer>();
            var server = _repo.GetAll<Server>();
            var rank = _repo.GetAll<Rank>();
            var lang = _repo.GetAll<Language>();

            if (id == null)
            {
                return null ;
            }

            var result = (from p in products
                          join pl in playerInfo on p.CreatorId equals pl.MemberId
                          join g in gameInfo on p.GameCategoryId equals g.GameCategoryId
                          join pserv in productServer on p.ProductId equals pserv.ProductId
                          join serv in server on pserv.ServerId equals serv.ServerId
                          join r in rank on p.RankId equals r.RankId
                          join l in lang on pl.LanguageId equals l.LanguageId
                          where p.ProductId == id
                          select new DetailViewModel
                          {
                              MemberName = pl.MemberName,
                              UnitPrice = (double)p.UnitPrice,
                              Recording = p.RecommendationVoice,
                              Intro = p.Introduction,
                              PlayerPic = p.CreatorImg,
                              GameScreenshot = p.ProductImg,
                              GameName = g.GameName,
                              GameBackdrop = g.GameCoverImg,
                              ServerName = serv.ServerName,
                              RankName = r.RankName,
                              LanguageName = l.LanguageName,
                              PlayerId = p.ProductId,
                              MobileGameImg = g.GameCoverImgMini
                              

                              
                          }).SingleOrDefault();

            return result;
        }

    }
}