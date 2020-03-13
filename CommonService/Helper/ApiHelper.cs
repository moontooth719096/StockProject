using CommonService.EnumModels;
using CommonService.Models.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CommonService.Helper
{
    public static class ApiHelper
    {

        #region Get(string serviceUrl)
        public static JObject Get(string serviceUrl)
        {
            JObject result = new JObject();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrl);
                request.Method = "GET";

                //API回傳的字串
                using (var response = (HttpWebResponse)request.GetResponse())
                {

                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string responseStr = sr.ReadToEnd();
                        result = JObject.Parse(responseStr);
                        return result;
                    }//end using  
                }//end using
            }
            catch (Exception)
            {

            }
            return result;
        }
        #endregion

        #region Header
        public static List<PostHeader> HeaderCombin(Dictionary<string, string> HeadSetValue)
        {
            List<PostHeader> headerList = new List<PostHeader>();
            PostHeader data = new PostHeader();
            foreach (var head in HeadSetValue)
            {
                data.HeaderKey = head.Key;
                data.HeaderContent = head.Value;
            }
            //data.HeaderKey = "Authorization";
            //data.HeaderContent = $"Bearer {this._Token}";
            headerList.Add(data);
            return headerList;
        }
        #endregion

        #region Post
        /// <summary>
        /// post
        /// </summary>
        /// <param name="ApiUrl">API接口</param>
        /// <param name="param">傳遞參數</param>
        /// <param name="ContentType">ContentType類型</param>
        /// <param name="HeaderList">Header資訊</param>
        /// <param name="Timeout">設定逾時秒數</param>
        /// <returns>回傳結果</returns>
        public static string Post(string ApiUrl, ContentTypeEnum ContentType, string param = null, List<PostHeader> HeaderList = null,string Timeout=null)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
                request.Method = "POST";
                switch (ContentType)
                {
                    case ContentTypeEnum.urlencoded:
                        request.ContentType = "application/x-www-form-urlencoded";
                        break;
                    case ContentTypeEnum.json:
                        request.ContentType = "application/json";
                        break;
                }
                if (HeaderList != null && HeaderList.Count > 0)
                {
                    foreach (PostHeader data in HeaderList)
                    {
                        request.Headers[data.HeaderKey] = data.HeaderContent;
                    }
                }
                
                if (!string.IsNullOrEmpty(Timeout))
                {
                    int timeoutvalue;
                    if (int.TryParse(Timeout,out timeoutvalue))
                    {
                        request.Timeout = timeoutvalue*1000;
                }
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(param);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);

                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }
        #endregion

        #region PATCH
        /// <summary>
        /// post
        /// </summary>
        /// <param name="ApiUrl">API接口</param>
        /// <param name="param">傳遞參數</param>
        /// <param name="ContentType">ContentType類型</param>
        /// <param name="HeaderList">Header資訊</param>
        /// <returns>回傳結果</returns>
        public static JObject PATCH(string ApiUrl, ContentTypeEnum ContentType, string param = null, List<PostHeader> HeaderList = null)
        {
            JObject result = new JObject();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiUrl);
                request.Method = "PATCH";
                switch (ContentType)
                {
                    case ContentTypeEnum.urlencoded:
                        request.ContentType = "application/x-www-form-urlencoded";
                        break;
                    case ContentTypeEnum.json:
                        request.ContentType = "application/json";
                        break;
                }
                if (HeaderList != null && HeaderList.Count > 0)
                {
                    foreach (PostHeader data in HeaderList)
                    {
                        request.Headers[data.HeaderKey] = data.HeaderContent;
                    }
                }
                
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                if(param!=null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(byteArray, 0, byteArray.Length);

                    }
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = JObject.Parse(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }
        #endregion
    }

}
