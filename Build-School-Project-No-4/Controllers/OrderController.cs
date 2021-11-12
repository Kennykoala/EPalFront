﻿using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Build_School_Project_No_4.ViewModels;
using System.Web.Security;
using Newtonsoft.Json;

namespace Build_School_Project_No_4.Controllers
{
   
    public class OrderController : Controller
    {


     

        private readonly ProductService _productService;
        private readonly EPalContext _ctx;
        private readonly DetailServices _detailService;
        private readonly AddToCartService _cartService;
        private readonly CheckoutService _checkoutService;
        private readonly OrderService _orderService;
        private readonly MemberService _memberService;

        public OrderController()
        {
            _memberService = new MemberService();
            _productService = new ProductService();
            _detailService = new DetailServices();
            _ctx = new EPalContext();
            _cartService = new AddToCartService();
            _checkoutService = new CheckoutService();
            _orderService = new OrderService();
        }
        // GET: Order
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //取得登入者的memberId
       
        public string GetMemberId()
        {
            var cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            string userid = "";
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                var obj = JsonConvert.DeserializeObject<Members>(ticket.UserData);
                userid = obj.MemberId.ToString();
                return userid;
            }
            return null;
        }

        [Authorize]
        public ActionResult PurchasedOrderSummary(int? id)
        {

            var mem = _ctx.Members.Find(int.Parse(GetMemberId()));
            var mems = mem.MemberId;

                if (!id.HasValue)
                {
                    return RedirectToAction("PurchasedOrderSummary", "Order", new { id = 1 });
                }

                var order = new OrderService();
                var PurchasedOrderInfo = order.GetOrderCardData(id.Value, mems);
            
                return View(PurchasedOrderInfo);       

        }

        [Authorize]
        public ActionResult CreatedOrderSummary(int? id)
        {

            var mem = _ctx.Members.Find(int.Parse(GetMemberId()));
            var mems = mem.MemberId;

            if (!id.HasValue)
            {
                return RedirectToAction("CreatedOrderSummary", "Order", new { id = 1 });
            }

            var order = new OrderService();
            var CreatedOrderInfo = order.GetCreatedOrderCardData(id.Value, mems);

            return View(CreatedOrderInfo);

        }


        [HttpPost]
        public ActionResult UpdatePurchasedStatus(OrderViewModel order)        
        {
            bool msg = _orderService.PurchasedStatusToDB(order);

            return Json(msg);


            //using (var tran = _ctx.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        var orderinfo = _ctx.Orders.First(x => x.OrderId == order.OrderId);
            //        if (orderinfo == null)
            //        {
            //            throw new NotImplementedException();
            //        }
            //        orderinfo.OrderStatusId = order.OrderStatusId;
            //        _ctx.SaveChanges();
            //        tran.Commit();
            //        return Json(true);
            //    }
            //    catch (Exception ex)
            //    {
            //        tran.Rollback();
            //        return Json(false);
            //    }
            //}


        }



        [HttpPost]
        public ActionResult UpdateCreatorNotStarted(OrderViewModel order)
        {

            bool msg = _orderService.CreatedStatusToDB(order);

            return Json(msg);


            //using (var tran = _ctx.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        var orderinfo = _ctx.Orders.First(x => x.OrderId == order.OrderId);
            //        if (orderinfo == null)
            //        {
            //            throw new NotImplementedException();
            //        }
            //        orderinfo.OrderStatusIdCreator = order.OrderStatusIdCreator;
            //        _ctx.SaveChanges();
            //        tran.Commit();
            //        return Json(true);
            //    }
            //    catch (Exception ex)
            //    {
            //        tran.Rollback();
            //        return Json(false);
            //    }
            //}

        }





    }
}