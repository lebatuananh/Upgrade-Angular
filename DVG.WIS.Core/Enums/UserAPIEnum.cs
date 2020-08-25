using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Enums
{
    public enum UserAPIStatus
    {
        [Description("Lỗi! Yêu cầu không thể thực hiện")]
        Error = 0,
        [Description("")]
        Success = 1,
        [Description("Tài khoản đã tồn tại và bị xóa trước đó. Bạn không được phép thao tác tiếp với tài khoản này")]
        AccountDeleted = 2,
        [Description("Tài khoản không tồn tại")]
        AccountNotExist = 3,
        [Description("Tài khoản này đã tồn tại trên hệ thống.")]
        AccountExisted = 4
    }
   
}
