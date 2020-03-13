using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace CommonService.Models.Common
{
    public class NetWorkTest
    {
        string[] DNSS = new string[]{ "www.google.tw", "zh-tw.facebook.com" };

        public bool NetWorkConnetStatus()
        {
            int SuccessCount = 0;
            //Ping網站
            Ping p = new Ping();
            //網站的回覆
            PingReply reply;
            foreach (string DNS in DNSS)
            {
                try
                {
                    //取得網站的回覆
                    reply = p.Send(DNS);
                    //如果回覆的狀態為Success則return true
                    if (reply.Status == IPStatus.Success) { SuccessCount++; };
                }
                //catch這裡的Exception, 是有可能網站當下的某某狀況造成, 可以直接讓它傳回false.
                //或在重覆try{}裡的動作一次
                catch { }
            }
            if (SuccessCount > 0)
                return true;
            else
                return false;
        }
    }
}
