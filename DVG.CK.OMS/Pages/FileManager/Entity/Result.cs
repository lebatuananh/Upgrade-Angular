using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CK.OMS.Pages.FileManager.Entity
{
    public class ResultReturn
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public IList<FolderInfo> FolderInfos { get; set; }
        public IList<FileInfo> FileInfos { get; set; }
    }
}
