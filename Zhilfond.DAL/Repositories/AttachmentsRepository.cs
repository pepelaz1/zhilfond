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
    public class AttachmentsRepository : IAttachmentsRepository
    {
        public IEnumerable<Attachment> Get(int id_house)
        {
            using (var ctx = new UserContext())
            {
                List<Attachment> result = new List<Attachment>();
                foreach (var r in (from x in ctx.Attachments
                                   where x.Id_house == id_house
                                       && x.Archive == false
                                   orderby x.Order
                                   select x))
                {
                    result.Add(r);
                }
                return result;
            }
        }

        public Attachment GetById(int id)
        {
            using (var ctx = new UserContext())
            {
                try
                {

                    return (from x in ctx.Attachments
                            where x.Id == id
                            select x).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Add(Attachment item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ctx.Attachments.Add(item);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(int id, Attachment item)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    Attachment f = (from x in ctx.Attachments
                               where x.Id == id
                               select x).First();
                    f.Filename = item.Filename;
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
                    Attachment f = (from x in ctx.Attachments
                                    where x.Id == id
                                    select x).First();
                    f.Archive = true;
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
