using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public enum UserTypeEnum
    {
        [Description("Quản trị viên")]
        Staff = 1,
        [Description("Admin")]
        Admin = 2,
        [Description("Quản lý nhà hàng")]
        Manager = 3,
        [Description("Chăm sóc khách hàng")]
        CustomerService = 4,
        [Description("Thu ngân")]
        Cashier = 5,
        [Description("Nhân viên bếp")]
        Kitchen = 6,
        [Description("Quản lý bếp")]
        KitchenManager = 7,
        [Description("Checkfood")]
        Checkfood = 8,
    }


    public enum UserStatusAdmin
    {
        [Description("Không hoạt động")]
        Deactived = 0,
        [Description("Đang hoạt động")]
        Actived = 1,
        [Description("Đã xóa")]
        Deleted = 2
    }
}
