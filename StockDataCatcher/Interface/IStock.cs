using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDataCatcher.Interface
{
    interface IStock
    {
        Task DataProccess();
        Task DataProccess_UserSeting(IList<DateTime> DatetimeList);
        Task Warming(string Message);
    }
}
