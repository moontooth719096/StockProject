using CommonService.EnumModels;
using CommonService.IHelper;
using CommonService.Models.Common;
using System;
using System.IO;

namespace StockDataCatcher.Helper
{
    public class TxtFileService:IFile
    {
        public ResultModel CreateFile(FileModel FileData)
        {
            ResultModel Result = new ResultModel();
            try
            {
                //確認檔案路徑是否存在
                if (Directory.Exists(FileData.Path) == false)
                {
                    Directory.CreateDirectory(FileData.Path);//不存在就產生
                }
                FileStream fs = File.Create(FileData.FilePath);
                fs.Close();
                Result.ResultStatus = ResultCodeEnum.Success;
                Result.ShowMessage = $"新增 {FileData.FileName} 成功";
            }
            catch (Exception ex)
            {
                ex.ToString();
                Result.ResultStatus = ResultCodeEnum.Exception;
                Result.ShowMessage = $"新增 {FileData.FileName} 發生例外";
                Result.Description = $"ex：{ex}";
            }
            return Result;
        }
        public ResultModel Write(FileModel FileData, string Message)
        {
            ResultModel Result = new ResultModel();
            //判斷檔案存在
            if (!File.Exists(FileData.FilePath))
            {
                //不存在就新增
                Result = CreateFile(FileData);
                if (Result.Code ==1 || Result.Code==-5)
                    Result = new ResultModel();
                else
                    return Result;
                using (StreamWriter sw = new StreamWriter(FileData.FilePath))    
                {
                    sw.WriteLineAsync(Message);
                }
            }
            return Result;
        }

        public object Read(FileModel FileData)
        {
            throw new NullReferenceException();
        }
    }
}
