using CommonService.DBClass;
using CommonService.Helper;
using CommonService.Models.StockInfoDB;
using CommonService.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StockDataCatcher.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace StockDataCatcher.Models.Stock
{
    /// <summary>
    /// 拉取股票交易資訊(股價、收盤價)
    /// </summary>
    public class StockTradeInfoService : StockBase<StockInfo>
    {
        public StockTradeInfoService(IConfiguration config
            , LineNotifyService Line
            , APIService Api
            , ConsoleLogHelper Log
            , List<StockIDModel> StockList
            , StockInfoDB StockInfoDB
            ) : base(config, Line, Api, Log, StockList, StockInfoDB) { }

        public async override Task<IList<StockInfo>> Data_Get()
        {
            IList<StockInfo> StockInfoList = new List<StockInfo>();
            foreach (StockIDModel stock in _stockList)
            {
                JObject CallResult = await StockApi_Call(stock);
                //拆解檔案內容
                StockInfoList = await DownloadData_Analysis(CallResult, stock);
            }

            return StockInfoList;
        }

        public async override Task<IList<StockInfo>> Data_Get_UserSetting(IList<DateTime> DateTimeList)
        {
            List<StockInfo> StockInfoList = new List<StockInfo>();
            //List<Task> ThingDO = new List<Task>();
            foreach (StockIDModel stock in _stockList)
            {
                foreach (DateTime date in DateTimeList)
                { 
                    JObject CallResult = await StockApi_Call(stock, date);
                    //拆解檔案內容
                    StockInfoList.AddRange(await DownloadData_Analysis(CallResult, stock));
                }
            }

            return StockInfoList;
        }

        #region 呼叫拉取股價api
        /// <summary>
        /// 呼叫api
        /// </summary>
        /// <param name="data">拉取股票資訊</param>
        /// <param name="SreachDate">拉取日期(沒有則預設今日)</param>
        /// <returns></returns>
        private async Task<JObject> StockApi_Call(StockIDModel data, DateTime? SreachDate = null)
        {
            string NowSearchDate = "";
            if (SreachDate==null)
                NowSearchDate = DateTime.Now.ToString("yyyyMMdd");
            else
                NowSearchDate = Convert.ToDateTime(SreachDate).ToString("yyyyMMdd");
            _log.Log($"拉取{data.StockID}-{data.StockName}-{NowSearchDate}的股票資訊");
            string GetString = $"{_config["StockInfo:APIUrl"].ToString()}?response=json&date={NowSearchDate}&stockNo={data.StockID}";
            _log.Log(GetString, ConsoleColor.Green);
            string ApiResult = await _api.Get_async(GetString);
            JObject CallResult = null;
            if (!string.IsNullOrEmpty(ApiResult))
            {
                CallResult = JObject.Parse(ApiResult);
            }
            return CallResult;
        }
        #endregion

        #region 拆解檔案內容
        /// <summary>
        /// 拆解檔案內容
        /// </summary>
        /// <param name="APIResult">Api呼叫結果</param>
        /// <param name="StockData">取得股票資訊</param>
        /// <returns></returns>
        private async Task<IList<StockInfo>> DownloadData_Analysis(JObject APIResult, StockIDModel StockData)
        {
            List<Task<StockInfo>> Result = null;
            if (APIResult.Property("stat") != null && APIResult["stat"].ToString() == "OK")
            {
                Result = new List<Task<StockInfo>>();
                _log.Log($"拉取成功，開始拆解json", ConsoleColor.Green);
                if (APIResult.Property("data") != null)
                {
                    foreach (JArray data in APIResult.Property("data").Value)
                    {
                        Result.Add(StockInfoData_Proccess(data, StockData.StockID));
                    }
                }
            }
            //await Task.WhenAll<List<StockInfo>>();
            return await Task.WhenAll(Result);
        }

        private async Task<StockInfo> StockInfoData_Proccess(JArray data, string StockID)
        {
            //DateTime dateout;
            decimal convertout;
            StockInfo StockInfoData = new StockInfo();
            await Task.Run(() =>
            {
                StockInfoData.StockID = StockID;
                DateTime Datec = DateTime.ParseExact(data[0].ToString(), "yyy/MM/dd", CultureInfo.InvariantCulture).AddYears(1911);
                StockInfoData.Date = Datec;
                //DateTime.TryParse(data[0].ToString(),out dateout)?new DateTime(): dateout;
                StockInfoData.TradingVolume = decimal.TryParse(data[1].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                StockInfoData.Turnover = decimal.TryParse(data[2].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                StockInfoData.OpeningPrice = decimal.TryParse(data[3].ToString(), out convertout) ? convertout : 0;
                StockInfoData.HighestPrice = decimal.TryParse(data[4].ToString(), out convertout) ? convertout : 0;
                StockInfoData.LowestPrice = decimal.TryParse(data[5].ToString(), out convertout) ? convertout : 0;
                StockInfoData.ClosingPrice = decimal.TryParse(data[6].ToString(), out convertout) ? convertout : 0;
                StockInfoData.Spreads = decimal.TryParse(data[7].ToString(), out convertout) ? convertout : 0;
                StockInfoData.TransactionAmount = decimal.TryParse(data[8].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                
            });
            _log.Log($"{StockInfoData.Date}處理完成!");
            return StockInfoData;
        }
        #endregion

        #region 存入db
        public override void DataSave(IList<StockInfo> Data)
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();

            try
            {
                Result = _stockInfoDB.BatchSP_ImportStockInfo(Data);
                string ResultMessage = $"{DateTime.Now}--Code：{Result["Code"].ToString()}，Msg：{Result["Message"].ToString()}";
                if (Result["Code"].ToString() == "1")
                    _log.Log(ResultMessage, ConsoleColor.Green);
                else
                    _log.Log(ResultMessage, ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                _log.Log($"{DateTime.Now}--存入股票資訊例外，ex{ex}", ConsoleColor.Red);
            }
        }
        #endregion


    }
}
