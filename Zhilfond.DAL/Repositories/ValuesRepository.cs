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
    public class ValuesRepository : IValuesRepository
    {
        static readonly IResultsRepository _results_repository = new ResultsRepository();
        static readonly IImpFilesRepository _imp_files_repository = new ImpFilesRepository();
        static readonly RulesValidator _rvalidator = new RulesValidator();
        static readonly FormulasEvaluator _fevaluator = new FormulasEvaluator();

        public IEnumerable<ZValueV> Get(int id_form, int id_house, int? id_parent, UserV u)
        {
            try
            {
                using (var ctx = new UserContext())
                {
                    List<ZValueV> result = new List<ZValueV>();
                    if (id_parent == 0 && id_form == 1)
                    {
                        IEnumerable<ZValueV> col;
                        if (u.Role == "Администраторы")
                        {
                            col = (from f in ctx.Fields
                                   join fr in ctx.Forms on f.Id_form equals fr.Id
                                   join d in ctx.Dicts on f.Id_dict equals d.Id
                                   join fc in ctx.FieldCategories on f.Id equals fc.Id_field into outer1
                                   from fc0 in outer1.DefaultIfEmpty()
                                   join c in ctx.Categories on fc0.Id_category equals c.Id into outer2
                                   from c0 in outer2.DefaultIfEmpty()
                                   join v in ctx.ValuesAll on
                                       new { Id = f.Id, Id_form = id_form, Id_house = id_house }
                                       equals
                                       new { Id = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house } into outer3
                                   from v0 in outer3.DefaultIfEmpty()
                                   where f.Id_form == id_form
                                   orderby c0.Order, f.Order
                                   select new ZValueV
                                   {
                                       Id = f.Id,
                                       Id_form = fr.Id,
                                       Title = f.Title,
                                       Dictname = d.Title,
                                       Id_category = c0.Id == null ? 3 : c0.Id,
                                       Category = c0.Title,
                                       Value = v0.Value == null ? "" : v0.Value,
                                       Id_dict = d.Id,
                                       Id_parent = v0.Id_parent,
                                       Warning = v0.Warning
                                   });
                        }
                        else
                        {
                            col = (from f in ctx.Fields
                                   join fr in ctx.Forms on f.Id_form equals fr.Id
                                   join d in ctx.Dicts on f.Id_dict equals d.Id
                                   join fc in ctx.FieldCategories on f.Id equals fc.Id_field into outer1
                                   from fc0 in outer1.DefaultIfEmpty()
                                   join c in ctx.Categories on fc0.Id_category equals c.Id into outer2
                                   from c0 in outer2.DefaultIfEmpty()
                                   join v in ctx.ValuesAll on
                                       new { Id = f.Id, Id_form = id_form, Id_house = id_house }
                                       equals
                                       new { Id = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house } into outer3
                                   from v0 in outer3.DefaultIfEmpty()
                                   join rf in ctx.RolesForms on
                                      new { Id_form = fr.Id, Id_cat = c0.Id, Id_Role = u.Id_Role }
                                      equals
                                      new { Id_form = rf.Id_Form, Id_cat = rf.Id_Cat, Id_Role = rf.Id_Role }
                                   where f.Id_form == id_form
                                   orderby c0.Order, f.Order
                                   select new ZValueV
                                   {
                                       Id = f.Id,
                                       Id_form = fr.Id,
                                       Title = f.Title,
                                       Dictname = d.Title,
                                       Id_category = c0.Id == null ? 3 : c0.Id,
                                       Category = c0.Title,
                                       Value = v0.Value == null ? "" : v0.Value,
                                       Id_dict = d.Id,
                                       Id_parent = v0.Id_parent,
                                       Warning = v0.Warning
                                   });
                        }
                        foreach (var r in col)
                        {
                            result.Add(r);
                        }
                    }
                    else
                    {
                        IEnumerable<ZValueV> col;
                        if (u.Role == "Администраторы")
                        {
                            col = (from f in ctx.Fields
                                   join fr in ctx.Forms on f.Id_form equals fr.Id
                                   join d in ctx.Dicts on f.Id_dict equals d.Id
                                   join fc in ctx.FieldCategories on f.Id equals fc.Id_field into outer1
                                   from fc0 in outer1.DefaultIfEmpty()
                                   join c in ctx.Categories on fc0.Id_category equals c.Id into outer2
                                   from c0 in outer2.DefaultIfEmpty()
                                   join v in ctx.ValuesAll on
                                       new { Id = f.Id, Id_form = id_form, Id_house = id_house, Id_parent = id_parent }
                                       equals
                                       new { Id = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house, Id_parent = v.Id_parent } into outer3
                                   from v0 in outer3.DefaultIfEmpty()              
                                   where f.Id_form == id_form
                                   orderby c0.Order, f.Order
                                   select new ZValueV
                                   {
                                       Id = f.Id,
                                       Title = f.Title,
                                       Dictname = d.Title,
                                       Category = c0.Title,
                                       Value = v0.Value == null ? "" : v0.Value,
                                       Id_dict = d.Id,
                                       Id_parent = v0.Id_parent,
                                       Warning = v0.Warning
                                   });
                        }
                        else
                        {
                            col = (from f in ctx.Fields
                                   join fr in ctx.Forms on f.Id_form equals fr.Id
                                   join d in ctx.Dicts on f.Id_dict equals d.Id
                                   join fc in ctx.FieldCategories on f.Id equals fc.Id_field into outer1
                                   from fc0 in outer1.DefaultIfEmpty()
                                   join c in ctx.Categories on fc0.Id_category equals c.Id into outer2
                                   from c0 in outer2.DefaultIfEmpty()
                                   join v in ctx.ValuesAll on
                                        new { Id = f.Id, Id_form = id_form, Id_house = id_house, Id_parent = id_parent }
                                        equals
                                        new { Id = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house, Id_parent = v.Id_parent } into outer3
                                   from v0 in outer3.DefaultIfEmpty()
                                   join rf in ctx.RolesForms on
                                       new { Id_form = fr.Id, Id_cat = c0.Id, Id_Role = u.Id_Role }
                                       equals
                                       new { Id_form = rf.Id_Form, Id_cat = rf.Id_Cat, Id_Role = rf.Id_Role }
                                   where f.Id_form == id_form
                                   orderby c0.Order, f.Order
                                   select new ZValueV
                                   {
                                       Id = f.Id,
                                       Title = f.Title,
                                       Dictname = d.Title,
                                       Category = c0.Title,
                                       Value = v0.Value == null ? "" : v0.Value,
                                       Id_dict = d.Id,
                                       Id_parent = v0.Id_parent,
                                       Warning = v0.Warning
                                   });
                        }
                        foreach (var r in col)
                        {
                            result.Add(r);
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ZValueV> Get(string form, int id_house, int? id_parent, UserV u)
        {
            try
            {

                using (var ctx = new UserContext())
                {
                    var id_form = (from f in ctx.Forms
                                   where f.Title == form
                                   select f.Id).FirstOrDefault();

                    // Проверка правил перед выводом
                    _rvalidator.Validate(ctx, id_house, id_form, id_parent);

                    return Get(id_form,id_house,id_parent, u);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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


        #region Processing

        public void Process(int id_user, string sourceType, string id_source, string xml, string signature, string cert)
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
                            throw new Exception("Ошибка проверки электронно-цифровой подписи");
                    }

                    // If ok - process data
                    XDocument xdoc = XDocument.Parse(xml);
                    foreach (var e in xdoc.Root.Elements())
                    {
                        Result r = new Result();
                        r.Id_user = id_user;
                        r.SourceType = sourceType;
                        if (sourceType == "import")
                            r.Id_source = int.Parse(id_source);
                        try
                        {
                            ProcessElement(id_user, e, ctx , ref r);
                            r.Status = "Ок";
                        }
                        catch (Exception ex)
                        {
                            r.Status = "Ошибка";
                            r.Error = "Неправильный формат файла.";
                        }
                        finally
                        {

                            r.Created = DateTime.Now;
                            _results_repository.Add(r);
                            _imp_files_repository.SignFile(int.Parse(id_source));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessElement(int id_user, XElement el, UserContext ctx, ref Result result)
        {
            string nasp_ = (el.Element("House").Attribute("nasp").Value == "" ? "" : el.Element("House").Attribute("nasp").Value);
            string district_ = (el.Element("House").Attribute("district").Value == "" ? "" : el.Element("House").Attribute("district").Value);
            string street_ = (el.Element("House").Attribute("street").Value == "" ? "" : el.Element("House").Attribute("street").Value);
            int number_ = (el.Element("House").Attribute("number").Value == "" ? -1 : int.Parse(el.Element("House").Attribute("number").Value));
            string letter_ = (el.Element("House").Attribute("letter").Value == "" ? "" : el.Element("House").Attribute("letter").Value);

           // string operation = el.Element("Form").Attribute("operation").Value;
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


            //// Check if we need insert or update data
            //if (form == "Основные данные")
            //{
            //    var v = from x in ctx.DValues
            //            join f in ctx.Fields on x.Id_field equals f.Id
            //            where x.Id_house == id_house && x.Id_form == id_form && f.Title == field
            //            select x;

            //}
            //else
            //{
            //}



            int? id_parent = null;
            var col3 = (from v in ctx.Values
                        join f in ctx.Fields on v.Id_field equals f.Id
                        where f.Id_form == id_form && f.Title == "Выводимое имя" && v.Id_house == id_house && v.Value == parent
                        select v.Id);
            if (col3.Count() > 0)
                id_parent = col3.First();
          

  

            // Сделаем так чтобы "Выводимое имя" обрабатывалось в первую очередь
            //int? id_parent = null;


            foreach (var fld in (from f in el.Elements("Field")
                                 where f.Attribute("name").Value == "Выводимое имя"
                                 select f))
            {
                if (parent != "")
                    result.Element = "[" + form + "].[" + parent + "]";
                else
                    result.Element = "[" + form + "]";

                result.Element += ".[Выводимое имя]";
                result.House = nasp_ + " " + district_ + " " + street_ + " " + number_.ToString() + " " + letter_;

                id_parent = ProcessField(id_user, id_house, id_form, form, fld, ctx, id_parent);
            }

            if (id_parent == null && id_form != 1)
            {
                // Дальше обрабатывать нет смысла
                return;
            }

       
            // Потом все остальные
            foreach (var fld in (from f in el.Elements("Field")
                                 where f.Attribute("name").Value != "Выводимое имя"
                                 select f))
            {
                if (parent != "")
                    result.Element = "[" + form + "].[" + parent + "]";
                else
                    result.Element = "[" + form + "]";

                result.Element += ".[" + fld.Attribute("name").Value + "]";
                result.House = nasp_ + " " + district_ + " " + street_ + " " + number_.ToString() + " " + letter_;

                //id_parent = ProcessField(id_user, id_house, id_form, form, fld, ctx, id_parent);
                ProcessField(id_user, id_house, id_form, form, fld, ctx, id_parent);
            }


            // Вычисление значений по формулам, где это нужно
             _fevaluator.Evaluate(ctx, id_house, id_form, id_parent);
            /// Проверка корректности данных
           // _rvalidator.Validate(ctx, id_house, id_form, id_parent);
        }


        private int? ProcessField(int id_user, int id_house, int id_form, string form, XElement el, UserContext ctx, int? id_parent)
        {
            // Get field id
            string field =  el.Attribute("name").Value;
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

            // Определяем тип операции - вставка-0 или обновление-1
            int op = 0;
            IEnumerable<ZValue> vals;
            if (form == "Основные данные")
            {
                //var vals = from x in ctx.DValues
                //           join f in ctx.Fields on x.Id_field equals f.Id
                //           where x.Id_house == id_house && x.Id_form == id_form && f.Title == field
                //           select new { x, f };
                //if (vals.Count() > 0)
                //    op = 1;  
                vals = from x in ctx.Values
                       where x.Id_house == id_house && x.Id_form == id_form && x.Id_field == id_field 
                       select x;
                if (vals.Count() == 0)
                    Insert(id_user, id_house, id_form, form, id_field, field, val, id_dict, ctx, ref id_parent);
                else
                    Update(id_user, id_house, id_form, form, id_field, field, val, vals.First(), id_dict, ctx, null);
            }
            else
            {
                //var vals = from x in ctx.DValues
                //           join f in ctx.Fields on x.Id_field equals f.Id
                //           where x.Id_house == id_house && x.Id_form == id_form && f.Title == field && x.Id_parent == id_parent
                //           select new { x, f };
                //if (vals.Count() > 0)
                //    op = 1;
                vals = from x in ctx.Values
                       where x.Id_house == id_house && x.Id_form == id_form && x.Id_field == id_field && x.Id_parent == id_parent
                       select x;

                if (vals.Count() == 0)
                    Insert(id_user, id_house, id_form, form, id_field, field, val, id_dict, ctx, ref id_parent);
                else
                    Update(id_user, id_house, id_form, form, id_field, field, val, vals.First(), id_dict, ctx, id_parent);
            }

   
            //if (op == 0)
            //    Insert(id_user, id_house, id_form, form, id_field, field, val, id_dict, ctx, ref id_parent);
            //else
            //    Update(id_user, id_house, id_form, form, id_field, field, val, id_dict, ctx, id_parent);

            return id_parent;
        }

        private void Insert(int id_user, int id_house, int id_form, string form, int id_field, string field, string val, int id_dict, UserContext ctx,  ref int? id_parent)
        {          
            ZValue v = ctx.Values.Add(new ZValue()
            {
                Id_house = id_house,
                Id_form = id_form,
                Id_field = id_field,
                Value = val,
                Id_parent = id_parent
            });

            try
            {
                ctx.SaveChanges();

                if (id_parent == null && id_form != 1) // Не для "Основных данных"
                {
                    v.Id_parent = v.Id;
                    id_parent = v.Id;
                }

                // update House table if needed
                if (form == "Основные данные")
                    UpdateHouse(id_house, id_dict, field, val, ctx);

                ctx.SaveChanges();

                //// Аудит
                var au = ctx.Audits.Add(new Audit()
                {
                    Id_user = id_user,
                    Id_house = id_house,
                    Id_form = id_form,
                    Id_field = id_field,
                    NewVal = val,
                    WhenDateTime = DateTime.Now
                });
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                ctx.Values.Remove(v);
            }
        }


        private void Update(int id_user, int id_house, int id_form, string form, int id_field, string field, string newVal, ZValue zv, int id_dict, UserContext ctx, int? id_parent)
        {
            //var col = (from v in ctx.Values
            //           where v.Id_house == id_house && v.Id_form == id_form && v.Id_field == id_field && v.Id_parent == id_parent
            //           select v).ToList();   

            string oldVal = zv.Value;
            if (oldVal != newVal) // Обновляем если новое значение отличается от старого
            {
                zv.Value = newVal;
                zv.Id_parent = id_parent;
                //ZValue zv = null; 
                //if (col.Count() > 0)
                //{
                //    zv = col.First();
                //    oldVal = zv.Value;
                //    zv.Value = val;
                //    zv.Id_parent = id_parent;
                //   // v.Warning = "";
                //}
                //else
                //{
                //    // Небольшой костыль для пдн
                //    // Поскольку реально данных вставляться не будет в таблицу Values, то можно взять любой элемент, берем первый
                //    Boolean? pdn = (from f in ctx.Fields
                //                    where f.Id == id_field
                //                    select f.Personal).First();
                //    if (pdn == true)
                //    {
                //        zv = ctx.Values.First();
                //        zv.Id_house = id_house;
                //        zv.Id_form = id_form;
                //        zv.Id_field = id_field;
                //        zv.Value = val;
                //        zv.Id_parent = id_parent;
                //    }
                //    else
                //    {

                //        zv = ctx.Values.Add(new ZValue()
                //        {
                //            Id_house = id_house,
                //            Id_form = id_form,
                //            Id_field = id_field,
                //            Value = val,
                //            Id_parent = id_parent/*,
                //        Warning = ""*/
                //        });
                //    }
                //}
                try
                {

                    ctx.SaveChanges();

                    // update House table if needed
                    if (form == "Основные данные")
                        UpdateHouse(id_house, id_dict, field, newVal, ctx);

                    ctx.SaveChanges();

                    //// Аудит
                    var au = ctx.Audits.Add(new Audit()
                    {
                        Id_user = id_user,
                        Id_house = id_house,
                        Id_form = id_form,
                        Id_field = id_field,
                        OldVal = oldVal,
                        NewVal = newVal,
                        WhenDateTime = DateTime.Now
                    });
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    ctx.Values.Remove(zv);
                }
            }
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
            else if (field == "Код управляющей / обслуживающей организации (справочник №24)")
                house.id_oorg = n;
            else if (field == "Тип дома (справочник №20)")
                house.id_tdoma = n;
            else if (field == "Серия, тип проекта")
                house.seriya = val;
            else if (field == "Дата ввода в эксплуатацию")
                house.date_exp = string.IsNullOrEmpty(val) ? null : (DateTime?)DateTime.Parse(val.ToString());
            else if (field == "Количество этажей, наибольшее")
                house.etaz_max = n;
            else if (field == "Количество муниципальных квартир")
                house.kolmunkv = n;
            else if (field == "Всего общая площадь квартир")
                house.ploo = n;
            else if (field == "Зарегистрированно постоянно")
                house.kolzp = n;
            else if (field == "Форма управления (справочник №3)")
                house.id_fupr = n;
            ctx.SaveChanges();

        }
        #endregion
    }
}
