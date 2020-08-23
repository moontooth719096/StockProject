using CommonService.EnumModels;
using CommonService.Models.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Helper
{
    public class APIService
    {
        public readonly IHttpClientFactory _clientF;
        public APIService(IHttpClientFactory ClientF)
        {
            _clientF = ClientF;
        }

        #region Get
        public async Task<string> Get_async(string Url, List<PostHeader> HeaderList = null)
        {
            string responseContent = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                var _httpClient = _clientF.CreateClient();

                if (HeaderList != null && HeaderList.Count > 0)
                {
                    foreach (PostHeader data in HeaderList)
                    {
                        _httpClient.DefaultRequestHeaders.Add(data.HeaderKey, data.HeaderContent);
                    }
                }
                response = await _httpClient.GetAsync(Url);
                responseContent = Result_Proccess(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseContent;
            
        }
        #endregion

        #region Post

        #region Header
        public List<PostHeader> HeaderCombin(Dictionary<string, string> HeadSetValue)
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

        public async Task<string> Post_async(string Url, ContentTypeEnum ContentType, string Pramater=null, List<PostHeader> HeaderList = null)
        {
            string responseContent = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                var _httpClient = _clientF.CreateClient();

                if (HeaderList != null && HeaderList.Count > 0)
                {
                    foreach (PostHeader data in HeaderList)
                    {
                        _httpClient.DefaultRequestHeaders.Add(data.HeaderKey, data.HeaderContent);
                    }
                }

                StringContent httpContent = new StringContent(Pramater, Encoding.UTF8);
                switch (ContentType)
                {
                    case ContentTypeEnum.urlencoded:
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        break;
                    case ContentTypeEnum.json:
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        break;
                }

                response = await _httpClient.PostAsync(Url, httpContent);
                responseContent = Result_Proccess(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseContent;
        }
        #endregion

        private string Result_Proccess(HttpResponseMessage RequestResult)
        {
            string Result = "";
            if (RequestResult != null && RequestResult.IsSuccessStatusCode)
            {
                var readTask = RequestResult.Content.ReadAsByteArrayAsync().Result;
                Result = System.Text.Encoding.UTF8.GetString(readTask, 0, readTask.Length);
            }
           // Console.WriteLine(Result);
            return Result;
        }
    }
}
