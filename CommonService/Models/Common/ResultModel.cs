using CommonService.EnumModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.Models.Common
{
    public class ResultModel
    {
        public ResultCodeEnum ResultStatus { get; set; }
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int Code => (Int32)ResultStatus;
        /// <summary>
        /// 介面錯誤訊息(秀給使用者看的)
        /// </summary>
        public string ShowMessage { get; set; }
        /// <summary>
        /// 內部錯誤訊息(給開發者看的)
        /// </summary>
        public string DebugMessage  => ResultStatus.ToString();
        /// <summary>
        /// 詳細描述
        /// </summary>
        public string Description { get; set; }
    }
}
