using CommonService.EnumModels;
using CommonService.Helper;
using CommonService.Models;
using MessageSend.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Services
{
    public class LineNotifyService
    {
        private readonly APIService _api;
        private readonly string _Token = "gWlf4thguhSZP5af6FF6qiyB24CNk0lujfY906UXC2f";
        public LineNotifyService(APIService api)
        {
            _api = api;
        }       

        #region 發送訊息
        public async Task LineNotify_Send(LineNotifyPostModel value)
        {
            string result = string.Empty;
            JObject PostResult = new JObject();
            try
            {
                LineNotify LineNotifyService = new LineNotify();//宣告LineNotify服務
                string CheckResult = LineNotify_Check(value);
                if (!string.IsNullOrEmpty(CheckResult))
                {
                    PostResult.Add("Code", (int)ErrorCodeEnum.ParameterisNull);
                    PostResult.Add("Message", CheckResult);
                    return;
                }
                Dictionary<string, string> HeadList = SendMessageHeaderCombin();
                string Message = SendMessageParameter_Combin(value);
                //呼叫傳送訊息API
                await _api.Post_async(LineNotifyService.LineNotifySendMessageURL_Get(), ContentTypeEnum.urlencoded, Message, _api.HeaderCombin(HeadList));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string LineNotify_Check(LineNotifyPostModel value)
        {
            string Message = string.Empty;
            if (string.IsNullOrEmpty(value.Message))
            {
                return Message = $"你Message為空，沒要發訊息不要來亂好嗎";
            }
            
            if (value.stickerPackageId != 0)
            {
                if (value.stickerId == 0)
                    return Message = $"stickerId不可為0";
            }
            if (!string.IsNullOrEmpty(value.imageThumbnail))
            {
                if (string.IsNullOrEmpty(value.imageFullsize))
                    return Message = $"imageFullsize不可為空";
            }
            return Message;
        }
        /// <summary>
        /// 組合Header
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> SendMessageHeaderCombin()
        {
            Dictionary<string, string> headerList = new Dictionary<string, string>();
            headerList.Add("Authorization", $"Bearer {this._Token}");
            return headerList;
        }

        /// <summary>
        /// 組合參數
        /// </summary>
        /// <param name="PostValue"></param>
        /// <returns></returns>
        public string SendMessageParameter_Combin(LineNotifyPostModel PostValue)
        {
            StringBuilder Parameterstring = new StringBuilder();
            Parameterstring.Append($"Message={PostValue.Message}");
            if (PostValue.stickerPackageId != 0)
            {
                Parameterstring.Append($"&stickerPackageId={PostValue.stickerPackageId}");
                Parameterstring.Append($"&stickerId={PostValue.stickerId}");
            }
            if (!string.IsNullOrEmpty(PostValue.imageThumbnail))
            {
                Parameterstring.Append($"&imageThumbnail={PostValue.imageThumbnail}");
                Parameterstring.Append($"&imageFullsize={PostValue.imageFullsize}");
            }

            return Parameterstring.ToString();
        }
        #endregion
    }
}
