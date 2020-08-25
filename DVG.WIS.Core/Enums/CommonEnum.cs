using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum StatusEnum
    {
        [Description("Không hoạt động")]
        Deactive = 0,
        [Description("Đang hoạt động")]
        Active = 1
    }

    public enum LanguageEnum
    {
        [Description("Tiếng Việt")]
        Vi = 0,
        [Description("Tiếng Anh")]
        En = 1
    }

    public enum SqlTypeEnum
    {
        [Description("PostgreSQL")]
        Postgre = 0,
        [Description("MSSQL")]
        MsSql = 1
    }
}
