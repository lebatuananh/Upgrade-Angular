using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using static DVG.WIS.Utilities.EnumHelper;

namespace DVG.WIS.PublicModel.CMS
{
    public class UsersModel
    {
        public UsersModel()
        {

        }
        public UsersModel(Entities.Account users)
        {
            this.UserId = users.Id;
            this.UserName = users.UserName;
            this.Password = users.Password;
            this.Email = users.Email;
            this.Mobile = users.Phone;
            this.FullName = users.FullName;
            this.Avatar = users.Avatar;
            this.Status = users.Status;
            this.CreatedDate = users.CreatedDate;
            this.Gender = users.Gender;
            this.Address = users.Address;
            this.Birthday = users.Birthday;
            this.PasswordQuestion = users.PasswordQuestion;
            this.PasswordAnswer = users.PasswordAnswer;
            this.Desciption = users.Desciption;
            this.LastLogin = users.LastLogin;
            this.LastPasswordChange = users.LastPasswordChange;
            this.LastUpdate = users.LastModifiedDate;
            this.UserType = users.UserType;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }

        public string StatusBack
        {
            get
            {
                switch (Status)
                {
                    case (int)UserStatusAdmin.Deactived:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Deactived);
                    case (int)UserStatusAdmin.Actived:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Actived);
                    case (int)UserStatusAdmin.Deleted:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Deleted); ;
                    default:
                        return string.Empty;
                }
            }
        }

        public Nullable<DateTime> CreatedDate { get; set; }

        public string CreatedDateStr
        {
            get
            {
                if (CreatedDate.HasValue)
                {
                    return CreatedDate.Value.ToString("dd/MM/yyyy");
                }
                return string.Empty;
            }
        }

        public bool Gender { get; set; }

        public string GenderStr
        {
            get
            {
                if (Gender)
                {
                    return Const.Male;
                }
                return Const.Female;
            }
        }

        public string Address { get; set; }
        public Nullable<byte> CityId { get; set; }
        public DateTime Birthday { get; set; }

        public string BirthdayStr
        {
            get
            {
                if (Birthday != null && Birthday != default(DateTime))
                {
                    return Birthday.ToString("dd/MM/yyyy");
                }
                return string.Empty;
            }
            set
            {
                Birthday = !String.IsNullOrEmpty(value) ? Utils.ConvertStringToDateTime(value, "dd/MM/yyyy") : default(DateTime);
            }
        }

        public int TotalRows { get; set; }


        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public string Desciption { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }
        public Nullable<DateTime> LastPasswordChange { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public Nullable<int> UserType { get; set; }
        public List<Enums> UsersTypeList { get; set; }
    }


    public class UsersSearchModel
    {
        public string Keyword { get; set; }
        public int UserType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public UsersSearchModel()
        {
            this.PageIndex = 1;
            this.PageSize = 15;
        }
    }
}
