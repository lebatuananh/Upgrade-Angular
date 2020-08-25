using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.WIS.Core.Enums
{
    public enum ProductStatusEnum
    {
        [Description("Chưa duyệt")]
        NotApproved = 0,
        [Description("Đã duyệt")]
        Approved = 1 
    }

}
