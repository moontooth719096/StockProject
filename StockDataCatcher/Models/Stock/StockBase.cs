using CommonService.DBClass;
using CommonService.Helper;
using CommonService.Models;
using CommonService.Models.StockInfoDB;
using CommonService.Services;
using Microsoft.Extensions.Configuration;
using StockDataCatcher.Helper;
using StockDataCatcher.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDataCatcher.Models.Stock
{
    /// <summary>
    /// 股票基底
    /// </summary>
    public abstract class StockBase<T> : IStock
    {
        protected readonly IConfiguration _config;
        protected readonly LineNotifyService _line;
        protected readonly APIService _api;
        protected readonly ConsoleLogHelper _log;
        protected readonly List<StockIDModel> _stockList;
        protected readonly StockInfoDB _stockInfoDB;
        public StockBase(IConfiguration config, LineNotifyService Line, APIService Api, ConsoleLogHelper Log, List<StockIDModel> StockList, StockInfoDB StockInfoDB)
        {
            _config = config;
            _line = Line;
            _api = Api;
            _log = Log;
            _stockList = StockList;
            _stockInfoDB = StockInfoDB;
        }
        /// <summary>
        /// 股票資料處理流
        /// </summary>
        /// <returns></returns>
        public async Task DataProccess()
        {
            List<T> Data = await Data_Get();

            DataSave(Data);
        }
        /// <summary>
        /// 儲存資料
        /// </summary>
        /// <returns></returns>
        public abstract void DataSave(List<T> Data);
        /// <summary>
        /// 拉取資料
        /// </summary>
        /// <returns></returns>
        public abstract Task<List<T>> Data_Get();

        /// <summary>
        /// 發出警告
        /// </summary>
        /// <returns></returns>
        public async virtual Task Warming(string Message)
        {
            await _line.LineNotify_Send(new LineNotifyPostModel { Message = Message });
        }
    }
}
