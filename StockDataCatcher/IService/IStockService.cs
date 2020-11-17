using System.Threading.Tasks;

namespace StockDataCatcher.IService
{
    public interface IStockService
    {
       Task Proccess();
       Task StockInfo_Get();
    }
}