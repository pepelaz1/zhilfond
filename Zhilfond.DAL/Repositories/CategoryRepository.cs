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
    public class CategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> GetAll()
        {
            using (var ctx = new UserContext())
            {
                List<Category> result = new List<Category>();
                foreach (var r in (from x in ctx.Categories
                                   orderby x.Title
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }      
    }
}
