using DVG.WIS.Utilities.SignalrHelper;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DVG.WIS.PublicModel
{
    [Serializable]
    public class UserLogin
    {
        public UserLogin() { }

        public UserLogin(Entities.Account userInfo)
        {
            if (userInfo != null)
            {
                this.UserId = userInfo.Id;
                this.FullName = userInfo.FullName;
                this.DisplayName = userInfo.FullName;
                this.Mobile = userInfo.Phone;
                this.Avatar = userInfo.Avatar;
                this.Email = userInfo.Email;
                this.UserName = userInfo.UserName;
                this.UserType = userInfo.UserType;
            }

        }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Signature { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public Nullable<long> UserType { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }
        public string ConnectionId { get; set; }
        public string Url { get; set; }
        public string EncryptId
        {
            get { return UserId > 0 ? EncryptUtils.Encrypt(this.UserId.ToString()) : string.Empty; }
        }
    }

    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
