using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.Services;
using Build_School_Project_No_4.ViewModels;

namespace Build_School_Project_No_4.Controllers
{
    
    public class JsonApiController : ApiController
    {
        private readonly ProductService _productService;
        private readonly ApiFilterService _apiFilterSevice;

        public JsonApiController()
        {
            _productService = new ProductService();
            _apiFilterSevice = new ApiFilterService();
        }

        
        [AcceptVerbs("GET", "POST")]
        public string getAllCardsJson(int id)
        {
            var cardJson = _productService.GetProductCardsJson(id);

            return cardJson;
        }

        [AcceptVerbs("GET", "POST")]
        public string GetProductCardByFilter(FilterItemViewModel FilterVM)
        {
            var cardJson = _apiFilterSevice.GetProductCardsByFilter(FilterVM);

            return cardJson;
        }
    }
}
