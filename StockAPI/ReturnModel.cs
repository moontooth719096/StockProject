using StockAPI.EnumClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI
{
    public class ReturnModel
    {
        public APICodeEnum APICode { get; set; }
        public int Code => (Int32)APICode;
        public string CodeMesage => APICode.ToString();
        public string DetilMessage { get; set; }
    }
}
