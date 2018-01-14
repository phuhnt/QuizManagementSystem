using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizManagementSystem.Controllers
{
    public class errorController : Controller
    {
        // GET: error
        public ViewResult Index()
        {
            return View();
        }

        //403: Truy cập bị từ chối
        public ViewResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        //404 : Không tìm thấy trang yêu cầu
        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        //500 : Lỗi máy chủ
        public ViewResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}