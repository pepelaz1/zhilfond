using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using UI.Classes;
using DAL.Models;


namespace UI.Filters
{
    public class TokenAuthorize : AuthorizationFilterAttribute
    {
        private string[] _userRoles;

        public TokenAuthorize(params String[] userRoles)
        {
            _userRoles = new String[userRoles.Length];
            userRoles.CopyTo(_userRoles, 0);
        }

   
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            object o = HttpContext.Current.Session["token"];
            if ( o == null )
            {
                //HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
                //response.Headers.Location = new Uri("/Home/Index");
                //actionContext.Response = response;
                //return;
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (!TokenValidator.Validate(o.ToString(), _userRoles))
                 throw new HttpResponseException(HttpStatusCode.Unauthorized);
         

          /**  string[] roles = Roles.Split(',');


            if(httpContext.Session["ID"] == null)
                return false;


            foreach (string role in roles)
            {
                if (httpContext.Session["ROLE"].ToString().ToUpper().Equals(role))
                    return true;
            */

            base.OnAuthorization(actionContext);
        }
    }
}