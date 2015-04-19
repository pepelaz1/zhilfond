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
using System.Xml.Linq;
using DAL.Classes;
using System.Dynamic;


namespace DAL.Repositories
{
    public class ReportParamsRepository : IReportParamsRepository
    {
        public IEnumerable<ReportParamV> GetAllV(int id_group)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    var result = new List<ReportParamV>();
                    foreach( var rp in (from f in ctx.Forms
                                        join fl in ctx.Fields on f.Id equals fl.Id_form
                                        join fc in ctx.FieldCategories on fl.Id equals fc.Id_field
                                        join c in ctx.Categories on fc.Id_category equals c.Id
                                        join r in ctx.ReportParams on
                                            new { Id_group = id_group, Id_form = f.Id, Id_field = fl.Id }
                                            equals
                                            new { Id_group = r.Id_group, Id_form = r.Id_form, Id_field = r.Id_field } into outer
                                        from r0 in outer.DefaultIfEmpty()
                                        orderby f.Order,c.Order, fl.Order
                                        select new ReportParamV{Id = r0.Id, Id_form = f.Id, Form = f.Title, Id_field = fl.Id, Field = fl.Title, Id_category = fc.Id, Category = c.Title, Chosen = r0.Chosen}))
                    {
                        result.Add(rp);
                    }
                    return result;                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ReportParam> GetAll(int id_group)
        {
            try
            {

               /* using (var ctx = new UserContext())
                {
                    var id_form = (from f in ctx.Forms
                                   where f.Title == form
                                   select f.Id).FirstOrDefault();
                 
                    return Get(id_form,id_house,id_parent);
                }*/
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Put(ReportParam rp)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    var col = (from x in ctx.ReportParams
                               where x.Id_group == rp.Id_group && x.Id_form == rp.Id_form && x.Id_field == rp.Id_field && x.Id_category == rp.Id_category
                               select x);
                    if (col.Count() > 0)
                    {
                        ReportParam rp1 = col.First();
                        rp1.Chosen = rp.Chosen;
                    }
                    else
                    {
                        ctx.ReportParams.Add(rp);
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*public IEnumerable<ZFieldV> GetDetails(int id_form)
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
                                       select new ZFieldV { Id = x.Id, Title = x.Title, Typename = d.Title, Category = c.Title, Order = x.Order, Required = x.Required }))
                    {
                        result.Add(r);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<dynamic> GetDisplayNames(string form, int id_house)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    List<ExpandoObject> result = new List<ExpandoObject>();
                    foreach (var r in (from x in ctx.Values
                                       join f in ctx.Fields on x.Id_field equals f.Id
                                       join fr in ctx.Forms on f.Id_form equals fr.Id
                                       where f.Title == "Выводимое имя" && fr.Title == form && x.Id_house == id_house
                                       select new { Id = x.Id, Value = x.Value }))
                    {
                        dynamic o = new ExpandoObject();
                        o.Id = r.Id;
                        o.Value = r.Value;
                        
                        result.Add(o);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    var f = new ZField { Id = id };
                    ctx.Fields.Attach(f);
                    ctx.Fields.Remove(f);

                    foreach (var fc in from x in ctx.FieldCategories
                                       where x.Id_field == f.Id
                                       select x)
                    {
                        ctx.FieldCategories.Remove(fc);
                    }
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        #region Processing

        public void Process(string xml, string signature, string cert)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    using (SignatureValidator sv = new SignatureValidator()
                    {
                        Text = xml,
                        CryptedSignature = signature,
                        Cert = cert
                    })
                    {
                        if (!sv.Validate())
                            throw new Exception("SIGNATURE NOT VALID!!!!!");
                    }


                    // If ok - process data
                    XDocument xdoc = XDocument.Parse(xml);
                    foreach (var e in xdoc.Root.Elements())
                        ProcessElement(e, ctx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessElement(XElement el, UserContext ctx)
        {
            string nasp_ = (el.Element("House").Attribute("nasp").Value == "" ? "" : el.Element("House").Attribute("nasp").Value);
            string district_ = (el.Element("House").Attribute("district").Value == "" ? "" : el.Element("House").Attribute("district").Value);
            string street_ = (el.Element("House").Attribute("street").Value == "" ? "" : el.Element("House").Attribute("street").Value);
            int number_ = (el.Element("House").Attribute("number").Value == "" ? -1 : int.Parse(el.Element("House").Attribute("number").Value));
            string letter_ = (el.Element("House").Attribute("letter").Value == "" ? "" : el.Element("House").Attribute("letter").Value);

            string operation = el.Element("Form").Attribute("operation").Value;
            string parent = el.Element("Form").Attribute("parent") == null ? "" : el.Element("Form").Attribute("parent").Value;

            // Get form id
            string form = el.Element("Form").Value;
            int id_form = (from f in ctx.Forms
                           where f.Title == form
                           select f.Id).First();

            int id_house = -1;
            // Get house id
            var col = from h in ctx.HousesV
                      where h.nasp == nasp_ && (h.raion ?? "") == (district_ ?? "") && (h.street ?? "") == (street_ ?? "") && h.number == number_ && (h.letter ?? "") == (letter_ ?? "")
                      select h.id;

            if (col.Count() == 0)
            {
                // add empty house
                House h = new House()
                {
                    id_location = 0,
                    id_raion = 0,
                    id_street = 0,
                    number = 0,
                    letter = ""
                };
                h = ctx.Houses.Add(h);
                ctx.SaveChanges();
                id_house = h.Id;
            }
            else
            {
                id_house = col.First();
            }

            // Сделаем так чтобы "Выводимое имя" обрабатывалось в первую очередь
            int id_parent = -1;
            foreach (var fld in (from f in el.Elements("Field")
                                 where f.Attribute("name").Value == "Выводимое имя"
                                 select f))
            {
                if (operation.ToUpper() == "INSERT")
                    Insert(id_house, id_form, form, fld, ctx, ref id_parent);
                else
                    Update(id_house, id_form, form, fld, ctx, parent);
            }

            // Потом все остальные
            foreach (var fld in (from f in el.Elements("Field")
                                 where f.Attribute("name").Value != "Выводимое имя"
                                 select f))
            {
                if (operation.ToUpper() == "INSERT")
                    Insert(id_house, id_form, form, fld, ctx, ref id_parent);
                else
                    Update(id_house, id_form, form, fld, ctx, parent);
            }
        }


        private void Insert(int id_house, int id_form, string form, XElement el, UserContext ctx,  ref int id_parent)
        {
            // Get field id
            string field = el.Attribute("name").Value;
            int id_field = (from f in ctx.Fields
                            where f.Title == field && f.Id_form == id_form
                            select f.Id).First();

            string val = el.Value;
            // check if current value is value from dict and get it from this dict
            string type = el.Attribute("type").Value;
            int id_dict = (from f in ctx.Dicts
                           where f.Title == type
                           select f.Id).First();

            if (id_dict < 1000)
            {
                var col2 = (from dv in ctx.DictsValues
                            where dv.Id_dict == id_dict && dv.Value == val
                            select dv.Id);
                if (col2.Count() == 0)
                    val = "";
                else
                    val = col2.First().ToString();
            }
            
            ZValue v = ctx.Values.Add(new ZValue()
            {
                Id_house = id_house,
                Id_form = id_form,
                Id_field = id_field,
                Value = val,
                Id_parent = id_parent
            });
            ctx.SaveChanges();

            if (id_parent == -1)
            {
                v.Id_parent = v.Id;
                id_parent = v.Id;
            }

            // update House table if needed
            if (form == "Основные данные")
                UpdateHouse(id_house, id_dict, field, val, ctx);

            ctx.SaveChanges();
        }


        private void Update(int id_house, int id_form, string form, XElement el, UserContext ctx, string parent)
        {
            // Get field id
            string field = el.Attribute("name").Value;
            int id_field = (from f in ctx.Fields
                            where f.Title == field && f.Id_form == id_form
                            select f.Id).First();

            string val = el.Value;
            // check if current value is value from dict and get it from this dict
            string type = el.Attribute("type").Value;
            int id_dict = (from f in ctx.Dicts
                           where f.Title == type
                           select f.Id).First();

            if (id_dict < 1000)
            {
                var col2 = (from dv in ctx.DictsValues
                            where dv.Id_dict == id_dict && dv.Value == val
                            select dv.Id);
                if (col2.Count() == 0)
                    val = "";
                else
                    val = col2.First().ToString();
            }

            var id_parent = 0;
            var col3 = (from v in ctx.Values
                        join f in ctx.Fields on v.Id_field equals f.Id
                        where f.Id_form == id_form && f.Title == "Выводимое имя" && v.Id_house == id_house && v.Value == parent
                        select v.Id);
            if (col3.Count() > 0)
                id_parent = col3.First();

       
            var col = from v in ctx.Values
                      where v.Id_house == id_house && v.Id_form == id_form && v.Id_field == id_field
                      select v;
            if (col.Count() > 0)
            {
                ZValue v = col.First();
                v.Value = val;
                v.Id_parent = id_parent;
            }
            else
            {
                ZValue v = ctx.Values.Add(new ZValue()
                {
                    Id_house = id_house,
                    Id_form = id_form,
                    Id_field = id_field,
                    Value = val,
                    Id_parent = id_parent
                });
            }

            // update House table if needed
            if (form == "Основные данные")
                UpdateHouse(id_house, id_dict, field, val, ctx);

            ctx.SaveChanges();
        }


        private void UpdateHouse(int id_house, int id_dict, string field, string val, UserContext ctx)
        {
            var house = (from h in ctx.Houses
                         where h.Id == id_house
                         select h).FirstOrDefault();
            int n = 0;
            int.TryParse(val, out n);
            if (field == "Населенный пункт")
                house.id_location = n;
            else if (field == "Район")
                house.id_raion = n;
            else if (field == "Улица")
                house.id_street = n;
            else if (field == "Номер дома")
                house.number = n;
            else if (field == "Литера дома")
                house.letter = val;
            ctx.SaveChanges();

        }
        #endregion*/
    }
}
