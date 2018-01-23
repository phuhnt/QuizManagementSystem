using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizManagementSystem.Common;

namespace QuizManagementSystem.Controllers
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var isAuthorezed = base.AuthorizeCore(httpContext);
            //if (!isAuthorezed)
            //{
            //    return false;
            //}
            var session = HttpContext.Current.Session[ConstantVariable.USER_SESSION] as UserLogin;
            List<string> privilegelLevels = GetCredentialByLoggedInUser(session.UserName);
            if (session == null)
            {
                return false;
            }
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