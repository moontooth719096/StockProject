using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Helper
{
    public  class APIHelperV2
    {
        IHttpClientFactory _IHCF;
        public APIHelperV2(IHttpClientFactory IHCF)
        {
            _IHCF = IHCF;
        }

        public JObject Get(string Url)
        {
            //HttpClient _httpClient = _IHCF.CreateClient();
            //var result = _httpClient.GetAsync(Url).Wait(); ;
            //return result;
           return ApiHelper.Get(Url);
        }
    }
}
