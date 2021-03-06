﻿using CommonService.DBClass;
using CommonService.EnumModels;
using CommonService.Helper;
using CommonService.Models.Common;
using CommonService.Models.StockInfoDB;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using StockDataCatcher.Helper;
using StockDataCatcher.IService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataCatcher.Service
{
    public class StockService : IStockService
    {
        private readonly ConsoleLogHelper _Log;
        private readonly IConfiguration _Configuration;
        private StockInfoDB _StockInfoDB;
        private APIService _api;
        public StockService(IConfiguration Configuration, ConsoleLogHelper Log, APIService Api)
        {
            _Log = Log;
            _api = Api;
            _Configuration = Configuration;
            _StockInfoDB = new StockInfoDB(_Configuration);
        }
        public async Task Proccess()
        {
            NetWorkTest TestNet = new NetWorkTest();
            if (TestNet.NetWorkConnetStatus())
            {
                _Log.Log("網路連線測試成功", ConsoleColor.Green);
                if (_StockInfoDB.ConnetTest)
                {
                    _Log.Log("DB連線測試成功", ConsoleColor.Green);
                    await  StockInfo_Get();
                }
                else
                {
                    _Log.Log("DB連線異常", ConsoleColor.Red);
                }
            }
            else
            {
                _Log.Log("網路連線異常，請確認網路是否連線", ConsoleColor.Red);
            }
        }

        #region 取得股票交易資訊
        public async Task StockInfo_Get()
        {
            _Log.Log("開始執行", ConsoleColor.Green);
            //取得要拉取資訊的公司
            List<StockIDModel> StockIDList = await StockIDByStatusTrue_Get();
            foreach (StockIDModel data in StockIDList)
            {
                #region 取得主力資訊
                try
                {
                    await MainForceInOut_Get(data);
                }
                catch
                {

                }
                #endregion
                try
                {
                    // 呼叫api
                    JObject APIResult = await StockApi_Call(data);
                    //拆解檔案內容
                    List<StockInfo> StockInfoList = DownloadData_Analysis(APIResult, data);
                    //存入db
                    DownloadData_Save(StockInfoList);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            _Log.Log("執行結束", ConsoleColor.Yellow);
        }
        #region 取得要拉取資訊的公司
        private async  Task<List<StockIDModel>> StockIDByStatusTrue_Get()
        {
            _Log.Log("取得要拉取資訊的公司");
            List<StockIDModel> StockIDList = null;

            try
            {
                StockIDList = (await _StockInfoDB.BatchSP_StockID_StatusTrue_Get()).ToList();
            }
            catch (Exception ex)
            {
                _Log.Log($"取得要拉取資訊的公司發生例外，{ex.Message}");
            }
            _Log.Log($"共取得 {StockIDList.Count} 間公司");
            return StockIDList;
        }
        #endregion

        #region 呼叫api
        private async Task<JObject> StockApi_Call(StockIDModel data, string SreachDate = null)
        {

            if (string.IsNullOrEmpty(SreachDate))
                SreachDate = DateTime.Now.ToString("yyyyMMdd");
            _Log.Log($"拉取{data.StockID}-{data.StockName}-{SreachDate}的股票資訊");
            string GetString = $"{_Configuration["StockInfo:APIUrl"].ToString()}?response=json&date={SreachDate}&stockNo={data.StockID}";
            _Log.Log(GetString, ConsoleColor.Green);
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
        private List<StockInfo> DownloadData_Analysis(JObject APIResult, StockIDModel StockData)
        {
            List<StockInfo> StockInfoList = new List<StockInfo>();
            if (APIResult.Property("stat") != null && APIResult["stat"].ToString() == "OK")
            {
                _Log.Log($"拉取成功，開始拆解json", ConsoleColor.Green);
                JProperty Data = APIResult.Property("data");
                if (Data != null)
                {
                    //StockInfo StockInfoData;
                    //DateTime dateout;
                    decimal convertout;
                    StockInfoList = Data.Value.AsJEnumerable().Select(x=>
                        new StockInfo {
                            StockID = StockData.StockID,
                            Date = DateTime.ParseExact(x[0].ToString(), "yyy/MM/dd", CultureInfo.InvariantCulture).AddYears(1911),
                            TradingVolume = decimal.TryParse(x[1].ToString().Replace(",", ""), out convertout) ? convertout : 0,
                            Turnover = decimal.TryParse(x[2].ToString().Replace(",", ""), out convertout) ? convertout : 0,
                            OpeningPrice = decimal.TryParse(x[3].ToString(), out convertout) ? convertout : 0,
                            HighestPrice = decimal.TryParse(x[4].ToString(), out convertout) ? convertout : 0,
                            LowestPrice = decimal.TryParse(x[5].ToString(), out convertout) ? convertout : 0,
                            ClosingPrice = decimal.TryParse(x[6].ToString(), out convertout) ? convertout : 0,
                            Spreads = decimal.TryParse(x[7].ToString(), out convertout) ? convertout : 0,
                            TransactionAmount = decimal.TryParse(x[8].ToString().Replace(",", ""), out convertout) ? convertout : 0,
                             }).ToList();
                    //foreach (JArray data in APIResult.Property("data").Value)
                    //{
                    //    StockInfoData = new StockInfo();
                    //    StockInfoData.StockID = StockData.StockID;
                    //    DateTime Datec = DateTime.ParseExact(data[0].ToString(), "yyy/MM/dd", CultureInfo.InvariantCulture).AddYears(1911);
                    //    StockInfoData.Date = Datec;
                    //    //DateTime.TryParse(data[0].ToString(),out dateout)?new DateTime(): dateout;
                    //    StockInfoData.TradingVolume = decimal.TryParse(data[1].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                    //    StockInfoData.Turnover = decimal.TryParse(data[2].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                    //    StockInfoData.OpeningPrice = decimal.TryParse(data[3].ToString(), out convertout) ? convertout : 0;
                    //    StockInfoData.HighestPrice = decimal.TryParse(data[4].ToString(), out convertout) ? convertout : 0;
                    //    StockInfoData.LowestPrice = decimal.TryParse(data[5].ToString(), out convertout) ? convertout : 0;
                    //    StockInfoData.ClosingPrice = decimal.TryParse(data[6].ToString(), out convertout) ? convertout : 0;
                    //    StockInfoData.Spreads = decimal.TryParse(data[7].ToString(), out convertout) ? convertout : 0;
                    //    StockInfoData.TransactionAmount = decimal.TryParse(data[8].ToString().Replace(",", ""), out convertout) ? convertout : 0;
                    //    StockInfoList.Add(StockInfoData);
                    //}
                }
            }
            _Log.Log($"拆解結束，共 {StockInfoList.Count} 筆");
            return StockInfoList;
        }
        #endregion

        #region 存入db
        private async Task DownloadData_Save(List<StockInfo> StockInfoList)
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();

            try
            {
                Result =await _StockInfoDB.BatchSP_ImportStockInfo(StockInfoList);
                if (Result["Code"].ToString() == "1")
                    _Log.Log($"{DateTime.Now}--Code：{Result["Code"].ToString()}，Msg：{Result["Message"].ToString()}", ConsoleColor.Green);
                else
                    _Log.Log($"{DateTime.Now}--Code：{Result["Code"].ToString()}，Msg：{Result["Message"].ToString()}", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                _Log.Log($"{DateTime.Now}--存入股票資訊例外，ex{ex}", ConsoleColor.Red);
            }
        }
        #endregion

        #endregion

        #region 取得主力進出資訊
        private async Task MainForceInOut_Get(StockIDModel data)
        {
            _Log.Log("開始拉取主力買賣資訊");
            HtmlDocument MainForceInOutHtmlInfo =await MainForceInOutHtmlInfo_Get(data.StockID);
            
            List<MainForceInOutInfo> MainForceInOutInfoList = MainForceInOutData_Analysis(MainForceInOutHtmlInfo, data.StockID);
            MainForceInOutData_Save(MainForceInOutInfoList);
            _Log.Log("拉取主力買賣資訊結束");
        }

        private async Task<HtmlDocument> MainForceInOutHtmlInfo_Get(string StockID)
        {
            StringBuilder Url = new StringBuilder("http://pchome.megatime.com.tw/stock/sto1/ock4/sid");
            Url.Append(StockID).Append(".html");
            string Parameter = "is_check=1";
            _Log.Log(Url.ToString(), ConsoleColor.Green);
            string Result =await _api.Post_async(Url.ToString(), ContentTypeEnum.urlencoded, Parameter);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Result);
            //webClient.Load(Url.ToString(),"POST");
            return doc;
        }

        public List<MainForceInOutInfo> MainForceInOutData_Analysis(HtmlDocument doc, string StockID)
        {
            List<MainForceInOutInfo> MainForceInOutInfoList = new List<MainForceInOutInfo>();
            StringBuilder XPath = new StringBuilder("//*[@id='mainprice']/table");
            int count = doc.DocumentNode.SelectNodes(XPath.ToString()).Count;
            DateTime DataDate;

            //以下取得日期失敗就不做
            if (!string.IsNullOrEmpty(doc.DocumentNode.SelectNodes($"//*[@id='update_date']")[0].InnerText))
            {
                string[] XPstring = doc.DocumentNode.SelectNodes($"//*[@id='update_date']")[0].InnerText.Split('：');
                if (XPstring.Count() > 1)
                {
                    if (!DateTime.TryParse(XPstring[1], out DataDate))
                        return MainForceInOutInfoList;
                }
                else
                    return MainForceInOutInfoList;
            }
            else
                return MainForceInOutInfoList;

            for (int i = 1; i <= count; i++)
            {
                string trXPath = $"{XPath}[{i}]/tbody/tr";
                int trcount = doc.DocumentNode.SelectNodes(trXPath).Count;
                MainForceInOutInfo data;
                for (int j = 2; j <= trcount; j++)
                {
                    string tdXPath = $"{trXPath}[{j}]/td";
                    int tdcount = doc.DocumentNode.SelectNodes(tdXPath).Count;
                    decimal convertD;
                    try
                    {
                        data = new MainForceInOutInfo();
                        data.StockID = StockID;
                        data.Buyers = doc.DocumentNode.SelectNodes($"{tdXPath}[{1}]/span")[0].InnerText.ToString();
                        data.BuyCount = decimal.TryParse(doc.DocumentNode.SelectNodes($"{tdXPath}[{2}]")[0].InnerText.ToString(), out convertD) ? convertD : 0;
                        data.SellCount = decimal.TryParse(doc.DocumentNode.SelectNodes($"{tdXPath}[{3}]")[0].InnerText.ToString(), out convertD) ? convertD : 0;
                        data.AverageBuyPrice = decimal.TryParse(doc.DocumentNode.SelectNodes($"{tdXPath}[{4}]")[0].InnerText.ToString(), out convertD) ? convertD : 0;
                        data.AverageSellPrice = decimal.TryParse(doc.DocumentNode.SelectNodes($"{tdXPath}[{5}]")[0].InnerText.ToString(), out convertD) ? convertD : 0;
                        data.OverPrice = decimal.TryParse(doc.DocumentNode.SelectNodes($"{tdXPath}[{6}]")[0].InnerText.ToString(), out convertD) ? convertD : 0;
                        data.Date = DataDate;

                        MainForceInOutInfoList.Add(data);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            return MainForceInOutInfoList;
        }


        public async Task MainForceInOutData_Save(List<MainForceInOutInfo> MainForceInOutInfoList)
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();
            try
            {
                Result =await _StockInfoDB.BatchSP_ImportStockInfo(MainForceInOutInfoList);
                if (Result["Code"].ToString() == "1")
                    _Log.Log($"{DateTime.Now}--主力買賣資訊，Code：{Result["Code"].ToString()}，Msg：{Result["Message"].ToString()}", ConsoleColor.Green);
                else
                    _Log.Log($"{DateTime.Now}--主力買賣資訊，Code：{Result["Code"].ToString()}，Msg：{Result["Message"].ToString()}", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                _Log.Log($"{DateTime.Now}--主力買賣資例外，ex{ex}", ConsoleColor.Red);
            }
        }
        #endregion
    }
}
