using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class LinePayService
    {
        private readonly Repository _repo;
        private readonly LinePayViewModel _linePayVM;
        public LinePayService()
        {
            _repo = new Repository();
            _linePayVM = new LinePayViewModel();
        }
		public static string LinePayHMACSHA256(string key, string message)
		{
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(key);

			HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

			byte[] messageBytes = encoding.GetBytes(message);
			byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

			//注意他原本的公式是直接轉為string
			return Convert.ToBase64String(hashmessage);
		}

		public LinePayViewModel.LineForm LinePayCreateOrder(string confirmation)
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
                          select new LinePayViewModel.LineForm
                          {
                             amount = p.UnitPrice,



                          }
                              ).SingleOrDefault();
            return result;
        }
	}
}