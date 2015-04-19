using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
using UI.Classes;
using DAL.Models;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        private static Boolean IsAuthorired()
        {
            object o = HttpContext.Current.Session["token"];
            if (o == null)
                return false;

            return true;
        }

        /*  private static Boolean IsAdmin()
          {
              object o = HttpContext.Current.Session["token"];
              if (o == null)
                  return false;

              User user = Utils.GetCurrentUser();
              return user.Id_Role == 1;
          }*/


        public static MvcHtmlString AuthUserActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controller)
        {
            if (IsAuthorired())
                return htmlHelper.ActionLink(linkText, actionName, controller);
            else
                return MvcHtmlString.Empty;
        }

        public static MvcHtmlString AdminActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controller)
        {
            //  if (IsAdmin())
            if (IsAuthorired())
                return htmlHelper.ActionLink(linkText, actionName, controller);
            else
                return MvcHtmlString.Empty;
        }
    }
}
