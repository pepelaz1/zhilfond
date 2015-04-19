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
    public class RightsRepository : IRightsRepository
    {

        #region GET

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Right> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Right> result = new List<Right>();
                foreach (var r in (from x in ctx.Rights                                   
                                   orderby x.Title
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<Right> GetAllDesc()
        {
            using (var ctx = new UserContext())
            {
                List<Right> result = new List<Right>();
                foreach (var r in (from x in ctx.Rights
                                   orderby x.Id descending
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public Right Get(int id)
        {
            //return new Right() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                return ctx.Rights.Single(p => p.Id == id);
            }
        }

        #endregion GET

        #region POST

        public Right Add(Right item)
        {
            //return new Right() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Rights.Add(item);
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

        public bool Update(int id, Right item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        Right f = (from x in ctx.Rights
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
                        var f = new Right { Id = id };
                        ctx.Rights.Attach(f);
                        ctx.Rights.Remove(f);
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
