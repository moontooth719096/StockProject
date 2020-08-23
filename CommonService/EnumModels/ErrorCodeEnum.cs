using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageSend.Enum
{
    public enum ErrorCodeEnum
    {
        Success = 1,
        Error = -1,
        ParameterisNull = -2,
        ExceptionError = -99,
    }
}
