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
            var session = (UserLogin)HttpContext.Current.Session[ConstantVariable.USER_SESSION];
            string privilegelLevels = string.Join(";", this.GetCredentialByLoggedInUser(session.UserName));

            if (privilegelLevels.Contains(this.RoleID))
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