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
            return View("Index");
        }

        //401: Không có quyền truy cập
        public ViewResult NotAccessPermission()
        {
            Response.StatusCode = 401;
            return View("NotAccessPermission");
        }


        //403: Truy cập bị từ chối
        public ViewResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View("AccessDenied");
        }

        //404 : Không tìm thấy trang yêu cầu
        public ViewResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("PageNotFound");
        }

        //500 : Lỗi máy chủ
        public ViewResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }
    }
}