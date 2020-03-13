using System;
using System.Collections.Generic;
using System.Text;

namespace StockDataCatcher.Helper
{
    public class ConsoleLogHelper
    {
        //public void Log(string Message, ConsoleColor FontColor = ConsoleColor.White)
        //{
        //    Console.ForegroundColor = FontColor;
        //    Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}--{Message}");
        //}

        public void Log(string Message, ConsoleColor FontColor = ConsoleColor.White , ConsoleColor BackGroundColor= ConsoleColor.Black)
        {
            Console.BackgroundColor = BackGroundColor;
            Console.ForegroundColor = FontColor;
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}--{Message}");
        }

        public void Log(string Message, string ClassName, string FunctionName, ConsoleColor FontColor = ConsoleColor.White)
        {
            Console.ForegroundColor = FontColor;
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}--{Message}");
        }
    }
}
