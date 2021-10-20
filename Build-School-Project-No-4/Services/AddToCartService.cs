using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Repositories;

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

        public Orders CreateUnpaidOrder(GroupViewModel AddCartVM, string startTime, int id)
        {
            var cart = AddCartVM.AddCart;
            var timeNow = DateTime.Now;
            var utcTimeNow = timeNow.ToUniversalTime();
            var timestamp = UtcDateTimeToUnix(utcTimeNow);
            var dummyCustomerId = 1;
            var formattedTimestamp = $"GLHF-{dummyCustomerId}{timestamp}";
            Orders order = new Orders()
            {
                CustomerId = dummyCustomerId,
                ProductId = id,
                Quantity = cart.Rounds,
                UnitPrice = cart.UnitPrice,
                OrderDate = utcTimeNow,
                DesiredStartTime = Convert.ToDateTime(startTime),
                OrderStatusId = 1,
                OrderConfirmation = formattedTimestamp
            };
            return order;
        }

        public bool AddCartSuccess(Orders unpaid)
        {
            using (var tran = _ctx.Database.BeginTransaction())
            {
                try
                {
                    _ctx.Orders.Add(unpaid);
                    _ctx.SaveChanges();
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