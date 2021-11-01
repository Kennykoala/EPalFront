using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Services
{
    public class OrderPaymentService
    {
        private readonly Repository _repo;
        private readonly EPalContext _ctx;
        private readonly CheckoutService _checkoutService;
        public OrderPaymentService()
        {
            _repo = new Repository();
            _ctx = new EPalContext();
            _checkoutService = new CheckoutService();
        }
        public bool UpdateToUnstarted(string confirmation, string transactionId)
        {
            if (confirmation == null || transactionId == null)
            {
                return false;
            }
            try
            {
                var orders = _repo.GetAll<Orders>();
                var payments = _repo.GetAll<Payments>();
                var orderId = _checkoutService.GetOrderIdFromConfirmation(confirmation);
                var order = orders.Where(x => x.OrderConfirmation == confirmation).FirstOrDefault();
                order.OrderStatusId = (int)Enums.PaymentStatus.NotStarted;
                order.OrderStatusIdCreator = (int)Enums.PaymentStatus.NotStarted;
                _repo.Update(order);
                _repo.SaveChanges();
                var payment = payments.Where(x => x.OrderId == orderId).OrderByDescending(x => x.PaymentId).FirstOrDefault();
                payment.ConfirmationId = transactionId;
                _repo.Update(payment);
                _repo.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
                return false;
            }
        }
        public OrderConfirmationViewModel GetConfirmationInfo(string confirmation)
        {
            var orders = _repo.GetAll<Orders>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var members = _repo.GetAll<Members>();
            var payments = _repo.GetAll<Payments>();

            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join g in gameCat on p.GameCategoryId equals g.GameCategoryId
                          join m in members on p.CreatorId equals m.MemberId
                          join pay in payments on o.OrderId equals pay.OrderId
                          where o.OrderConfirmation == confirmation
                          select new OrderConfirmationViewModel
                          {
                              OrderConfirmation = confirmation,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              PlayerName = m.MemberName,
                              GameName = g.GameName,
                              PlayerPic = p.CreatorImg,
                              StartTime = o.DesiredStartTime,
                              PaymentConfirmation = pay.ConfirmationId
                          }).FirstOrDefault();
            return result;
        }
    }
}