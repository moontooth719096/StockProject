using CommonService.DBClass;
using CommonService.Models.StockInfoDB;
using Newtonsoft.Json.Linq;
using StockAPI.EnumClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace StockAPI.Controllers.Services
{
    public class StockIDataService
    {
        protected readonly StockInfoDB _stockDB;
        public StockIDataService(StockInfoDB stockDB)
        {
            _stockDB = stockDB;
        }
        public async Task<IEnumerable<StockIDModel>> StockInfo_Get()
        {
            return await _stockDB.BatchSP_StockID_StatusTrue_Get();
        }

        public async Task<ReturnModel> StockID_Update(string StockID, int status)
        {
            Dictionary<string, object>  DB_Result= await _stockDB.BackEndSP_StockID_Update(StockID,status);
            if (DB_Result != null && DB_Result["Code"].ToString() == "1")
            {
                return new ReturnModel {
                    APICode = APICodeEnum.Success
                };
            }
            else
            {
                return new ReturnModel {
                    APICode = APICodeEnum.DBError,
                    DetilMessage = DB_Result["Message"].ToString()
                };
            }
        }
    }
}
