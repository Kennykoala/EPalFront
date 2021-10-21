using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class WalletService
    {

        private readonly Repository _Repo;
        //public WalletService()
        //{
        //    //_OrderRepo = new OrderRepository();
        //    _Repo = new Repository();
        //}

        private IQueryable<Orders> Orders;
        private IQueryable<Members> Members;
        private IQueryable<OrderStatus> OrderStatus;

        public WalletService()
        {
            _Repo = new Repository();
            Orders = _Repo.GetAll<Orders>();
            Members = _Repo.GetAll<Members>();
            OrderStatus=_Repo.GetAll<OrderStatus>();
        }

        public List<WalletViewModel> GetWalletData()
        {
            List<WalletViewModel> WalletVM = new List<WalletViewModel>();
            foreach(var item in Orders)
            {
                WalletVM.Add( new WalletViewModel
                { 
                    OrderId = item.OrderId,
                    OrderDate =item.OrderDate,
                    MemberName = Members.FirstOrDefault(x=>x.MemberId== item.CustomerId).MemberName,
                    TotalPrice= item.Quantity*item.UnitPrice,
                    OrderStatusName =OrderStatus.FirstOrDefault(x=>x.OrderStatusId== item.OrderStatusId).OrderStatusName
                });
            }
            return WalletVM;
        }

    }
}