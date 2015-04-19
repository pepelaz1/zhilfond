using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;

namespace DAL.Classes
{
    public static class Utils
    {
        static readonly ISettingsRepository _repository = new SettingsRepository();

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }


        public static string ResolveBaloonTemplate(int Id_house)
        {
            var templateBaloon = _repository.Get().BaloonTemplate;
            var result = templateBaloon;
            foreach (Match m in Regex.Matches(templateBaloon, @"\[(.*?)\]\.\[(.*?)\]"))  //  [текст].[текст]  ,где текст - нежадный
            {
                if (m.Value != "")
                {
                    try
                    {
                        string[] templateValue;
                        templateValue = m.Value.Split(new char[] { '.' }, StringSplitOptions.None);
                        string formName = templateValue[0].Replace("[", "").Replace("]", "");
                        string fieldName = templateValue[1].Replace("[", "").Replace("]", "");

                        using (var ctx = new UserContext())
                        {
                            DValue val = (from f in ctx.Fields
                                          join form in ctx.Forms on f.Id_form equals form.Id
                                          join v in ctx.DValues on
                                          new { Id_form = f.Id_form, Id_field = f.Id, Id_house = Id_house }
                                          equals
                                          new { Id_form = v.Id_form, Id_field = v.Id_field, Id_house = v.Id_house }
                                          where f.Id_form == v.Id_form && f.Title == fieldName
                                          select v).FirstOrDefault();

                            string fieldValueResult;

                            if (val != null)
                            {
                                if (fieldName.ToLower().Contains("справочник"))
                                {
                                    int toLINQintVal = int.Parse(val.Value);
                                    DictValue dictVal = (from dict in ctx.DictsValues
                                                    where dict.Id == toLINQintVal
                                                      select dict).FirstOrDefault();
                                    if (dictVal == null) fieldValueResult = "[Значение не задано]";
                                    else fieldValueResult = "'" + dictVal.Value + "'";
                                }
                                else
                                {
                                    fieldValueResult = val.Value;
                                }
                            }
                            else
                            {
                                fieldValueResult = "[Значение не задано]";
                            }

                            result = result.Replace(m.Value, fieldValueResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }
    }
}
