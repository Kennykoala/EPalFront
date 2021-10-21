using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class ApiFilterService
    {
        private readonly Repository _repo;

        public ApiFilterService()
        {
            _repo = new Repository();
        }

        public string GetProductCardsByFilter(FilterItemViewModel FilterVM)
        {
            var result = new ProductViewModel()
            {
                ProductCards = new List<ProductCard>()
            };

            var server = FilterVM.Server;
            var language = FilterVM.Language;
            var level = FilterVM.Level;
            var highAge = FilterVM.HighAge;
            var lowAge = FilterVM.LowAge;
            var highPrice = FilterVM.HighPrice;
            var lowPrice = FilterVM.LowPrice;
            var gender = FilterVM.Gender;
            var status = FilterVM.Status;
            var categoryId = FilterVM.CategoryId;

            var category = _repo.GetAll<GameCategories>().FirstOrDefault(x => x.GameCategoryId == categoryId);
            var Members = _repo.GetAll<Members>();
            var LineStatus = _repo.GetAll<LineStatus>();
            var CommentDetails = _repo.GetAll<CommentDetails>();
            var ProductPositions = _repo.GetAll<ProductPosition>();
            var Positions = _repo.GetAll<Position>();
            var Ranks = _repo.GetAll<Rank>();
            var ProductServers = _repo.GetAll<ProductServer>();
            var Servers = _repo.GetAll<Server>();
            var Language = _repo.GetAll<Language>();
            var thisYear = DateTime.Now.Year;
            var productsx = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();

            if (category == null)
            {
                return result.ToString();
            }
            var products = _repo.GetAll<Products>().Where(x => x.GameCategoryId == category.GameCategoryId);
            
            if(!string.IsNullOrEmpty(status))
            {
                products = products.Where(x => x.Members.LineStatus.LineStatusName == status);
            }

            if(server != null && server.Count > 0)
            {
                ProductServers = ProductServers.Where(x => server.Contains(x.Server.ServerName));
                var temp = ProductServers.Select(x => x.Products).Distinct();
                products = products.Intersect(temp);
            }

            if(language != null && language.Count > 0)
            {
                products = products.Where(x =>language.Contains(x.Members.Language.LanguageName));
            }

            if (level != null && level.Count >0)
            {
                products = products.Where(x => level.Contains(x.Rank.RankName));
            }
            
            if(!string.IsNullOrEmpty(lowAge))
            {
                int lowage = thisYear - int.Parse(lowAge);
                products = products.Where(x => x.Members.BirthDay.Value.Year <= lowage);
            }

            if(!string.IsNullOrEmpty(highAge))
            {
                int highage = thisYear - int.Parse(highAge);
                products = products.Where(x => x.Members.BirthDay.Value.Year >= highage);
            }

            if(!string.IsNullOrEmpty(lowPrice))
            {
                decimal lowprice = decimal.Parse(lowPrice);
                products = products.Where(x => x.UnitPrice >= lowprice);
            }

            if(!string.IsNullOrEmpty(highPrice))
            {
                decimal highprice = decimal.Parse(highPrice);
                products = products.Where(x => x.UnitPrice <= highprice);
            }
            if (!string.IsNullOrEmpty(gender))
            {
                if(gender == "Male")
                {
                    products = products.Where(x => x.Members.Gender == 1);
                }
                if(gender == "Female")
                {
                    products = products.Where(x => x.Members.Gender == 0);
                }
                else
                {
                    products = products.Select(x => x);
                }
                
            }
            var productCards = products.Select(p => new ProductCard
            {
                UnitPrice = p.UnitPrice,
                CreatorImg = p.CreatorImg,
                Introduction = p.Introduction,
                RecommendationVoice = p.RecommendationVoice,
                LineStatus = LineStatus.FirstOrDefault(y => y.LineStatusId == Members.FirstOrDefault(x => x.MemberId == p.CreatorId).LineStatusId).LineStatusImg,
                CreatorName = Members.FirstOrDefault(x => x.MemberId == p.CreatorId).MemberName,
                Rank = Ranks.FirstOrDefault(x => x.RankId == p.RankId) == null ? "No Rank" : Ranks.FirstOrDefault(x => x.RankId == p.RankId).RankName,
                Position = Positions.FirstOrDefault(y => y.PositionId == (ProductPositions.FirstOrDefault(x => x.ProductId == p.ProductId).PositionId)).PositionName,
                ProductId = p.ProductId,
            }).ToList();
            result.ProductCards = productCards;
            result.CategoryId = categoryId;
           
            return JsonConvert.SerializeObject(result);
        }
    }
}