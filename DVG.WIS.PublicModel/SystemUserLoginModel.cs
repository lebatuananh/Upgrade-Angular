using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DVG.WIS.PublicModel
{
    public class SystemUserLoginModel
    {
        [Display(Name = "Google Code")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string GGCode { get; set; }
        public string Languague { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Require input!")]
        [UIHint("stringPassword")]
        public string Password { get; set; }
        public string Provider { get; set; }
        [DefaultValue(true)]
        [Display(Name = "Remember account")]
        public bool Remember { get; set; }
        public string ReturnUrl { get; set; }
        [Required(ErrorMessage = "Require input!")]
        public string UserName { get; set; }
    }
}
