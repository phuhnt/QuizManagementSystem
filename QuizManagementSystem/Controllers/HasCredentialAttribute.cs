using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizManagementSystem.Areas.admin.Controllers;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Controllers
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = HttpContext.Current.Session[ConstantVariable.USER_SESSION] as UserLogin;
            if (session == null)
            {
                return false;
            }
            List<string> privilegelLevels = GetCredentialByLoggedInUser(session.UserName);
            if (privilegelLevels.Contains(this.RoleID) || session.GroupID == ConstantVariable.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[ConstantVariable.SESSION_CREDENTIAL];
            return credentials;
        }

    }
}