using CommonService.DBClass;
using CommonService.Helper;
using CommonService.Models.StockInfoDB;
using CommonService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockDataCatcher.Helper;
using StockDataCatcher.Interface;
using StockDataCatcher.IService;
using StockDataCatcher.Models.Stock;
using StockDataCatcher.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockDataCatcher
{
     class Program
    {
        static IConfiguration config;
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
#if(DEVELOP)
            Console.WriteLine("DEVELOPMode");
#endif
            CongifSetting();
            ServiceSetting();
            IStockService stockService = serviceProvider.GetService<IStockService>();
            //IList<DateTime> date = new List<DateTime>() {
            //   new DateTime(2020,08,01)
            //};
            foreach (IStock service in serviceProvider.GetServices<IStock>())
            {
                service.DataProccess();
                //service.DataProccess_UserSeting(date);
            }
            Console.ReadKey();
        }

        private static void CongifSetting()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("APISetting.json", optional: true, reloadOnChange: true)
#if DEVELOP
                .AddJsonFile("DevelopDBSetting.json", optional: true, reloadOnChange: true)
#else
                .AddJsonFile("DBSetting.json", optional: true, reloadOnChange: true)
#endif
                .Build();
        }

        private static void ServiceSetting()
        {
            List<StockIDModel> data = new List<StockIDModel> {
                new StockIDModel{ StockID = "1907",StockName="永豐餘"},
                new StockIDModel{ StockID = "2354",StockName="鴻準"},
                new StockIDModel{ StockID = "3017",StockName="錡鋐"},
                new StockIDModel{ StockID = "2344",StockName="華邦電"}
            };
            serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<ConsoleLogHelper>()
                .AddSingleton<APIService>()
                .AddSingleton<LineNotifyService>()
                .AddSingleton<List<StockIDModel>>(data)
                .AddSingleton<StockInfoDB>()
                .AddSingleton<IStock, StockTradeInfoService>()
                .BuildServiceProvider();
        }

    }
}
