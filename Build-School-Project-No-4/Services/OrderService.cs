using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;

namespace Build_School_Project_No_4.Services
{
    public class OrderService
    {

        private readonly Repository _repo;

        public OrderService()
        {
            _repo = new Repository();
        }

        public OrderViewModel GetOrderCardData(int OrderStatusId,int mems)
        {
            var result = new OrderViewModel()
            {
              OrderCards = new List<OrderCard>(),
               Order = new List<Orderstatusall>()
            };
            var category = _repo.GetAll<OrderStatus>().FirstOrDefault(x => x.OrderStatusId == OrderStatusId);
            if(category == null)
            {
                return result;
            }
            var orders = _repo.GetAll<Orders>().Where(x => x.OrderStatusId == category.OrderStatusId && x.CustomerId== mems);
            var GameCat = _repo.GetAll<GameCategories>().ToList();
            var orderstatu = _repo.GetAll<OrderStatus>().ToList();
            var products = _repo.GetAll<Products>().ToList();
            var members = _repo.GetAll<Members>().ToList();
            
            var OrderCards = orders.Select(o => new OrderCard
            {
                OrderStatusName = category.OrderStatusName,
                Quantity = o.Quantity,
                OrderDate = o.OrderDate,
                TotalPrice = o.UnitPrice * o.Quantity,
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                GameName = o.Products.GameCategories.GameName,
                MemberName=o.Members.MemberName,
                //ProfilePicture=o.Members.ProfilePicture
                ProfilePicture = o.Products.Members.ProfilePicture
              
                //ProductId =o.Products.ProductId
                //GameName=GameCat.FirstOrDefault(y=>y.GameCategoryId ==(products.FirstOrDefault(x=>x.ProductId==o.ProductId).GameCategoryId)).GameName

            }).ToList();

            var Orders = orderstatu.Select(g => new Orderstatusall
            {
                Id = g.OrderStatusId,
                Name = g.OrderStatusName
            }).ToList();
            result.Order = Orders;
            result.OrderCards = OrderCards;
            result.OrderStatusId = OrderStatusId;
            result.Title = category.OrderStatusName;
            return result;

        }

        ////private OrderRepository _OrderRepo;
        //private readonly Repository _Repo;
        //public OrderService()
        //{
        //    //_OrderRepo = new OrderRepository();
        //    _Repo = new Repository();
        //}
        ////controller  or  service取得登入者的memberId

        ////public string GetMemberId()
        ////{
        ////    var cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

        ////    string userid = "";
        ////    if (cookie != null)
        ////    {
        ////        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
        ////        userid = ticket.UserData;
        ////        return userid;
        ////    }
        ////    return null;
        ////}

        //public List<OrderViewModel> Order()
        //{
        //    List<Orders> Orders = _Repo.GetAll<Orders>().ToList();
        //    List<OrderStatus> OrderStatus = _Repo.GetAll<OrderStatus>().ToList();

        //    List<OrderViewModel> result = new List<OrderViewModel>();

        //    //var result = (from o in Orders
        //    //              join os in OrderStatus on o.OrderStatusId equals os.OrderStatusId)
        //    foreach (var item in Orders)
        //    {
        //        result.Add(new OrderViewModel
        //        {
        //            //OrderStatus = item.OrderStatus,
        //            OrderId = item.OrderId,
        //            PlayerId = item.CustomerId,
        //            OrderDate = item.OrderDate,
        //            Quantity = item.Quantity,
        //            UnitPrice = item.UnitPrice,
        //            TotalPrice = item.Quantity * item.UnitPrice
        //        });
        //    }
        //    return result;
        //}

    }
}