using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAL.Models;
using ExpressionEvaluator;

namespace DAL.Classes
{
    public class FormulasEvaluator
    {
        private string Update(string val, string newVal, int id_dict)
        {
            if (string.IsNullOrEmpty(newVal) && (id_dict == 1002 || id_dict == 1003))
                newVal = "0";

            if (id_dict == 1002)
            {
                if (val == "") val = "0";
                int intNewVal = 0;
                try
                {
                    intNewVal = int.Parse(newVal);
                }
                catch (Exception ex) {
                }
                val = (int.Parse(val) + intNewVal).ToString();
            }
            else if (id_dict == 1003)
            {
                if (val == "") val = "0";
                float floatNewVal = 0;
                try
                {
                    floatNewVal = float.Parse(newVal);
                }
                catch (Exception ex) {
                }
                val = (float.Parse(val) + floatNewVal).ToString();
            }
            else
                val = newVal;
            return val;
        }

        public void Evaluate(UserContext ctx, int id_house, int id_form, int? id_parent)
        {
            //foreach (var o in (from v in ctx.Values
            //                   where v.Id_form == id_form && v.Id_house == id_house && ((v.Id_parent == id_parent) || id_parent == null)
            //                   select v))
            //{
            //    o.Warning = "";
            //}
            //ctx.SaveChanges();

            var flst = new List<ExpandoObject>();
            foreach (var o in (from fm in ctx.Formulas
                               join fl in ctx.Fields on fm.Id_field equals fl.Id
                               join v in ctx.DValues on
                               new { Id_field = fl.Id, Id_form = id_form, Id_house = id_house, Id_parent = id_parent }
                               equals
                               new { Id_field = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house, Id_parent = v.Id_parent }
                               into outer0
                               from v0 in outer0.DefaultIfEmpty()
                               where fl.Id_form == id_form
                               select new { fm, fl, v0}))
            {
                dynamic f = new ExpandoObject();
                if (o.v0 != null)
                {
                    f.Id = o.v0.Id;
                    f.Value = o.v0.Value;
                }
                else
                {
                    f.Id = 0;
                    f.Value = null;
                }
                //r.Id_dict = o.f.Id_dict;
                //r.Operation = o.ro.Operation;
                f.Predicate = o.fm.Predicate;
                f.Id_field = o.fl.Id;
                //r.RuleTitle = o.r.Title;
                flst.Add(f);
            }

            var wlst = new List<ExpandoObject>();
            foreach (dynamic o in flst)
            {
                var result = o.Predicate;
                foreach (Match m in Regex.Matches(o.Predicate, @"\[(.*?)\]\.?(\[(.*?)\])?"))
                {
                    if (m.Value != "")
                    {
                        string form = "";
                        string field = "";

                        string[] tmp = m.Value.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Count() > 1)
                        {
                            form = tmp[0].Replace("[", "").Replace("]", "");
                            field = tmp[1].Replace("[", "").Replace("]", "");
                        }
                        else
                        {
                            form = "Основные данные";
                            field = tmp[0].Replace("[", "").Replace("]", "");
                        }

                        string val = "";
                        if (id_parent == 0)
                        {
                            // Основные данные
                            foreach (var v1 in (from f in ctx.Fields
                                                join fr in ctx.Forms on f.Id_form equals fr.Id
                                                join v in ctx.DValues on
                                                new { Id_form = f.Id_form, Id_field = f.Id }
                                                equals
                                                new { Id_form = v.Id_form, Id_field = v.Id_field }
                                                where fr.Title == form && f.Title == field && v.Id_house == id_house
                                                select new { v, f }))
                            {
                                val = Update(val, v1.v.Value, v1.f.Id_dict);
                            }
                        }
                        else
                        {
                            // Другие формы
                            foreach (var v1 in (from f in ctx.Fields
                                                join fr in ctx.Forms on f.Id_form equals fr.Id
                                                join v in ctx.DValues on
                                                new { Id_form = f.Id_form, Id_field = f.Id, Id_parent = id_parent }
                                                equals
                                                new { Id_form = v.Id_form, Id_field = v.Id_field, Id_parent = v.Id_parent }
                                                where fr.Title == form && f.Title == field && v.Id_house == id_house
                                                select new { v, f }))
                            {
                                val = Update(val, v1.v.Value, v1.f.Id_dict);
                            }
                        }
                        result = result.Replace(m.Value, val);
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    //string value = o.Value;
                    //if (string.IsNullOrEmpty(value) && (o.Id_dict == 1002 || o.Id_dict == 1003))
                    //    value = "0";

                    try
                    {
                        var p = new CompiledExpression(result);
                        p.Compile();
                        string res = p.Eval().ToString();
      
                        dynamic w = new ExpandoObject();
                        w.Id = o.Id;
                        w.Id_field = o.Id_field;
                        w.Value = res;
                        wlst.Add(w);
            
                    }
                    catch (Exception ex)
                    {
                        int tt = 4;
                        tt = 4;
                    }
                }
            }

            int oid = -1;
            foreach (dynamic w in wlst)
            {
                if (oid != w.Id)
                {
                    oid = w.Id;
                    bool found = false;
                    if (oid != 0)
                    {
                        foreach (var zv in (from x in ctx.Values
                                            where x.Id == oid
                                            select x))
                        {
                            if (string.IsNullOrEmpty(zv.Value))
                            {
                                zv.Value = w.Value;
                                found = true;
                            }
                        }
                    }
                    else
                    {
                        ZValue v = ctx.Values.Add(new ZValue()
                        {
                            Id_house = id_house,
                            Id_form = id_form,
                            Id_field = w.Id_field,
                            Value = w.Value,
                            Id_parent = id_parent
                        });
                        found = true;
                    }

                    if (found)
                    {
                        ////// Аудит
                        //var au = ctx.Audits.Add(new Audit()
                        //{
                        //    Id_user = id_user,
                        //    Id_house = id_house,
                        //    Id_form = id_form,
                        //    Id_field = id_field,
                        //    OldVal = oldVal,
                        //    NewVal = val,
                        //    WhenDateTime = DateTime.Now
                        //});
                        //ctx.SaveChanges();
                    }
                }
            }
            ctx.SaveChanges();
        }
    }    
}
