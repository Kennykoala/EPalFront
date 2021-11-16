using System.Linq;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;
using System;
using Build_School_Project_No_4.Utilities;

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
                              OrderDateTime = o.OrderDate,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              PlayerId = p.CreatorId,
                              PlayerName = m.MemberName,
                              GameName = g.GameName,
                              PlayerPic = p.CreatorImg,
                              ProductId = p.ProductId,
                              OrderStatus = (int)o.OrderStatusId
                          }).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public CheckoutViewModel GetOrderDateTime(string confirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCategories = _repo.GetAll<GameCategories>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join m in members on p.CreatorId equals m.MemberId
                          join g in gameCategories on p.GameCategoryId equals g.GameCategoryId
                          where o.OrderConfirmation == confirmation
                          select new CheckoutViewModel
                          {
                              OrderDateTime = o.OrderDate,
                          }).SingleOrDefault();
            return result;
        }
        public bool CanCheckout(string confirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var order = GetOrderDateTime(confirmation);
            var dateTimeNow = DateTime.UtcNow;
            var timeOut = order.OrderDateTime.AddMinutes(15);
            if (dateTimeNow > timeOut)
            {
                var orderResult = orders.Where(x => x.OrderConfirmation == confirmation).FirstOrDefault();
                orderResult.OrderStatusId = (int)Enums.PaymentStatus.Cancelled;
                orderResult.OrderStatusIdCreator = (int)Enums.PaymentStatus.Cancelled;
                _repo.Update(orderResult);
                _repo.SaveChanges();
                return false; 
            }
           return true;
        }
        public CheckoutViewModel GetPlayerIdFromConfirmation(string orderConfirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var products = _repo.GetAll<Products>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          where o.OrderConfirmation == orderConfirmation
                          select new CheckoutViewModel
                          {
                              ProductId = p.ProductId,
                              OrderConfirmation = o.OrderConfirmation
                          }).SingleOrDefault();

            return result;
        }
        public int GetOrderIdFromConfirmation(string orderConfirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var result = (from o in orders
                          where o.OrderConfirmation == orderConfirmation
                          select o.OrderId).FirstOrDefault();
            return result;        
        }
        public void CreateTransaction(string orderConfirmation, int payMethod, string orderUID)
        {
            try
            {
                var orderIds = GetOrderIdFromConfirmation(orderConfirmation);
                Payments payment = new Payments()
                {
                    TransactionUID = orderUID,
                    TransationDateTime = DateTime.Now.ToUniversalTime(),
                    OrderId = orderIds,
                    PayMethod = payMethod
                };
                _repo.Create(payment);
                _repo.SaveChanges();
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
            }
        }
        public int GetOrderStatus(string orderConfirmation)
        {
            var orders = _repo.GetAll<Orders>();
            return (int)(from o in orders
                          where o.OrderConfirmation == orderConfirmation
                          select o.OrderStatusId).SingleOrDefault();
        }
    }
}