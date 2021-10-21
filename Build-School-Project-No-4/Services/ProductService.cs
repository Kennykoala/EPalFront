using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
   public class ProductService
   {
        private readonly Repository _repo;

        public ProductService()
        {
            _repo = new Repository();
        }

        public CategoryViewModel GetGamesAllAndDeatils(int categoryId)
        {
            var result = new CategoryViewModel();
            var category = _repo.GetAll<GameCategories>().FirstOrDefault(x => x.GameCategoryId == categoryId);
            if (category == null)
            {
                return result;
            }
            var GameTable = _repo.GetAll<GameCategories>().ToList();
            var GameAll = GameTable.Select(g => new Game
            {
                GameId = g.GameCategoryId,
                GameName = g.GameName
            }).ToList();
            result.GameAll = GameAll;
            result.GameCoverImg = category.GameCoverImg;
            result.GameTitle = category.GameName;
            return result;
        }

        public string GetProductCardsJson(int categoryId)
        {

            var result = new ProductViewModel()
            {
                ProductCards = new List<ProductCard>()
            };
            var category = _repo.GetAll<GameCategories>().FirstOrDefault(x => x.GameCategoryId == categoryId);
            if (category == null)
            {
                return result.ToString();
            }
            var products = _repo.GetAll<Products>().Where(x => x.GameCategoryId == category.GameCategoryId).ToList();
            var CommentDetails = _repo.GetAll<CommentDetails>().ToList();
            var ProductPositions = _repo.GetAll<ProductPosition>().ToList();
            var Positions = _repo.GetAll<Position>().ToList();
            var Ranks = _repo.GetAll<Rank>().ToList();
            var Members = _repo.GetAll<Members>().ToList();
            var ProductServers = _repo.GetAll<ProductServer>().ToList();
            var Servers = _repo.GetAll<Server>().ToList();
            var LineStatus = _repo.GetAll<LineStatus>().ToList();
            var LanguageName = _repo.GetAll<Language>().ToList();
            var todayYear = DateTime.Now.Year;
            var productCards = products.Select(p => new ProductCard
            {
                UnitPrice = p.UnitPrice,
                CreatorImg = p.CreatorImg,
                Introduction = p.Introduction,
                RecommendationVoice = p.RecommendationVoice,
                LineStatus = LineStatus.First(y => y.LineStatusId == (Members.First(x => x.MemberId == p.CreatorId).LineStatusId)).LineStatusImg,
                CreatorName = Members.First(x => x.MemberId == p.CreatorId).MemberName,
               // StarLevel = CommentDetails.FirstOrDefault(x => x.ProductId == p.ProductId).StarLevel,
                Rank = Ranks.FirstOrDefault(x => x.RankId == p.RankId) == null ? "No Rank" : Ranks.First(x => x.RankId == p.RankId).RankName,
                Position = Positions.First(y => y.PositionId == (ProductPositions.FirstOrDefault(x => x.ProductId == p.ProductId).PositionId)).PositionName,
                ProductId = p.ProductId,
            }).ToList();
            result.CategoryId = categoryId;
            result.ProductCards = productCards;
            return JsonConvert.SerializeObject(result);
        }

      
    }
    
}