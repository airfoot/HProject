using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HProject.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "部门")]
        public string Department { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "联系电话")]
        public string ContactPhone { get; set; }
        [Required]
        [Display(Name = "用户全名")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "登陆账号")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "登陆密码")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }

        public string Id { get; set; }
    }
}