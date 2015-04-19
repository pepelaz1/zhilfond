using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;
using DAL.Models;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;


namespace DAL.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        #region GET

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Role> result = new List<Role>();
                foreach (var r in (from x in ctx.Roles
                                   where x.Id <= 1000
                                   orderby x.Title
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<Role> GetAllDesc()
        {
            using (var ctx = new UserContext())
            {
                List<Role> result = new List<Role>();
                foreach (var r in (from x in ctx.Roles
                                   orderby x.Id descending
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public Role Get(int id)
        {
            //return new Role() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                return ctx.Roles.Single(p => p.Id == id);
            }
        }

        #endregion GET

        #region POST

        public Role Add(Role item)
        {
            //return new Role() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Roles.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return item;
        }

        #endregion POST

        #region PUT

        public bool Update(int id, Role item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        Role f = (from x in ctx.Roles
                                  where x.Id == id
                                  select x).First();
                        f.Title = item.Title;
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion PUT

        public bool Delete(int id)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        var f = new Role { Id = id };
                        ctx.Roles.Attach(f);
                        ctx.Roles.Remove(f);
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
