﻿using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.Utilities;

namespace Build_School_Project_No_4.Services
{

    public class PaypalService
    {

        private PayPal.Api.Payment payment;
        private readonly OrderUtil _orderUtil;
        public PaypalService()
        {
            _orderUtil = new OrderUtil();
        }

        public Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        public Payment CreatePayment(APIContext apiContext, string redirectUrl, string confirmation)
        {
            var order = _orderUtil.GetOrder(confirmation);

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            itemList.items.Add(new Item()
            {
                name = $"{order.Rounds} round(s) of {order.GameName} with {order.ePalName}",
                currency = "USD",
                price = (order.UnitPrice).ToString(),
                quantity = order.Rounds.ToString(),
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            var details = new Details()
            {
                subtotal = (order.Rounds * order.UnitPrice).ToString()
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = (order.Rounds * order.UnitPrice).ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            var customerId = MemberUtil.GetMemberId();
            transactionList.Add(new Transaction()
            {
                description = $"Order ID: {confirmation}",
                invoice_number = Utilities.PaymentUtil.CreateTransactionUID(customerId),
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }

        public string GetRedirectUrl(string confirmation, APIContext apiContext)
        {
            string baseURI = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/Checkout/PaymentWithPayPal?";
            var guid = Convert.ToString((new Random()).Next(100000));
            var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid, confirmation);
            var links = createdPayment.links.GetEnumerator();
            string paypalRedirectUrl = null;
            while (links.MoveNext())
            {
                Links lnk = links.Current;
                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                {
                    paypalRedirectUrl = lnk.href;
                }
            }
            HttpContext.Current.Session.Add(guid, createdPayment.id);
            return paypalRedirectUrl;
        }

        //public string CheckPayApproval(APIContext apiContext, string payerId)
        //{
        //    var guid = HttpContext.Current.Request.Params["guid"];

        //    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Current.Session[guid] as string);
        //    var trId = executedPayment.transactions[0].related_resources[0].sale.id;
        //    if (executedPayment.state.ToLower() != PaymentStatus.approved.ToString())
        //    {
        //        return PaymentStatus.failed.ToString();
        //    }
        //}
            


    }
}