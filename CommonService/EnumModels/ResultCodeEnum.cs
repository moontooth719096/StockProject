using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.EnumModels
{
    public enum ResultCodeEnum
    {
        Defale = 0,
        Success = 1,
        Error = -1,
        ParameterNull = -2,
        ParameterError = -3,
        SignError = -4,
        FileExist = -5,
        PathNotExist = -6,
        Exception = -99
    }
}
