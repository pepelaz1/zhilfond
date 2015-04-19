using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class GroupRepTemplatesRepository : IGroupRepTemplatesRepository
    {
        #region GET

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GroupRepTemplate> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<GroupRepTemplate> result = new List<GroupRepTemplate>();
                foreach (var r in (from x in ctx.GroupRepTemplates
                                   orderby x.Id
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public GroupRepTemplate Get(int id)
        {
            //return new Dict() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                return ctx.GroupRepTemplates.Find(id);
            }
        }

        #endregion GET

        #region POST

        public GroupRepTemplate Add(GroupRepTemplate item)
        {
            //return new Dict() { Id=0, Title="Нет"};

            using (var ctx = new UserContext())
            {
                try
                {
                    item.Id = ctx.GroupRepTemplates.Count() > 0 ? ctx.GroupRepTemplates.Max(grt => grt.Id) + 1 : 1;
                    ctx.GroupRepTemplates.Add(item);
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

        public bool Update(int id, GroupRepTemplate item)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    try
                    {
                        GroupRepTemplate f = ctx.GroupRepTemplates.Find(item.Id);
                        f.template_name = item.template_name;
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
                        var f = ctx.GroupRepTemplates.Find(id);
                        ctx.GroupRepTemplates.Remove(f);
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
