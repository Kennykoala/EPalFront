using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.Controllers
{
    public class epalAccountController : Controller
    {
        // GET: epalAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Wallet()
        {
            var Wallet = new WalletService();
            var Wallets = Wallet.GetWalletData();

            GroupViewModel Epalindex = new GroupViewModel
            {
                wallets = Wallets
            };
            return View(Epalindex);
        }

      
    }
}