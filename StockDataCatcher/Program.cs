using CommonService.Helper;
using CommonService.IHelper;
using CommonService.Models.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockDataCatcher.Helper;
using StockDataCatcher.IService;
using StockDataCatcher.Service;
using System;
using System.Net.Http;

namespace StockDataCatcher
{
    class Program
    {
        static IConfiguration config;
        static ServiceProvider serviceProvider;
        static void Main(string[] args)
        {
            CongifSetting();
            ServiceSetting();

            IHttpClientFactory httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            IStockService stockService = serviceProvider.GetService<IStockService>();
            stockService.Proccess();
            //FileModel FileData = new FileModel("","fuck");
            //IFile FileService = new TxtFileService();
            //FileService.Write(FileData,"Fuckyou");
            //ConsoleLogHelper Loghelper = new ConsoleLogHelper();
            //Loghelper.Log("test", ConsoleColor.Red);
            Console.ReadKey();
        }

        private static void CongifSetting()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("APISetting.json", optional: true, reloadOnChange: true)
                .AddJsonFile("DBSetting.json", optional: true, reloadOnChange: true)
                .Build();
        }

        private static void ServiceSetting()
        {
            serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<IStockService, StockService>()
                .AddSingleton<ConsoleLogHelper>()
                .AddSingleton<APIHelperV2>()
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();
        }

    }
}
