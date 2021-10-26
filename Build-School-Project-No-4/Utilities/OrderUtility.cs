using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;

namespace Build_School_Project_No_4.Utilities
{
    public class OrderUtility
    {
        private readonly Repository _repo;
        public OrderUtility()
        {
            _repo = new Repository();
        }
        public PaymentViewModel GetOrder(string confirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join m in members on p.CreatorId equals m.MemberId
                          join g in gameCat on p.GameCategoryId equals g.GameCategoryId
                          where o.OrderConfirmation == confirmation
                          select new PaymentViewModel
                          {
                              ePalName = m.MemberName,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              GameName = g.GameName,
                              ePalImg = p.CreatorImg
                          }).SingleOrDefault();
            return result;
        }
    }
}