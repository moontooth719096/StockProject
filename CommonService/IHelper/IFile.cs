using CommonService.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.IHelper
{
    public interface IFile
    {

        ResultModel Write(FileModel FileData, string Message);
        Object Read(FileModel FileData);
    }
}
