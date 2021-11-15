using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Services
{
    public class OrderService
    {

        private readonly Repository _repo;
        private readonly EPalContext _ctx;

        public OrderService()
        {
            _repo = new Repository();
            _ctx = new EPalContext();
        }


        //Purchased Orders
        public OrderViewModel GetOrderCardData(int OrderStatusId, int mems)
        {

            var result = new OrderViewModel()
            {
                OrderCards = new List<OrderCard>(),
                Order = new List<Orderstatusall>()
            };

            //訂單狀態
            var category = _repo.GetAll<OrderStatus>().FirstOrDefault(x => x.OrderStatusId == OrderStatusId);
            if (category == null)
            {
                return result;
            }


            //Purchased Orders
            var orders = _repo.GetAll<Orders>().Where(x => x.OrderStatusId == category.OrderStatusId && x.CustomerId == mems);
            var products = _repo.GetAll<Products>();
            var OrderCards = (from o in orders
                              join p in products on o.ProductId equals p.ProductId
                              select new OrderCard
                              {
                                  OrderStatusName = category.OrderStatusName,
                                  Quantity = o.Quantity,
                                  OrderDate = o.OrderDate,
                                  TotalPrice = o.UnitPrice * o.Quantity,
                                  OrderId = o.OrderId,
                                  ProductId = o.ProductId,
                                  GameName = o.Products.GameCategories.GameName,
                                  MemberName = o.Products.Members.MemberName,
                                  ProfilePicture = o.Products.CreatorImg,
                                  OrderStatusIdCreator = o.OrderStatusIdCreator,
                                  PlayerId = p.CreatorId,
                                  Confirmation = o.OrderConfirmation
                              }).OrderByDescending(x => x.OrderId).ToList();
            //var OrderCards = orders.Select(o => new OrderCard
            //{
            //    OrderStatusName = category.OrderStatusName,
            //    Quantity = o.Quantity,
            //    OrderDate = o.OrderDate,
            //    TotalPrice = o.UnitPrice * o.Quantity,
            //    OrderId = o.OrderId,
            //    ProductId = o.ProductId,
            //    GameName = o.Products.GameCategories.GameName,
            //    MemberName = o.Products.Members.MemberName,
            //    //ProfilePicture=o.Members.ProfilePicture
            //    ProfilePicture = o.Products.Members.ProfilePicture,
            //    OrderStatusIdCreator = o.OrderStatusIdCreator,
            //    //PlayerId = 

            //    //ProductId =o.Products.ProductId
            //    //GameName=GameCat.FirstOrDefault(y=>y.GameCategoryId ==(products.FirstOrDefault(x=>x.ProductId==o.ProductId).GameCategoryId)).GameName

            //}).ToList();


            var orderstatu = _repo.GetAll<OrderStatus>().ToList();
            var Orders = orderstatu.Select(g => new Orderstatusall
            {
                OrderStatusId = g.OrderStatusId,
                Name = g.OrderStatusName
            }).ToList();
            result.Order = Orders;

            result.OrderCards = OrderCards;

            result.OrderStatusId = OrderStatusId;
            result.Title = category.OrderStatusName;

            return result;

        }


        //Created Orders
        public OrderViewModel GetCreatedOrderCardData(int OrderStatusIdCreator, int mems)
        {

            var result = new OrderViewModel()
            {
                CreatedCards = new List<CreatedCard>(),
                Order = new List<Orderstatusall>()
            };

            //訂單狀態
            var category = _repo.GetAll<OrderStatus>().FirstOrDefault(x => x.OrderStatusId == OrderStatusIdCreator);
            if (category == null)
            {
                return result;
            }


            //created orders
            var createorders = _repo.GetAll<Orders>().Where(x => x.OrderStatusIdCreator == category.OrderStatusId && x.Products.CreatorId == mems);
            var CreateCards = createorders.Select(c => new CreatedCard
            {
                OrderStatusName = category.OrderStatusName,
                Quantity = c.Quantity,
                OrderDate = c.OrderDate,
                TotalPrice = c.UnitPrice * c.Quantity,
                OrderId = c.OrderId,
                ProductId = c.ProductId,
                GameName = c.Products.GameCategories.GameName,
                MemberName = c.Members.MemberName,
                PlayerId = c.CustomerId,
                //ProfilePicture=o.Members.ProfilePicture
                ProfilePicture = c.Members.ProfilePicture,

                OrderStatusId = c.OrderStatusId

            }).OrderByDescending(x => x.OrderId).ToList();




            var orderstatu = _repo.GetAll<OrderStatus>().ToList();
            var Orders = orderstatu.Select(g => new Orderstatusall
            {
                OrderStatusId = g.OrderStatusId,
                Name = g.OrderStatusName
            }).ToList();
            result.Order = Orders;

            result.CreatedCards = CreateCards;

            result.OrderStatusId = OrderStatusIdCreator;
            result.Title = category.OrderStatusName;


            return result;

        }




        public bool PurchasedStatusToDB(OrderViewModel order)
        {

            //using (var tran = _ctx.Database.BeginTransaction())
            //{
            try
            {
                var orderinfo = _repo.GetAll<Orders>().First(x => x.OrderId == order.OrderId);
                if (orderinfo == null)
                {
                    throw new NotImplementedException();
                }

                if (order.OrderStatusId == 6)
                {
                    orderinfo.OrderStatusId = 6;
                    orderinfo.OrderStatusIdCreator = 6;
                    _repo.Update(orderinfo);
                    _repo.SaveChanges();
                }
                else
                {
                    orderinfo.OrderStatusId = order.OrderStatusId;
                    _repo.Update(orderinfo);
                    _repo.SaveChanges();
                }

                //tran.Commit();
                return true;

            }
            catch (Exception ex)
            {
                //tran.Rollback();
                return false;
            }
            //}
        }


        public bool CreatedStatusToDB(OrderViewModel order)
        {

            //using (var tran = _ctx.Database.BeginTransaction())
            //{
            try
            {
                var orderinfo = _repo.GetAll<Orders>().First(x => x.OrderId == order.OrderId);
                if (orderinfo == null)
                {
                    throw new NotImplementedException();
                }

                if (order.OrderStatusId == 6)
                {
                    orderinfo.OrderStatusId = 6;
                    orderinfo.OrderStatusIdCreator = 6;
                    _repo.Update(orderinfo);
                    _repo.SaveChanges();
                }
                else
                {
                    orderinfo.OrderStatusIdCreator = order.OrderStatusIdCreator;
                    _repo.Update(orderinfo);
                    _repo.SaveChanges();
                }

                //tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //tran.Rollback();
                return false;
            }
            //}
        }

        public OrderDetailViewModel GetOrderDetail(int orderId)
        {
            var memberId = MemberUtil.GetMemberId();
            var products = _repo.GetAll<Products>();
            var orders = _repo.GetAll<Orders>();
            var payments = _repo.GetAll<Payments>();
            var members = _repo.GetAll<Members>();
            var gameCat = _repo.GetAll<GameCategories>();
            var result = (from pr in products
                          join o in orders on pr.ProductId equals o.ProductId
                          join m in members on pr.CreatorId equals m.MemberId
                          join g in gameCat on pr.GameCategoryId equals g.GameCategoryId
                          where o.OrderId == orderId
                          select new OrderDetailViewModel
                          {
                              PlayerName = m.MemberName,
                              ProfilePic = pr.CreatorImg,
                              OrderStatusId = (int)o.OrderStatusId,
                              GameImg = g.GameCoverImgMini,
                              GameName = g.GameName,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              OrderConfirmation = o.OrderConfirmation,
                              OrderDateTime = o.OrderDate,
                              ProductId = pr.ProductId,
                          }).FirstOrDefault();
            var buyerDetails = (from o in orders
                                join m in members on o.CustomerId equals m.MemberId
                                where o.OrderId == orderId
                                select new
                                {
                                    Name = m.MemberName,
                                    Pic = m.ProfilePicture,
                                    Id = m.MemberId

                                }
                               ).FirstOrDefault();
            result.BuyerName = buyerDetails.Name;
            result.BuyerProfilePic = buyerDetails.Pic;
            result.BuyerId = buyerDetails.Id;

            var statusName = Enum.GetName(typeof(Enums.PaymentStatus), result.OrderStatusId);
            result.OrderStatusName = statusName;
            var orderPaid = (from o in orders
                             join p in payments on o.OrderId equals p.OrderId
                             where o.OrderId == orderId
                             select new OrderDetailViewModel
                             {
                                 PaymentConfirmation = p.ConfirmationId,
                                 PaymentDateTime = p.TransationDateTime
                             }).FirstOrDefault();
            if (orderPaid != null)
            {
                result.PaymentConfirmation = orderPaid.PaymentConfirmation;
                result.PaymentDateTime = orderPaid.PaymentDateTime;
            }
            return result;

        }
        public void CheckOrderTimeoutPurchased(int id)
        {
            var orders = _repo.GetAll<Orders>();
            var products = _repo.GetAll<Products>();
            var members = _repo.GetAll<Members>();
            var result = (from o in orders
                          where o.CustomerId == id && o.OrderStatusId == 1
                          select new
                          {
                              OrderId = o.OrderId,
                              OrderStatusId = o.OrderStatusId,
                              OrderDateTime = o.OrderDate
                          }
                          ).ToList();
            var utcTimeNow = DateTime.UtcNow;
            foreach (var order in result)
            {
                if (utcTimeNow > order.OrderDateTime.AddMinutes(15))
                {
                    try
                    {
                        var orderResult = orders.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                        orderResult.OrderStatusId = (int)Enums.PaymentStatus.Cancelled;
                        orderResult.OrderStatusIdCreator = (int)Enums.PaymentStatus.Cancelled;
                        _repo.Update(orderResult);
                        _repo.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var err = ex.ToString();
                    }
                }
            }

        }
        public void CheckOrderTimeoutCreated(int id)
        {
            var orders = _repo.GetAll<Orders>();
            var products = _repo.GetAll<Products>();
            var members = _repo.GetAll<Members>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          where p.CreatorId == id && o.OrderStatusId == 1
                          select new
                          {
                              OrderId = o.OrderId,
                              OrderStatusId = o.OrderStatusId,
                              OrderDateTime = o.OrderDate
                          }
                          ).ToList();
            var utcTimeNow = DateTime.UtcNow;
            foreach (var order in result)
            {
                if (utcTimeNow > order.OrderDateTime.AddMinutes(15))
                {
                    try
                    {
                        var orderResult = orders.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                        orderResult.OrderStatusId = (int)Enums.PaymentStatus.Cancelled;
                        _repo.Update(orderResult);
                        _repo.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var err = ex.ToString();
                    }
                }
            }
            //public OrderDetailViewModel GetCreatedOrderDetail(int orderId)
            //{
            //    //var memberId = MemberUtil.GetMemberId();
            //    //var products = _repo.GetAll<Products>();
            //    //var orders = _repo.GetAll<Orders>();
            //    //var payments = _repo.GetAll<Payments>();
            //    //var members = _repo.GetAll<Members>();
            //    //var gameCat = _repo.GetAll<GameCategories>();
            //    //var result = (from pr in products
            //    //              join o in orders on pr.ProductId equals o.ProductId

            //    //              where o.OrderId == orderId
            //    //              sleect new 
            //    //              )


            //}











            ////GetOrderInfo
            //public int GetCreatedOrderStatus(int OrderId)
            //{
            //    //var result = new OrderViewModel();
            //    //{
            //    //    CreatedCards = new List<CreatedCard>(),
            //    //    Order = new List<Orderstatusall>()
            //    //};


            //    var orderinfo = _repo.GetAll<Orders>().FirstOrDefault(x => x.OrderId == OrderId);
            //    if (orderinfo == null)
            //    {
            //        throw new NotImplementedException();
            //    }
            //    //var ordervm = new OrderViewModel()
            //    //{
            //    //    OrderStatusIdCreator = orderinfo.OrderStatusIdCreator
            //    //};

            //    if( orderinfo.OrderStatusIdCreator != null)
            //    {
            //        return (int)orderinfo.OrderStatusIdCreator;
            //    }
            //    else
            //    {
            //        throw new NotImplementedException();
            //    }         

            //}



            //public OrderViewModel GetOrderInfo(int OrderId)
            //{
            //    var emp = _repo.GetAll<Orders>().FirstOrDefault(x => x.OrderId == OrderId);

            //    if (emp == null)
            //    {
            //        throw new NotImplementedException();
            //    }

            //    OrderViewModel OrderInfo = new OrderViewModel()
            //    {
            //        OrderStatusId = emp.OrderStatusId == null? 1 : 3,
            //    };

            //    return OrderInfo;

            //}


        }
    }
}