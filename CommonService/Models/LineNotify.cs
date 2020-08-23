namespace CommonService.Models
{
    public class LineNotify
    {
        //private string _ClientID = "wktcfK3VHyQTNlomnd4O9L";
        //private string _ClinetSecret = "qfi2yshR6enaFvctwL7r7Az5lUDo2fKk9pulvRssEu5";

        #region accesstoken
        //private string _grant_type = "authorization_code";
        //private string _Code = "h1iJJXp7WtAOY6PFYiPWex";
        #endregion

        #region sendmessage
        private string _LineNotifySendMessageURL = "https://notify-api.line.me/api/notify";
        private string _Token { get; set; }
        //private string _Token = "4LlyQ9eHnsOJGLd8GNuMCufX0ZRocxMmpQ1WKHqLcZ0";//風控管理帳號
        #endregion

        public string LineNotifySendMessageURL_Get()
        {
            return this._LineNotifySendMessageURL;
        }


        #region 取得AccessToken
        //public string AccessTokenParameter_Combin(LineNotifyPostModel PostValue)
        //{
        //    StringBuilder Parameterstring = new StringBuilder();
        //    Parameterstring.Append($"grant_type={this._grant_type}");
        //    if (PostValue.stickerPackageId != 0)
        //    {
        //        Parameterstring.Append($"&stickerPackageId={PostValue.stickerPackageId}");
        //        Parameterstring.Append($"&stickerId={PostValue.stickerId}");
        //    }
        //    if (!string.IsNullOrEmpty(PostValue.imageThumbnail))
        //    {
        //        Parameterstring.Append($"&imageThumbnail={PostValue.imageThumbnail}");
        //        Parameterstring.Append($"&imageFullsize={PostValue.imageFullsize}");
        //    }

        //    return Parameterstring.ToString();
        //}
        #endregion

        #region 發送訊息

        /// <summary>
        /// 取得要發送訊息的群組token
        /// </summary>
        /// <returns></returns>
        public string GroupToken_Get(string TokenID)
        {
            //string result = new NiceEventDB().LineNotifyAccessToken_Get(TokenID);
            //if (string.IsNullOrEmpty(result))
            //{
            //    return "查無AccessToken";
            //}
            //else
            this._Token = "UEC7oqXgUHHnVJw6USRHZSCA1luTR9F8anCVbdwKUJ9";
            return "";
        }

        
        #endregion
    }
}
