using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.Models.StockInfoDB
{
    public class MainForceInOutInfo
    {
        public string StockID { get; set; }
        public string Buyers { get; set; }
        public decimal BuyCount { get; set; }
        public decimal SellCount { get; set; }
        public decimal AverageBuyPrice { get; set; }
        public decimal AverageSellPrice { get; set; }
        public decimal OverPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
