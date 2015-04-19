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
using DAL.Classes;


namespace DAL.Repositories
{
    public class GroupsRepository : IGroupsRepository
    {
        public IEnumerable<Group> Get(int id_user)
        {
            using (var ctx = new UserContext())
            {
                List<Group> result = new List<Group>();
                foreach (var r in (from x in ctx.Groups
                                   where x.Id_user == id_user
                                   orderby x.Id
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

       
        public void Add(Group item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Groups.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }       
        

        public void Update(int id, Group item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Group g = (from x in ctx.Groups
                               where x.Id == id
                               select x).First();
                    g.Title = item.Title;
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
                     var f = new Group { Id = id };
                     ctx.Groups.Attach(f);
                     ctx.Groups.Remove(f);
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
