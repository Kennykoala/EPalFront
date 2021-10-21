using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Build_School_Project_No_4.Controllers
{
    public class becomeepalController : Controller
    {
        private readonly EPalContext _ctx;

        public becomeepalController()
        {
            _ctx = new EPalContext();
        }

        // GET: BeComeEPal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BecomeEPalPage()
        {
            return View();
        }


        [HttpGet]
        public ActionResult addgame()
        {
            GroupViewModel addgame = new GroupViewModel()
            {
                addgame = new AddgameViewModel()
            };

            AddgameViewModel addVM = new AddgameViewModel()
            {
                //planset = new List<ProductPlanSet>(),
                ServerItems = new List<Server>(),
                ServerSelectedId = new List<int>(),
                PositionItems = new List<Position>(),
                PositionSelectedId = new List<int>(),
            };

            //List<ProductPlanSet> AvailabledayList = new List<ProductPlanSet>()
            //{
            //    new ProductPlanSet{ GameAvailableDay = "Monday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Tuesday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Wednesday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Thursday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Friday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Saturday", GameStartTime = null, GameEndTime=null },
            //    new ProductPlanSet{ GameAvailableDay = "Sunday", GameStartTime = null, GameEndTime=null }
            //};


            List<Server> ServerList = new List<Server>()
            {
                new Server() { ServerId = 1, ServerName = "OCE" },
                new Server() { ServerId = 2, ServerName = "NA" },
                new Server() { ServerId = 3, ServerName = "LAN" },
                new Server() { ServerId = 4, ServerName = "BR" },
                new Server() { ServerId = 5, ServerName = "EU_West" },
                new Server() { ServerId = 6, ServerName = "EU_NorthEast" }
            };
            List<int> defaultServer = new List<int>() { 1 };

            List<Position> PositionList = new List<Position>()
            {
                new Position() { PositionId = 1, PositionName = "Top" },
                new Position() { PositionId = 2, PositionName = "Jungler" },
                new Position() { PositionId = 3, PositionName = "ADC" },
                new Position() { PositionId = 4, PositionName = "Support" },
                new Position() { PositionId = 5, PositionName = "Middle" }
            };
            List<int> defaultPosition = new List<int>() { 1 };

            List<Style> StyleList = new List<Style>()
            {
                new Style() { StyleId = 1, StyleName = "Love_Inting" },
                new Style() { StyleId = 2, StyleName = "Try_Hard" },
                new Style() { StyleId = 3, StyleName = "Hard_Stuck" },
                new Style() { StyleId = 4, StyleName = "Sneaky" },
                new Style() { StyleId = 5, StyleName = "Global_Presence" },
                new Style() { StyleId = 6, StyleName = "One_Shot" }
            };
            List<int> defaultStyle = new List<int>() { 1 };



            //addgame.addgame.planset = AvailabledayList;
            addgame.addgame.ServerItems = ServerList;
            addgame.addgame.ServerSelectedId = defaultServer;
            addgame.addgame.PositionItems = PositionList;
            addgame.addgame.PositionSelectedId = defaultPosition;
            addgame.addgame.StyleItems = StyleList;
            addgame.addgame.StyleSelectedId = defaultStyle;

            addgame.addgame.GameAvailableDay1 = "Monday";
            addgame.addgame.GameAvailableDay2 = "Tuesday";
            addgame.addgame.GameAvailableDay3 = "Wednesday";
            addgame.addgame.GameAvailableDay4 = "Thursday";
            addgame.addgame.GameAvailableDay5 = "Friday";
            addgame.addgame.GameAvailableDay6 = "Saturday";
            addgame.addgame.GameAvailableDay7 = "Sunday";

            //string JsonDay = JsonConvert.SerializeObject(AvailabledayList);
            //ViewBag.JsonLocations = JsonDay;

            return View(addgame);
            //return View("_GameDayPartial", addgame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addgame(GroupViewModel registerVM)
        {
            var mem = _ctx.Members.Find(1);
            if (mem.MemberId == 1)
            {
                using (var tran = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        AddgameViewModel add = new AddgameViewModel()
                        {
                            //planset = new List<ProductPlanSet>(),
                            ServerItems = new List<Server>(),
                            ServerSelectedId = new List<int>(),
                            PositionItems = new List<Position>(),
                            PositionSelectedId = new List<int>(),
                            StyleItems = new List<Style>(),
                            StyleSelectedId = new List<int>()
                        };

                        //VM -> DM
                        Products product = new Products
                        {
                            GameCategoryId = (int)registerVM.addgame.GameCategoryId,
                            CreatorId = 1,
                            UnitPrice = registerVM.addgame.UnitPrice,
                            ProductImg = registerVM.addgame.ProductImg,
                            Introduction = registerVM.addgame.Introduction,
                            CreatorImg = "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/best-girl-cat-names-1606245046.jpg",
                            //CreatorImg = registerVM.addgame.CreatorImg,
                            RecommendationVoice = registerVM.addgame.RecommendationVoice,
                            RankId = (int)registerVM.addgame.RankId
                        };

                        _ctx.Products.Add(product);
                        _ctx.SaveChanges();



                        //server
                        ProductServer serverDB = new ProductServer();
                        var serverSelected = registerVM.addgame.ServerSelectedId;
                        foreach (var item in serverSelected)
                        {
                            //.username = comboUserName3.SelectedValue.ToString();
                            serverDB.ProductId = product.ProductId;

                            string selectedItem = item.ToString();
                            int val = int.Parse(selectedItem);
                            serverDB.ServerId = val;
                            ////grp.iscurrent = true;
                            //grp.dateadded = DateTime.Now;
                            _ctx.ProductServer.Add(serverDB);
                            _ctx.SaveChanges();                            
                        }


                        //position
                        ProductPosition positionDB = new ProductPosition();
                        var positionSelected = registerVM.addgame.PositionSelectedId;
                        foreach (var item in positionSelected)
                        {
                            //.username = comboUserName3.SelectedValue.ToString();
                            positionDB.ProductId = product.ProductId;

                            string selectedItem = item.ToString();
                            int val = int.Parse(selectedItem);
                            positionDB.PositionId = val;
                            ////grp.iscurrent = true;
                            //grp.dateadded = DateTime.Now;
                            _ctx.ProductPosition.Add(positionDB);
                            _ctx.SaveChanges();
                        }


                        //style
                        ProductStyle styleDB = new ProductStyle();
                        var styleSelected = registerVM.addgame.StyleSelectedId;
                        foreach (var item in styleSelected)
                        {
                            //.username = comboUserName3.SelectedValue.ToString();
                            styleDB.ProductId = product.ProductId;

                            string selectedItem = item.ToString();
                            int val = int.Parse(selectedItem);
                            styleDB.StyleId = val;
                            ////grp.iscurrent = true;                            
                            //grp.dateadded = DateTime.Now;
                            _ctx.ProductStyle.Add(styleDB);
                            _ctx.SaveChanges();
                        }


                        //plan
                        //VM -> DM
                        //ProductPlan productplan1 = new ProductPlan() 
                        //if (productplan1.) { }

                        //for(var i = 1; i<=7; i++)
                        //{
                        //}


                        ProductPlans productplan1 = new ProductPlans
                        {
                            //.ToShortTimeString()
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay1,
                            GameStartTime = registerVM.addgame.GameStartTime1,
                            GameEndTime = registerVM.addgame.GameEndTime1,
                        };
                        if (productplan1 != null) 
                        {
                            _ctx.ProductPlans.Add(productplan1);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan2 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay2,
                            GameStartTime = registerVM.addgame.GameStartTime2,
                            GameEndTime = registerVM.addgame.GameEndTime2,
                        };
                        if (productplan2 != null)
                        {
                            _ctx.ProductPlans.Add(productplan2);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan3 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay3,
                            GameStartTime = registerVM.addgame.GameStartTime3,
                            GameEndTime = registerVM.addgame.GameEndTime3,
                        };
                        if (productplan3 != null)
                        {
                            _ctx.ProductPlans.Add(productplan3);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan4 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay4,
                            GameStartTime = registerVM.addgame.GameStartTime4,
                            GameEndTime = registerVM.addgame.GameEndTime4,
                        };
                        if (productplan4 != null)
                        {
                            _ctx.ProductPlans.Add(productplan4);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan5 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay5,
                            GameStartTime = registerVM.addgame.GameStartTime5,
                            GameEndTime = registerVM.addgame.GameEndTime5,
                        };
                        if (productplan5 != null)
                        {
                            _ctx.ProductPlans.Add(productplan5);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan6 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay6,
                            GameStartTime = registerVM.addgame.GameStartTime6,
                            GameEndTime = registerVM.addgame.GameEndTime6,
                        };
                        if (productplan6 != null)
                        {
                            _ctx.ProductPlans.Add(productplan6);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan7 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.addgame.GameAvailableDay7,
                            GameStartTime = registerVM.addgame.GameStartTime7,
                            GameEndTime = registerVM.addgame.GameEndTime7,
                        };
                        if (productplan7 != null)
                        {
                            _ctx.ProductPlans.Add(productplan7);
                            _ctx.SaveChanges();
                        }




                        ////plan
                        //var planItem1 = registerVM.addgame.GameAvailableDay1;
                        //var planItems1 = registerVM.addgame.GameStartTime1;
                        //var planIteme1 = registerVM.addgame.GameEndTime1;
                        //_ctx.ProductPositions.Add(positionDB);
                        //_ctx.SaveChanges();

                        //var planItem2 = registerVM.addgame.GameAvailableDay2;
                        //var planItems2 = registerVM.addgame.GameStartTime2;
                        //var planIteme2 = registerVM.addgame.GameEndTime2;



                        ////plan(test)
                        //List<ProductPlanSet> planset = new List<ProductPlanSet>();
                        //ProductPlan planDB = new ProductPlan();
                        //var planItem = registerVM.addgame.planset[0].GameAvailableDay;
                        //var planItems = registerVM.addgame.planset[0].GameStartTime;
                        //var planIteme = registerVM.addgame.planset[0].GameEndTime;

                        //var planItem2 = registerVM.addgame.planset[1].GameAvailableDay;
                        //var planItems2 = registerVM.addgame.planset[1].GameStartTime;
                        //var planIteme2 = registerVM.addgame.planset[1].GameEndTime;








                        //foreach (var item in planItem)
                        //{
                        //    planDB.ProductId = product.ProductId;
                        //    planDB.GameAvailableDay = item.GameAvailableDay;
                        //    planDB.GameStartTime = item.GameStartTime;
                        //    planDB.GameEndTime = item.GameEndTime;
                        //    _ctx.ProductPlans.Add(planDB);
                        //    _ctx.SaveChanges();
                        //}

                        //product plan
                        //List<ProductPlan> planList = new List<ProductPlan>();
                        //foreach (var item in registerVM.addgame.planset)
                        //{
                        //    planList.Add(new ProductPlan
                        //    {
                        //        GameAvailableDay = pro.GameAvailableDay,
                        //        GameStartTime = pro.GameStartTime,
                        //        GameEndTime = pro.GameEndTime
                        //    });
                        //}
                        //_ctx.ProductPlans.Add(productPlan);
                        //_ctx.SaveChanges();



                        tran.Commit();

                        //ViewData["Message"] = "使用者儲存成功";
                        return Content("創建商品成功");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return Content("創建商品失敗:" + ex.ToString());
                    }
                }
            }

            return Content("此會員不存在");
        }
    }
}