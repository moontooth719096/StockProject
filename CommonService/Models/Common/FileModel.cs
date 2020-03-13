using System;
using System.Collections.Generic;
using System.Text;

namespace CommonService.Models.Common
{
    public class FileModel
    {
        
        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = string.IsNullOrEmpty(value) ? "\\" : value;
                this.FilePath = $"{_Path}{this.FileName}";
                
            }
        }
        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                this.FilePath = $"{this.Path}{value.ToString()}";
                _FileName = value;
            }
        }


        public string FilePath { get; set; }
        public readonly string FileType = "txt";

        public FileModel(string Path, string FileName)
        {
            this.Path = Path;
            this.FileName = $"{FileName}.{FileType}";
        }
    }
}
