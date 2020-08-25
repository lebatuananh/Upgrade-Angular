using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Desciption { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public int UserType { get; set; }
    }
}
