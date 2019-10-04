using Microsoft.AspNetCore.Mvc;
using StudentManagement.CustomerUtil;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action:"IsEmailInUse",controller:"Account")]
        [Display(Name = "邮箱地址")]
        [ValidEmailDomain(allowedDomain:"qq.com",ErrorMessage ="邮箱的地址必须是QQ邮箱 ")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密码与确认密码不一致，请重新输入")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="城市")]
        public string City  { get; set; }
    }
}