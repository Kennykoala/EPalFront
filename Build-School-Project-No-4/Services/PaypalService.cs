using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;

namespace Build_School_Project_No_4.Services
{
    public class PaypalService
    {

        private PayPal.Api.Payment payment;
        private readonly Repository _repo;
        public PaypalService()
        {
            _repo = new Repository();
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
            var orders = _repo.GetAll<Orders>();
            var members = _repo.GetAll<Members>();
            var products = _repo.GetAll<Products>();
            var gameCat = _repo.GetAll<GameCategories>();
            var result = (from o in orders
                          join p in products on o.ProductId equals p.ProductId
                          join m in members on p.CreatorId equals m.MemberId
                          join g in gameCat on p.GameCategoryId equals g.GameCategoryId
                          where o.OrderConfirmation == confirmation
                          select new PaymentViewModel
                          {
                              ePalName = m.MemberName,
                              UnitPrice = o.UnitPrice,
                              Rounds = o.Quantity,
                              GameName = g.GameName
                          }
                              ).SingleOrDefault();


            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = $"{result.Rounds} round(s) of {result.GameName} with {result.ePalName}",
                currency = "USD",
                price = (result.UnitPrice).ToString(),
                quantity = result.Rounds.ToString(),
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                subtotal = (result.Rounds * result.UnitPrice).ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (result.Rounds * result.UnitPrice).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            var customerId = GetCustomerIdService.GetMemberId();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = $"Order ID: {confirmation}",
                invoice_number = PaymentUIDService.CreateTransactionUID(customerId), //Generate an Invoice No  
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
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        //public bool PaymentTransaction(int paymentId)
        //{
        //    //change primary key and foreign key in payments and order
        //    return true;
        //}

    }
}