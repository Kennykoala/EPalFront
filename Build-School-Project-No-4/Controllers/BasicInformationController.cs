using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.Controllers
{
    public class BasicInformationController : Controller
    {
        private readonly EPalContext _ctx;

        public BasicInformationController()
        {
            _ctx = new EPalContext();
        }

        // GET: addgame
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult BasicInformation()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(GroupViewModel registerVM)
        //{
        //    var mem = _ctx.Members.Find(1);
        //    if (mem.MemberId == 1)
        //    {
        //        Product product = new Product
        //        {
        //            //GameCategoryId = registerVM.addgame.GameCategoryId,
        //            CreatorId = 1,
        //            UnitPrice = registerVM.addgame.UnitPrice,
        //            ProductImg = registerVM.addgame.ProductImg,
        //            Introduction = registerVM.addgame.Introduction,
        //            CreatorImg = registerVM.addgame.CreatorImg
        //        };

        //        ProductPlan productPlan = new ProductPlan
        //        {
        //            GameAvailableDay = registerVM.addgame.GameAvailableDay,
        //            GameStartTime = registerVM.addgame.GameStartTime,
        //            GameEndTime = registerVM.addgame.GameEndTime
        //        };

        //        List<ProductServer> serverName = new List<ProductServer>
        //        {
        //            new ProductServer
        //            {
        //                ServerId =(int)registerVM.addgame.ServerId

        //            }
        //        };

        //        List<ProductStyle> styleName = new List<ProductStyle>
        //        {
        //            new ProductStyle
        //            {
        //                StyleId = (int)registerVM.addgame.StyleId

        //            }
        //        };
        //        List<ProductPosition> positionName = new List<ProductPosition>
        //        {
        //            new ProductPosition
        //            {
        //               PositionId = (int)registerVM.addgame.PositionId

        //            }
        //        };

        //        using (var tran = _ctx.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                //_ctx.Members.Add(memnber);
        //                //_ctx.SaveChanges();
        //                _ctx.Products.Add(product);
        //                _ctx.ProductPlans.Add(productPlan);
        //                _ctx.ProductServers.AddRange(serverName);
        //                _ctx.ProductStyles.AddRange(styleName);
        //                _ctx.ProductPositions.AddRange(positionName);
        //                //_ctx.SaveChanges();                    
        //                _ctx.SaveChanges();
        //                tran.Commit();

        //               //tran.Commit();
        //                //ViewData["Message"] = "使用者儲存成功";
        //                return Content("儲存成功");
        //            }
        //            catch (Exception ex)
        //            {
        //                tran.Rollback();
        //                return Content("使用者儲存失敗:" + ex.ToString());
        //            }
        //        }
        //    }
          
        //    return Content("此會員不存在");
        //}
    }



}