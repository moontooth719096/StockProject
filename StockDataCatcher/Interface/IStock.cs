using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDataCatcher.Interface
{
    interface IStock
    {
        Task DataProccess();

        Task Warming(string Message);
    }
}
