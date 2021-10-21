using System;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;

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

        public Orders CreateUnpaidOrder(DetailViewModel AddCartVM, string startTime, int id)
        {
            //var cart = AddCartVM;
            var timeNow = DateTime.Now;
            var utcTimeNow = timeNow.ToUniversalTime();
            var timestamp = UtcDateTimeToUnix(utcTimeNow);
            var customerId = Int32.Parse(GetCustomerIdService.GetMemberId());
            var formattedTimestamp = $"GLHF-{customerId}{timestamp}";
            Orders order = new Orders()
            {
                CustomerId = customerId,
                ProductId = id,
                Quantity = AddCartVM.Rounds,
                UnitPrice = (decimal)AddCartVM.UnitPrice,
                OrderDate = utcTimeNow,
                DesiredStartTime = Convert.ToDateTime(startTime),
                OrderStatusId = 1,
                OrderConfirmation = formattedTimestamp
            };
            return order;
        }

        public bool AddCartSuccess(Orders unpaid)
        {
            using (var tran = _repo._context.Database.BeginTransaction())
            {
                try
                {
                    _repo.Create(unpaid);
                    _repo.SaveChanges();
                    tran.Commit();
                    var confirmation = unpaid.OrderConfirmation;
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


        static long UtcDateTimeToUnix(DateTime x)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long result = (x.ToUniversalTime() - unixStart).Ticks;
            return result;
        }
        static DateTime UnixToDateTime(long datestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(datestamp).DateTime;
            return result;
        }
        static DateTime UnixToLocalDateTime(long datestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(datestamp).DateTime.ToLocalTime();
            return result;
        }

    }
}