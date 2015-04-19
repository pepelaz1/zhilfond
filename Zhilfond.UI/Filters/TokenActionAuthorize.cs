using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Classes;

namespace UI.Filters
{
    public class TokenActionAuthorize : AuthorizeAttribute
    {
        private string[] _userRoles;

        public TokenActionAuthorize(params String[] userRoles)
        {
            _userRoles = new String[userRoles.Length];
            userRoles.CopyTo(_userRoles, 0);
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //   filterContext.Controller.ViewBag.AutherizationMessage = "Custom Authorization: Message from OnAuthorization method.";
            object o = HttpContext.Current.Session["token"];
            if (o == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;

            }

            if (!TokenValidator.Validate(o.ToString(), _userRoles))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
          //  base.OnAuthorization(filterContext);
        }   
    }
}