using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    [Description("Hành động người dùng")]
    public enum ActivityTypeEnum
    {
    
        [Description("Đăng nhập")]
        Login = 100, // Done
        [Description("Đăng xuất")]
        Logout = 101, // Done
        [Description("Đổi mật khẩu")]
        ChangePassword = 102, // Done
        [Description("Phân quyền cho user khác")]
        ChangePermission = 103, // Chua lam chuc nang nay

        [Description("Thêm ngân hàng mới")]
        AddBank = 200, // Done
        [Description("Sửa thông tin ngân hàng")]
        EditBank = 201,  // Done
        [Description("Xóa thông tin ngân hàng")]
        DeleteBank = 202, // Khong co tinh nang nay

        [Description("Thêm gói vay mới")]
        AddLoanPackage = 300, // Done
        [Description("Sửa thông tin gói vay")]
        EditLoanPackage = 301,// Done
        [Description("Xóa thông tin gói vay")]
        DeleteLoanPackage = 302,// Done

        [Description("Thêm gói tiết kiệm mới")]
        AddSavePackage = 400, // Done
        [Description("Sửa thông tin gói tiết kiệm")]
        EditSavePackage = 401, // Done
        [Description("Xóa thông tin gói tiết kiệm")]
        DeleteSavePackage = 402, // Done

        [Description("Thêm thẻ ATM mới")]
        AddDebitCard = 500,// Done
        [Description("Sửa thông tin thẻ ATM")]
        EditDebitCard = 501,// Done
        [Description("Xóa thông tin thẻ ATM")]
        DeleteDebitCard = 502, // Done

        [Description("Thêm thẻ tín dụng mới")]
        AddCreditCard = 600, // Done
        [Description("Sửa thông tin thẻ tín dụng")]
        EditCreditCard = 601, // Done
        [Description("Xóa thông tin thẻ tín dụng")]
        DeleteCreditCard = 602,  // Done

        [Description("Thêm tin mới")]
        AddNews = 700,  // Done
        [Description("Sửa tin bài")]
        EditNews = 701,  // Done
        [Description("Xóa tin bài")]
        DeleteNews = 702,  // Done
        [Description("Xuất bản tin bài")]
        ApprovedNews = 703,  // Done
        [Description("Gỡ tin bài")]
        DownNews = 704,  // Done
        [Description("Xóa tin bài vĩnh viễn")]
        RemoveNews = 705,  // Done
        [Description("Sửa tin bài landing page")]
        EditNewsLanding = 800,  // Done
        [Description("Gỡ tin bài landing page")]
        DownNewsLanding = 801,  // Done
        [Description("Xuất bản tin bài landing page")]
        ApprovedNewsLanding = 802  // Done
    }
}
