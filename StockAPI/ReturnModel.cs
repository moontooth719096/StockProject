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
        public int Code
        { get { return (int)APICode; } }
        public string CodeMesage
        { get { return APICode.ToString(); } }
        public string DetilMessage { get; set; }
    }
}
