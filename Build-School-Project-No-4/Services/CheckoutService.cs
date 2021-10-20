using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.Repositories;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;

namespace Build_School_Project_No_4.Services
{
    public class CheckoutService
    {
        private readonly Repository _repo;
        public CheckoutService()
        {
            _repo = new Repository();
        }

        public CheckoutViewModel GetCheckoutDetails(string orderConfirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCategories = _repo.GetAll<GameCategories>();

            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join m in members on p.CreatorId equals m.MemberId
                          join g in gameCategories on p.GameCategoryId equals g.GameCategoryId
                          where o.OrderConfirmation == orderConfirmation
                          select new CheckoutViewModel
                          {
                              OrderConfirmation = orderConfirmation,
                              StartTime = o.DesiredStartTime,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              PlayerId = p.CreatorId,
                              PlayerName = m.MemberName,
                              GameName = g.GameName,
                              PlayerPic = p.CreatorImg,
                              ProductId = p.ProductId
                          }).SingleOrDefault();


            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}