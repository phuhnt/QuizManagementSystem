using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QuizManagementSystem.Areas.admin.Models
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage ="Tên tài khoản không được để trống!")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        public string passWord { get; set; }
    }
}