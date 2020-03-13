using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.Models.StockInfoDB
{
    public class StockInfo
    {
        public string StockID { get; set; }
        public decimal TradingVolume { get; set; }
        public decimal Turnover { get; set; }
        public decimal OpeningPrice { get; set; }
        public decimal HighestPrice { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal Spreads { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
