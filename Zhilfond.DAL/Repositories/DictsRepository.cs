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
    public class DictsRepository : IDictsRepository
    {

        #region GET

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dict> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Dict> result = new List<Dict>();
                foreach (var r in (from x in ctx.Dicts
                                   where x.Id <= 1000
                                   //orderby x.Title
                                   orderby x.Id
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public IEnumerable<Dict> GetAllDesc()
        {
            using (var ctx = new UserContext())
            {
                List<Dict> result = new List<Dict>();
                foreach (var r in (from x in ctx.Dicts
                                   orderby x.Id descending
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public Dict Get(int id)
        {
            //return new Dict() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                return ctx.Dicts.Single(p => p.Id == id);
            }
        }

        #endregion GET

        #region POST

        public Dict Add(Dict item)
        {
            //return new Dict() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Dicts.Add(item);
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

        public bool Update(int id, Dict item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        Dict f = (from x in ctx.Dicts
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
                        var f = new Dict { Id = id };
                        ctx.Dicts.Attach(f);
                        ctx.Dicts.Remove(f);
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
