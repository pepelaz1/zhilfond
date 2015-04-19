using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Security.Cryptography;


namespace DAL.Repositories
{
    public class PrivateKeysRepository : IPrivateKeysRepository
    {
        public PrivateKey Add(PrivateKey item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.PrivateKeys.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return item;
        }

        public PrivateKey Get()
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    return (from pk in ctx.PrivateKeys
                            join u in ctx.Users on pk.Id_user equals u.Id
                            where u.Login == "admin"
                            orderby u.Id 
                            select pk).ToList().Last();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
