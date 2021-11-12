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
        private readonly EPalContext _ctx;

        public OrderService()
        {
            _repo = new Repository();
            _ctx = new EPalContext();
        }


        //Purchased Orders
        public OrderViewModel GetOrderCardData(int OrderStatusId,int mems)
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
            var OrderCards = orders.Select(o => new OrderCard
            {
                OrderStatusName = category.OrderStatusName,
                Quantity = o.Quantity,
                OrderDate = o.OrderDate,
                TotalPrice = o.UnitPrice * o.Quantity,
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                GameName = o.Products.GameCategories.GameName,
                MemberName = o.Products.Members.MemberName,
                //ProfilePicture=o.Members.ProfilePicture
                ProfilePicture = o.Products.Members.ProfilePicture,
                OrderStatusIdCreator = o.OrderStatusIdCreator

                //ProductId =o.Products.ProductId
                //GameName=GameCat.FirstOrDefault(y=>y.GameCategoryId ==(products.FirstOrDefault(x=>x.ProductId==o.ProductId).GameCategoryId)).GameName

            }).ToList();


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
                //ProfilePicture=o.Members.ProfilePicture
                ProfilePicture = c.Members.ProfilePicture,

                OrderStatusId = c.OrderStatusId

            }).ToList();




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