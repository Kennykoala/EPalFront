using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.ViewModels
{
    public class FilterItemViewModel
    {
        public int CategoryId { get; set; }

        public List<string> Server { get; set; }

        public List<string> Language { get; set; }

        public List<string> Level { get; set; }

        public string HighAge { get; set; }

        public string LowAge { get; set; }

        public string HighPrice { get; set; }

        public string LowPrice { get; set; }

        public string Gender { get; set; }

        public string Status { get; set; }

    }
}