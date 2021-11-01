using System;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Services
{
    public class AddToCartService
    {
        private readonly Repository _repo;
        private readonly EPalContext _ctx;
        public AddToCartService()
        {
            _repo = new Repository();
            _ctx = new EPalContext();
        }

        public Orders CreateUnpaidOrder(AddToCartViewModel AddCartVM, string startTime, int id)
        {
            var utcTimeNow = DateTime.Now.ToUniversalTime();
            var timestamp = PaymentUtil.UtcDateTimeToUnix(utcTimeNow);
            var customerId = Int32.Parse(MemberUtil.GetMemberId());
            var orderId = $"X-{customerId}{timestamp}";
            Orders order = new Orders()
            {
                CustomerId = customerId,
                ProductId = id,
                Quantity = AddCartVM.Rounds,
                UnitPrice = (decimal)AddCartVM.UnitPrice,
                OrderDate = utcTimeNow,
                DesiredStartTime = Convert.ToDateTime(startTime),
                OrderStatusId = (int)Enums.PaymentStatus.Unpaid,
                OrderStatusIdCreator = (int)Enums.PaymentStatus.Unpaid,
                OrderConfirmation = orderId
                
            };
            return order;
        }

        public bool AddCartSuccess(Orders unpaid)
        {
            try
            {
                _repo.Create(unpaid);
                _repo.SaveChanges();
                var confirmation = unpaid.OrderConfirmation;
                return true;
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
                return false;
            }
        }
    }
}