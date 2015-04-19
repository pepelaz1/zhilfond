using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DAL.Models;
using ExpressionEvaluator;

namespace DAL.Classes
{
    public class RulesValidator
    {

        private string Update(string val, string part, int id_dict)
        {
            string t = part;
            if (string.IsNullOrEmpty(t) && (id_dict == 1002 || id_dict == 1003))
                t = "0";

            if (id_dict == 1002)
            {
                if (val == "") val = "0";
                val = (int.Parse(val) + int.Parse(t)).ToString();
            }
            else if (id_dict == 1003)
            {
                if (val == "") val = "0";
                val = (float.Parse(val) + float.Parse(t)).ToString();
            }
            else
                val = t;
            return val;
        }

        public void Validate(UserContext ctx, int id_house, int id_form, int? id_parent)
        {
            foreach (var o in (from v in ctx.Values
                               where v.Id_form == id_form && v.Id_house == id_house && ((v.Id_parent == id_parent)  || id_parent == null)
                               select v))
            {
                o.Warning = "";
            }
            ctx.SaveChanges();

            var rlst = new List<ExpandoObject>();
            foreach (var o in (from r in ctx.Rules
                               join ro in ctx.RuleOperations on r.Id_operation equals ro.Id
                               join f in ctx.Fields on r.Id_field equals f.Id
                               join v in ctx.DValues on
                               new { Id_field = f.Id, Id_form = id_form, Id_house = id_house, Id_parent = id_parent }
                               equals
                               new { Id_field = v.Id_field, Id_form = v.Id_form, Id_house = v.Id_house, Id_parent = v.Id_parent }
                               into outer0
                               from v0 in outer0.DefaultIfEmpty()
                               where f.Id_form == id_form
                               select new { r, ro, v0,f }))
            {
                dynamic r = new ExpandoObject();
                if (o.v0 != null)
                {
                    r.Id = o.v0.Id;
                    r.Value = o.v0.Value;
                }
                else
                {
                    r.Id = 0;
                    r.Value = null;
                }
                r.Id_dict = o.f.Id_dict;
                r.Operation = o.ro.Operation;
                r.Predicate = o.r.Predicate;
                r.RuleTitle = o.r.Title;
                rlst.Add(r);
            }

            var wlst = new List<ExpandoObject>();
            foreach(dynamic o in rlst)
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
                    string value = o.Value;
                    if (string.IsNullOrEmpty(value) && (o.Id_dict == 1002 || o.Id_dict == 1003))
                        value = "0";

                    try
                    {
                        string operation = o.Operation == "=" ? "==" : o.Operation;

                        string expr = value + operation + result;
                        var p = new CompiledExpression(expr);
                        p.Compile();
                        string res = p.Eval().ToString();
                        if (res.ToUpper() == "FALSE")
                        {
                            dynamic w = new ExpandoObject();
                            w.Id = o.Id;
                            w.Msg = "Не выполнено правило: " + o.RuleTitle;
                            wlst.Add(w);
                        }
                    }
                    catch(Exception ex)
                    {
                        int tt = 4;
                        tt = 4;
                    }
                }
            }


            int oid = 0;
            foreach (dynamic w in wlst)
            {
                if (oid != w.Id)
                {
                    oid = w.Id;
                    foreach (var zv in (from x in ctx.Values
                                        where x.Id == oid
                                        select x))
                    {
                        zv.Warning = w.Msg;
                    }                   
                }               
            }
            ctx.SaveChanges();

        }
    }
}
