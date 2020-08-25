using DVG.WIS.Entities.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.WIS.Entities
{
    public enum ErrorCodes
    {
        #region common
        [Description("Lỗi nghiệp vụ")]
        BusinessError = 500,
        [Description("Lỗi chưa xác định")]
        UnknowError = 501,
        [Description("Yêu cầu không hợp lệ")]
        InvalidRequest = 502,
        [Description("Lỗi ngoại lệ (Exception)")]
        Exception = 503,
        [Description("Lỗi trạng thái đã bị thay đổi hoặc không thể thay đổi trạng thái")]
        StatusError = 504,
        [Description("Lỗi khi gọi API từ bên Ahamove")]
        AhamoveError = 505,
        [Description("Đơn hàng đã được gọi sang Ahamove")]
        AhamoveExistError = 506,
        [Description("Thành công")]
        Success = 0,
        #endregion

        #region UpdateAccount
        [Description("Không tìm thấy thông tin tài khoản")]
        UpdateAccountUserNotFound = 1000,
        [Description("Tên đăng nhập không hợp lệ")]
        UpdateAccountInvalidUsername = 1001,
        [Description("Mật khẩu không hợp lệ")]
        UpdateAccountInvalidPassword = 1002,
        [Description("Mật khẩu nhập lại không chính xác")]
        UpdateAccountInvalidRetypePassword = 1003,
        [Description("Địa chỉ Email không hợp lệ")]
        UpdateAccountInvalidEmail = 1004,
        [Description("Số điện thoại di động không hợp lệ")]
        UpdateAccountInvalidMobile = 1005,
        [Description("Tên đăng nhập đã tồn tại")]
        UpdateAccountUsernameExists = 1006,
        [Description("Địa chỉ email đã tồn tại")]
        UpdateAccountEmailExists = 1007,
        [Description("Mật khẩu cũ không chính xác")]
        UpdateAccountInvalidOldPassword = 1008,
        [Description("Không tìm thấy thông tin quyền")]
        UpdateAccountPermissionNotFound = 1009,
        [Description("Không tìm thấy thông tin chuyên mục")]
        UpdateAccountZoneNotFound = 1010,
        [Description("Tài khoản đang bị khóa")]
        UpdateAccountUserHasBeenLocked = 1011,
        [Description("Mật khẩu cũ và mật khẩu mới không giống nhau")]
        UpdateAccountOldAndNewPasswordNotMatch = 1012,
        [Description("Trạng thái không hợp lệ")]
        UpdateAccountInvalidStatus = 1013,
        [Description("Bạn không có quyền sửa thông tin account này")]
        UpdateAccountCantNotEditSystem = 1014,
        #endregion

        #region ValidAccount
        [Description("Không tìm thấy tài khoản")]
        ValidAccountInvalidUsername = 2001,
        [Description("Mật khẩu không hợp lệ")]
        ValidAccountInvalidPassword = 2002,
        [Description("Tài khoản đang bị khóa")]
        ValidAccountUserLocked = 2003,
        [Description("Mã sms không hợp lệ")]
        ValidAccountInvalidSmsCode = 2004,
        [Description("Không có quyền xử lý")]
        ValidAccountNotHavePermission = 2005,
        #endregion

        #region SwitchCurrentRole
        [Description("Tài khoản không hợp lệ")]
        SwitchCurrentRoleUserNotFound = 02006,
        [Description("Vai trò không hợp lệ")]
        SwitchCurrentRoleInvalidRole = 02007,
        [Description("Tài khoản không có vai trò cần chuyển")]
        SwitchCurrentRoleUserNotHaveRole = 02008,
        #endregion

        #region Permission
        [Description("Lỗi chưa đăng nhập")]
        NotLogin = 101,
        [Description("Không được quyền truy cập")]
        AccessDenined = 102,
        [Description("Bạn không được quyền tạo [Chủ đề]")]
        NotAllowPostTopic = 103,
        [Description("Bạn không được quyền tạo [Bài viết]")]
        NotAllowPostThread = 104,
        [Description("Bạn không được quyền gửi bình luận")]
        NotAllowPostComment = 105,
        [Description("Bạn bị giới hạn quyền cảm ơn do chưa đạt cấp độ cao hơn")]
        NotEnoughQuotaToThanks = 106,
        [Description("Bạn bị giới hạn quyền tạo chủ đề do chưa đạt cấp độ cao hơn")]
        NotEnoughQuotaToPostTopic = 107,
        #endregion

        #region UserLogin
        [Description("Tên đăng nhập không hợp lệ")]
        AccountLoginInvalidUserName = 400,
        [Description("Mật khẩu không hợp lệ")]
        AccountLoginInvalidPassword = 401,
        [Description("Tên đăng nhập hoặc mật khẩu không chính xác")]
        AccountLoginWrongUserNameOrPassword = 402,
        [Description("Tài khoản của bạn đã bị khóa")]
        AccountLoginUserBanned = 403,
        [Description("Tài khoản của bạn đã bị xóa")]
        AccountLoginUserRemoved = 404,
        [Description("Mật khẩu mới không khớp")]
        AccountPasswordNotMatch = 405,
        [Description("Tài khoản chưa được kích hoạt")]
        AccountUnActive = 406,
        #endregion

        #region News
        [Description("Tiêu đề không được để trống")]
        NewsTitleEmpty = 600,
        [Description("Mô tả không được để trống")]
        NewsSapoEmpty = 601,
        [Description("Nội dung không được để trống")]
        NewsContentEmpty = 602,
        [Description("Người đăng không được để trống")]
        NewsCreatedByEmpty = 603,
        [Description("Bài viết không tồn tại")]
        NewsNotFound = 604,
        [Description("Url không đúng định dạng")]
        NewsInValidUrl = 605,
        [Description("Tiêu đề có từ quá dài")]
        NewsInValidTitle = 606,
        [Description("Trạng thái hiện tại không cho phép bạn gửi lên")]
        NewsSendFail = 607,
        [Description("Gửi tin thành công")]
        NewsSendSuccess = 608,
        [Description("Trạng thái hiện tại không cho phép bạn nhận tin hoặc tin đã được nhận bởi người khác")]
        NewsReciveFail = 609,
        [Description("Nhận tin thành công")]
        NewsReciveSuccess = 610,
        #endregion

        #region Account

        [Description("Tài khoản người dùng không tồn tại")]
        AccountNotExists = 700,

        [Description("Tên đăng nhập không hợp lệ")]
        UserInfoInvalidUserName = 701,

        [Description("Email không hợp lệ")]
        UserInfoInvalidEmail = 702,

        [Description("Tên đăng nhập đã tồn tại")]
        UserNameExisted = 703,
        [Description("Không gửi được mail")]
        SendMailError = 701,

        #endregion

        #region FAQ
        [Description("Chủ đề không tồn tại")]
        FaqTopicNotExists = 1100,
        [Description("Tiêu đề không đươc để trống")]
        FaqTitleEmpty = 1101,
        [Description("Câu hỏi không được để trống")]
        FaqQuestionEmpty = 1102,
        [Description("Câu hỏi không tồn tại")]
        FaqQuestionNotExist = 1103,
        [Description("Bình luận không tồn tại")]
        FaqParrentNotExist = 1104,
        [Description("Câu trả lời không được để trống")]
        FaqAnswerEmpty = 1105,
        [Description("Tên người hỏi không được để trống")]
        FaqFullNameEmpty = 1106,
        #endregion

        #region Register
        [Description("Email của bạn đã được đăng ký rồi.")]
        ExistedEmail = 800,

        [Description("Email bạn đăng ký không đúng định dạng.")]
        InvalidSubcribeEmail = 807,

        [Description("Username đã tồn tại")]
        ExistedAccount = 806,

        [Description("Số điện thoại đã tồn tại")]
        ExistedMobile = 801,

        [Description("Mã xác nhận không đúng")]
        WrongCapcha = 803,

        [Description("Các thông tin cần điền đầy đủ")]
        WrongOrNullInput = 804,

        [Description("Mail kích hoạt tài khoản lỗi")]
        ErrorActiveMail = 805,
        #endregion

        #region Ahamove
        [Description("Lỗi không call đc Api Ahamove")]
        CallAhamoveNotSuccess = 900,
        #endregion
    }
}
