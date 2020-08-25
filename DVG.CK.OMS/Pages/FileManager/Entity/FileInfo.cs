using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.CK.OMS.Pages.FileManager.Entity
{
    public class FileInfo
    {
        public bool Result { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string FullOriginalPath { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string Message { get; set; }
    }
}