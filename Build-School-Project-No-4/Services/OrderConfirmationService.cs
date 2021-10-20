using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.Repositories;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;

namespace Build_School_Project_No_4.Services
{
    public class OrderConfirmationService
    {
        private readonly Repository _repo;
        private readonly EPalContext _ctx;
        public OrderConfirmationService()
        {
            _repo = new Repository();
            _ctx = new EPalContext();
        }

        public bool UpdateOrderStatus(string confirmation)
        {
            var orders = _repo.GetAll<Orders>();

            if (confirmation == null)
            {
                return false;
            }

            using (var tran = _repo._context.Database.BeginTransaction())
            {
                try
                {
                    var result = orders.Where(x => x.OrderConfirmation == confirmation).FirstOrDefault();
                    result.OrderStatusId = 4;
                    //result.UpdateDateTime = DateTime.Now.ToUniversalTime();
                    _repo.Update(result);
                    _repo.SaveChanges();
                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    var err = ex.ToString();
                    return false;
                }
            }

        }
        public OrderConfirmationViewModel GetConfirmationInfo(string confirmationNum)
        {
            var orders = _repo.GetAll<Orders>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var members = _repo.GetAll<Members>();

            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join g in gameCat on p.GameCategoryId equals g.GameCategoryId
                          join m in members on p.CreatorId equals m.MemberId
                          where o.OrderConfirmation == confirmationNum
                          select new OrderConfirmationViewModel
                          {
                              OrderConfirmation = confirmationNum,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              PlayerName = m.MemberName,
                              GameName = g.GameName,
                              PlayerPic = p.CreatorImg,
                              StartTime = o.DesiredStartTime

                          }).FirstOrDefault();

            return result;
        }
    }
}