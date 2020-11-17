using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.EnumClass
{
    public enum APICodeEnum
    {
        Success = 1,
        Fail = -1,
        ParameterError = -2,
        DBError = -3,
        ExceptionError = -99
    }
}
