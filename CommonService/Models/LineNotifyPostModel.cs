using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonService.Models
{
    public class LineNotifyPostModel
    {
        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 要發送的群組代號
        /// </summary>
        public string GroupKey { get; set; }
        /// <summary>
        /// 貼圖包ID
        /// https://devdocs.line.me/files/sticker_list.pdf  貼圖包連結
        /// </summary>
        public int stickerPackageId { get; set; }
        /// <summary>
        /// 貼圖ID
        /// </summary>
        public int stickerId { get; set; }
        /// <summary>
        /// 縮圖網址
        /// </summary>
        public string imageThumbnail { get; set; }
        /// <summary>
        /// 圖片位置
        /// </summary>
        public string imageFullsize { get; set; }
    }
}
