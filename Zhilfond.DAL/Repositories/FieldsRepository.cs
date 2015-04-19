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
    public class FieldsRepository : IFieldsRepository
    {
        public IEnumerable<ZFieldV> Get(int id_form)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    List<ZFieldV> result = new List<ZFieldV>();
                    foreach (var r in (from x in ctx.Fields
                                       join d in ctx.Dicts on x.Id_dict equals d.Id
                                       join fc in ctx.FieldCategories on x.Id equals fc.Id_field into outer1
                                       from fc0 in outer1.DefaultIfEmpty()
                                       join c in ctx.Categories on fc0.Id_category equals c.Id into outer2
                                       from c0 in outer2.DefaultIfEmpty()
                                       where x.Id_form == id_form
                                       orderby x.Order
                                       select new ZFieldV { Id = x.Id, Title = x.Title, Typename = d.Title, Category = c0.Title, Order = x.Order, Required = x.Required, Personal = x.Personal }))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<ZFieldV>();
            }
        }

        public IEnumerable<ZFieldV> GetDetails(int id_form)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    List<ZFieldV> result = new List<ZFieldV>();
                    foreach (var r in (from x in ctx.Fields
                                       join d in ctx.Dicts on x.Id_dict equals d.Id
                                       join fc in ctx.FieldCategories on x.Id equals fc.Id_field
                                       join c in ctx.Categories on fc.Id_category equals c.Id
                                       where x.Id_form == id_form
                                       orderby c.Order, x.Order
                                       select new ZFieldV { Id = x.Id, Title = x.Title, Typename = d.Title, Category = c.Title, Order = x.Order, Required = x.Required, Personal = x.Personal }))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new List<ZFieldV>();
            }
        }

        public void Add(ZField item, int id_category)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ZField f = ctx.Fields.Add(item);
                    ctx.SaveChanges();

                    ctx.FieldCategories.Add(new FieldCategory()
                    {
                        Id_category = id_category,
                        Id_field = f.Id
                    });
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Update(int id, ZField item, int id_category)
        {
            using (var ctx = new UserContext())
            {
                try
                {
                    ZField f = (from x in ctx.Fields
                               where x.Id == id
                               select x).First();
                    f.Title = item.Title;
                    f.Id_dict = item.Id_dict;
                    f.Order = item.Order;
                    f.Required = item.Required;
                    f.Personal = item.Personal;


                    var fc = from x in ctx.FieldCategories
                             where x.Id_field == id
                             select x;
                    if (fc.Count() == 0)
                    {
                        ctx.FieldCategories.Add(new FieldCategory()
                        {
                            Id_category = id_category,
                            Id_field = id
                        });
                    }
                    else
                    {
                        fc.FirstOrDefault().Id_category = id_category;
                    }
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
                    var f = ctx.Fields.Find(id);//new ZField { Id = id };
                    //ctx.Fields.Attach(f);
                    ctx.Fields.Remove(f);

                    /*foreach (var fc in from x in ctx.FieldCategories
                                       where x.Id_field == f.Id
                                       select x)
                    {
                        ctx.FieldCategories.Remove(fc);
                    }*/
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
