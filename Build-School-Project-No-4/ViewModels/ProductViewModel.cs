using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class ProductViewModel
    {
        public int CategoryId { get; set; }

        public List<ProductCard> ProductCards { get; set; }
    }


    public class ProductCard 
    {
        /// <summary>
        /// 卡片名字(人)
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 卡片照片(人)
        /// </summary>
        public string CreatorImg { get; set; }

        /// <summary>
        /// 商品自介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 線上狀態
        /// </summary>
        public string LineStatus { get; set; }

        /// <summary>
        /// 錄音檔
        /// </summary>
        public string RecommendationVoice { get; set; }

        /// <summary>
        /// 遊戲牌位
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 遊戲路線
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 星星評價
        /// </summary>
        public int StarLevel { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        public int ProductId { get; set; }
    }
}