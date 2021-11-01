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
            var products = _repo.GetAll<Products>().Where(x => x.GameCategoryId == category.GameCategoryId);
            var ProductPositions = _repo.GetAll<ProductPosition>();
            var Positions = _repo.GetAll<Position>();

            var productCards = products.Select(p => new ProductCard
            {
                UnitPrice = p.UnitPrice,
                CreatorImg = p.CreatorImg,
                Introduction = p.Introduction,
                RecommendationVoice = p.RecommendationVoice,
                LineStatus = p.Members.LineStatus.LineStatusImg,
                CreatorName = p.Members.MemberName,
                Rank = p.Rank.RankName,
                Position = Positions.FirstOrDefault(y => y.PositionId == (ProductPositions.FirstOrDefault(x => x.ProductId == p.ProductId).PositionId)).PositionName,
                ProductId = p.ProductId,
            }).ToList();
            result.CategoryId = categoryId;
            result.ProductCards = productCards;
            return JsonConvert.SerializeObject(result);
        }

      
    }
    
}