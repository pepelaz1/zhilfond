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
    public class FormsRepository : IFormsRepository
    {
        public IEnumerable<ZForm> GetAll(String sfld, String sord, String srfield, String srop, String srvalue)
        {
            using (var ctx = new UserContext())
            {
                List<ZForm> result = new List<ZForm>();
                foreach (var r in (from f in ctx.Forms
                                   where f.Title != "Дома"
                                   orderby f.Order
                                   select f))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<ZForm> GetAll(UserV u)
        {
            using (var ctx = new UserContext())
            {
                List<ZForm> result = new List<ZForm>();
                if (u.Role == "Администраторы")
                {

                    foreach (var r in (from x in ctx.Forms
                                       orderby x.Order
                                       select x))
                    {
                        result.Add(r);
                    }
                }
                else
                {
                    foreach (var r in (from x in ctx.Forms
                                       where (from y in ctx.RolesForms
                                              where y.Id_Role == u.Id_Role
                                              select y.Id_Form).Contains(x.Id)
                                       orderby x.Order
                                       select x))
                    {
                        result.Add(r);
                    }
                }
                return result;
            }
        }

        public void Add(ZForm item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Forms.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }       
        

        public void Update(int id, ZForm item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ZForm f = (from x in ctx.Forms
                               where x.Id == id
                               select x).First();
                    f.Title = item.Title;
                    f.Order = item.Order;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                     var f = new ZForm { Id = id };
                     ctx.Forms.Attach(f);
                     ctx.Forms.Remove(f);
                     ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
