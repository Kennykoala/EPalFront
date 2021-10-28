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

            //Orders cusid = _ctx.Orders.Find(id);

            //if (mems == cusid.CustomerId)

                if (!id.HasValue)
                {
                    return RedirectToAction("PurchasedOrderSummary", "Order", new { id = 1 });
                }
                //if (!id.HasValue)
                //{
                //    return RedirectToAction("Index");
                //}
                var order = new OrderService();
                var abc = order.GetOrderCardData(id.Value, mems);
                // var ordercards = _orderService.GetOrderCardData(id.Value);
                //GroupViewModel result = new GroupViewModel
                //{
                //    Order = abc
                //};
            
                return View(abc);       

        }

        [Authorize]
        public ActionResult CreatedOrderSummary(int? id)
        {

            var mem = _ctx.Members.Find(int.Parse(GetMemberId()));
            var mems = mem.MemberId;

            //Orders cusid = _ctx.Orders.Find(id);

            //if (mems == cusid.CustomerId)

            if (!id.HasValue)
            {
                return RedirectToAction("CreatedOrderSummary", "Order", new { id = 1 });
            }
            //if (!id.HasValue)
            //{
            //    return RedirectToAction("Index");
            //}
            var order = new OrderService();
            var abc = order.GetCreatedOrderCardData(id.Value, mems);
            // var ordercards = _orderService.GetOrderCardData(id.Value);
            //GroupViewModel result = new GroupViewModel
            //{
            //    Order = abc
            //};

            return View(abc);

        }


        [HttpPost]
        public ActionResult UpdateNotStarted(OrderViewModel order)        
        {
            var msg = "";
            using (var tran = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var orderinfo = _ctx.Orders.First(x => x.OrderId == order.OrderId);
                    if (orderinfo == null)
                    {
                        throw new NotImplementedException();
                    }


                    orderinfo.OrderStatusId = order.OrderStatusId;
                    _ctx.SaveChanges();
                    tran.Commit();

                    msg = "OK";
                    return Json(msg);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return Content("更新linestatus失敗:" + ex.ToString());
                }
            }

        }

    }
}