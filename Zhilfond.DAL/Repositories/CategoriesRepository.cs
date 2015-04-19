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
    public class CategoriesRepository : ICategoriesRepository
    {
        public IEnumerable<Category> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Category> result = new List<Category>();
                foreach (var r in (from x in ctx.Categories
                                   orderby x.Order
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public void Add(Category item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Categories.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public void Update(int id, Category item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Category f = (from x in ctx.Categories
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
                    var f = new Category { Id = id };
                    ctx.Categories.Attach(f);
                    ctx.Categories.Remove(f);
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
