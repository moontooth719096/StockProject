using CommonService.DBClass.DBBase;
using Dapper;
using DapperParameters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CommonService.DBClass
{
    public class Sample : DBConnection
    {
        public Sample(IConfiguration Configuration)
        {
            base.DbName = "";//DBName
            _Configuration = Configuration;//json檔的設定資料
            Connection(DbName);//查詢相關DB資料
        }

        #region output範例

        public async Task<Dictionary<string, object>> APISP_Live_UserPermissionAdd(string name,string email,int status,int userlevel,int editor)
        {
            DynamicParameters parameters = new DynamicParameters();
            Dictionary<string, object> result = new Dictionary<string, object>();
            parameters.Add("@username", name, dbType: DbType.String);
            parameters.Add("@email", email, dbType: DbType.String);
            parameters.Add("@status", status, dbType: DbType.Int32);
            parameters.Add("@userlevel", userlevel, dbType: DbType.Int32);
            parameters.Add("@editor", editor, dbType: DbType.Int32);
            parameters.Add("@Code", dbType: DbType.Int32, size: 100, direction: ParameterDirection.Output);
            parameters.Add("@MSG", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
            DynamicParameters Result = await SystemDB.DB_Action_Output(str_conn, "APISP_Live_UserPermissionAdd" ,parameters);
            int code = parameters.Get<int>("@Code");
            string message = parameters.Get<string>("@MSG");
            result.Add("Code", code);
            result.Add("Message", message);
            return result;
        }


        #endregion

        #region 批量sp操作範例
        //public List<LiveDBModel> BatchSP_Live_SessionCurrentStateGet(List<AppSetupModel> LiveStatusOpen) 
        //{
        //    var lista = LiveStatusOpen.Select(x => new DatatableModel { Appid = x.Appid, AppName = null }).ToList();
        //    var parameters = new DynamicParameters();
        //    parameters.AddTable("@appidlist", "AppIDList", lista);
        //    return SystemDB.DB_SPAction<LiveDBModel>(str_conn, "BatchSP_Live_SessionCurrentStateGet", parameters);
        //}
        #endregion

        #region 查詢資料範例
        //public List<InfoModel> APISP_Live_StreamerInfoGet(int UID)
        //{
        //    DynamicParameters param = new DynamicParameters();
        //    param.Add("@uid", UID, dbType: DbType.Int32);
        //    return SystemDB.DB_SPAction<InfoModel>(str_conn, "APISP_Live_StreamerInfoGet", param);
        //}
        #endregion

    }
}
