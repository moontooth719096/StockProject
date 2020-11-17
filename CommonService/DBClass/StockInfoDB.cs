using CommonService.DBClass.DBBase;
using CommonService.Models.StockInfoDB;
using Dapper;
using DapperParameters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CommonService.DBClass
{
    public class StockInfoDB : DBConnection
    {
        public StockInfoDB(IConfiguration Configuration)
        {
            base.DbName = "StockInfoDB";//DBName
            _Configuration = Configuration;//json檔的設定資料
            Connection(DbName);//查詢相關DB資料
        }



        #region 整批新增股票資訊
        public async Task<Dictionary<string, object>> BatchSP_ImportStockInfo(IList<StockInfo> StockInfoList)
        {
            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                parameters.AddTable<StockInfo>("@importTable", "TB_ImporeStockInfo", StockInfoList);
                parameters.Add("@Code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Msg", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                DynamicParameters Result =await SystemDB.DB_Action_Output(str_conn, "BatchSP_ImportStockInfoV2",parameters);
                int code = Result.Get<int>("@Code");
                string message = Result.Get<string>("@Msg");
                result.Add("Code", code);
                result.Add("Message", message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region 查詢資料範例
        public async Task<IEnumerable<StockIDModel>> BatchSP_StockID_StatusTrue_Get()
        {
            return await SystemDB.DB_SPAction<StockIDModel>(str_conn, "BatchSP_StockID_StatusTrue_Get", null);
        }
        #endregion

        #region 整批新增主力買賣資訊
        public async Task<Dictionary<string, object>> BatchSP_ImportStockInfo(List<MainForceInOutInfo> MainForceInOutInfoList)
        {
            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                parameters.AddTable<MainForceInOutInfo>("@importTable", "TB_ImporeMainForceInOutInfo", MainForceInOutInfoList);
                parameters.Add("@Code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Msg", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                DynamicParameters Result = await SystemDB.DB_Action_Output(str_conn, "BatchSP_ImportMainForceInOutInfo", parameters);
                int code = Result.Get<int>("@Code");
                string message = Result.Get<string>("@Msg");
                result.Add("Code", code);
                result.Add("Message", message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region 變更股票使用狀態
        public async Task<Dictionary<string, object>> BackEndSP_StockID_Update(string StockID,int status)
        {
            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                parameters.Add("@StockID", StockID, dbType: DbType.String);
                parameters.Add("@Status", status, dbType: DbType.Int32);

                parameters.Add("@Code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Msg", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                DynamicParameters Result = await SystemDB.DB_Action_Output(str_conn, "BackEndSP_StockID_Update", parameters);
                int code = Result.Get<int>("@Code");
                string message = Result.Get<string>("@Msg");
                result.Add("Code", code);
                result.Add("Message", message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

    }
}
