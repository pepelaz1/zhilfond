using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Models;

namespace UI.Classes
{
    public static class TokenValidator
    {
        public static Boolean Validate(String token, String []roles = null)
        {
          /*  MemCacheProvider cache = new MemCacheProvider();
            User user = cache.GetUser(token);
            if (user == null)
                return false;

            if (roles.Length != 0)
            {
                if (user.Id_Role == 2 && roles.Contains("administrator"))
                    return true;
                else if ( user.Id_Role == 1 && roles.Contains("user"))
                    return true;

                return false;
            }*/
            return true;
        }
    }
}