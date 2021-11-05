using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        [HttpGet]
        public ActionResult addgame()
        {
            //GroupViewModel addgame = new GroupViewModel()
            //{
            //    addgame = new AddgameViewModel()
            //};

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
            addVM.ServerItems = ServerList;
            addVM.ServerSelectedId = defaultServer;
            addVM.PositionItems = PositionList;
            addVM.PositionSelectedId = defaultPosition;
            addVM.StyleItems = StyleList;
            addVM.StyleSelectedId = defaultStyle;

            addVM.GameAvailableDay1 = "Monday";
            addVM.GameAvailableDay2 = "Tuesday";
            addVM.GameAvailableDay3 = "Wednesday";
            addVM.GameAvailableDay4 = "Thursday";
            addVM.GameAvailableDay5 = "Friday";
            addVM.GameAvailableDay6 = "Saturday";
            addVM.GameAvailableDay7 = "Sunday";

            //string JsonDay = JsonConvert.SerializeObject(AvailabledayList);
            //ViewBag.JsonLocations = JsonDay;

            return View(addVM);
            //return View("_GameDayPartial", addgame);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addgame(AddgameViewModel registerVM)
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
                            GameCategoryId = (int)registerVM.GameCategoryId,
                            CreatorId = int.Parse(GetMemberId()),
                            UnitPrice = registerVM.UnitPrice,
                            ProductImg = registerVM.ProductImg,
                            Introduction = registerVM.Introduction,
                            CreatorImg = registerVM.CreatorImg,
                            //CreatorImg = registerVM.addgame.CreatorImg,
                            RecommendationVoice = registerVM.RecommendationVoice,
                            RankId = (int)registerVM.RankId
                        };

                        _ctx.Products.Add(product);
                        _ctx.SaveChanges();



                        //server
                        ProductServer serverDB = new ProductServer();
                        var serverSelected = registerVM.ServerSelectedId;
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
                        var positionSelected = registerVM.PositionSelectedId;
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
                        var styleSelected = registerVM.StyleSelectedId;
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
                        ProductPlans productplan1 = new ProductPlans
                        {
                            //.ToShortTimeString()
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay1,
                            GameStartTime = registerVM.GameStartTime1,
                            GameEndTime = registerVM.GameEndTime1,
                        };
                        if (productplan1.GameStartTime != null && productplan1.GameEndTime != null) 
                        {
                            _ctx.ProductPlans.Add(productplan1);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan2 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay2,
                            GameStartTime = registerVM.GameStartTime2,
                            GameEndTime = registerVM.GameEndTime2,
                        };
                        if (productplan2.GameStartTime != null && productplan2.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan2);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan3 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay3,
                            GameStartTime = registerVM.GameStartTime3,
                            GameEndTime = registerVM.GameEndTime3,
                        };
                        if (productplan3.GameStartTime != null && productplan3.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan3);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan4 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay4,
                            GameStartTime = registerVM.GameStartTime4,
                            GameEndTime = registerVM.GameEndTime4,
                        };
                        if (productplan4.GameStartTime != null && productplan4.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan4);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan5 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay5,
                            GameStartTime = registerVM.GameStartTime5,
                            GameEndTime = registerVM.GameEndTime5,
                        };
                        if (productplan5.GameStartTime != null && productplan5.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan5);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan6 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay6,
                            GameStartTime = registerVM.GameStartTime6,
                            GameEndTime = registerVM.GameEndTime6,
                        };
                        if (productplan6.GameStartTime != null && productplan6.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan6);
                            _ctx.SaveChanges();
                        }


                        ProductPlans productplan7 = new ProductPlans
                        {
                            ProductId = product.ProductId,
                            GameAvailableDay = registerVM.GameAvailableDay7,
                            GameStartTime = registerVM.GameStartTime7,
                            GameEndTime = registerVM.GameEndTime7,
                        };
                        if (productplan7.GameStartTime != null && productplan7.GameEndTime != null)
                        {
                            _ctx.ProductPlans.Add(productplan7);
                            _ctx.SaveChanges();
                        }

                        tran.Commit();
                        //Response.Write("<script language=javascript>alert('123');</" + "script>");
                        TempData["message"] = "success";
                        return RedirectToAction("BecomeEPalPage", "becomeepal");

                        //string script = "window.onload = function () {swal.fire({title: 'Create Game Success', icon: 'success'}); }";
                        //return JavaScript(script);

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        TempData["msg"] = "fail";
                        return RedirectToAction("BecomeEPalPage", "becomeepal");

                        //return Content("創建商品失敗:" + ex.ToString());
                    }

                }

        }
    }
}